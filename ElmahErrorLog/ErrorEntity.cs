using System;
using System.Linq;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Elmah;
using System.Collections;
using Microsoft.WindowsAzure;
using System.Configuration;


namespace ElmahErrorLog
{

    public class ErrorEntity : TableEntity
    {
        public string SerializedError { get; set; }

        public ErrorEntity() { }
        public ErrorEntity(Error error)
            : base(string.Empty, (DateTime.MaxValue.Ticks - DateTime.UtcNow.Ticks).ToString("d19"))
        {
            this.SerializedError = ErrorXml.EncodeString(error);
        }
    }

    public class TableErrorLog : ErrorLog
    {
        private const string ElmahTableName = "elmaherrors";

        private readonly string _connectionString;

        public override ErrorLogEntry GetError(string id)
        {
            var elmahTable = CloudStorageAccount.Parse(_connectionString).CreateCloudTableClient().GetTableReference(ElmahTableName);
            var errors = from errorEntity in elmahTable.CreateQuery<ErrorEntity>()
                         where errorEntity.PartitionKey.Equals(string.Empty) && errorEntity.RowKey.Equals(id)
                         select errorEntity;

            return new ErrorLogEntry(this, id, ErrorXml.DecodeString(errors.Single().SerializedError));
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            var count = 0;
            var elmahTable = CloudStorageAccount.Parse(_connectionString).CreateCloudTableClient().GetTableReference(ElmahTableName);
            var errors = (from errorEntity in elmahTable.CreateQuery<ErrorEntity>()
                          where errorEntity.PartitionKey.Equals(string.Empty)
                          select errorEntity).Take((pageIndex + 1) * pageSize).ToList().Skip(pageIndex * pageSize);

            foreach (var error in errors)
            {
                errorEntryList.Add(new ErrorLogEntry(this, error.RowKey, ErrorXml.DecodeString(error.SerializedError)));
                count += 1;
            }

            return count;
        }

        public override string Log(Error error)
        {
            var entity = new ErrorEntity(error);
            var elmahTable = CloudStorageAccount.Parse(_connectionString).CreateCloudTableClient().GetTableReference(ElmahTableName);
            elmahTable.Execute(TableOperation.Insert(entity));
            return entity.RowKey;
        }

        public TableErrorLog(IDictionary config)
        {
            _connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
            Initialize();
        }

        public TableErrorLog(string connectionString)
        {
            this._connectionString = connectionString;
            Initialize();
        }

        void Initialize()
        {
            CloudStorageAccount.Parse(_connectionString).CreateCloudTableClient().GetTableReference(ElmahTableName).CreateIfNotExists();
        }
    }
}

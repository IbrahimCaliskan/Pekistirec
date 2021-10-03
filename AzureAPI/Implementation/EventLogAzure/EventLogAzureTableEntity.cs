using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAPI.Implementation.EventLogAzure
{
    internal class EventLogAzureTableEntity : TableEntity
    {
        public string UserIP { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Tip { get; set; }
        public string Mesaj { get; set; }
        public string HttpMethod { get; set; }
        public string RawUrl { get; set; }
        public string UrlReferrer { get; set; }
        public string HttpBody { get; set; }

        public EventLogAzureTableEntity() : base()
        {

        }

        public EventLogAzureTableEntity(EventLogAzureEntity entity)
        {
            PartitionKey = entity.PartitionKey;            
            RowKey = entity.RowKey;
            
            UserIP = entity.UserIP;
            UserId = entity.UserId;
            UserName = entity.UserName;
            Controller = entity.Controller;
            Action = entity.Action;
            Tip = entity.Tip;
            Mesaj = entity.Mesaj;
            HttpMethod = entity.HttpMethod;
            RawUrl = entity.RawUrl;
            UrlReferrer = entity.UrlReferrer;
            HttpBody = entity.HttpBody;
        }

        public EventLogAzureEntity ToEventLogAzureEntity()
        {
            return new EventLogAzureEntity
            {
                UserIP = this.UserIP,
                UserId = this.UserId,
                UserName = this.UserName,
                Controller = this.Controller,
                Action = this.Action,
                Tip = this.Tip,
                Mesaj = this.Mesaj,
                HttpMethod = this.HttpMethod,
                RawUrl = this.RawUrl,
                UrlReferrer = this.UrlReferrer,
                HttpBody = this.HttpBody
            };
        }
    }
}

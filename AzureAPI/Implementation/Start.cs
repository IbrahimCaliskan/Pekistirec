using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAPI.Implementation
{
    public class Start
    {
        public Start()
        {
            new Implementation.DokumanBlob().CreateIfNotExist();
            new Implementation.ProfilAvatarBlob().CreateIfNotExist();
            new Implementation.EventLogAzure.EventLogAzureTable().CreateIfNotExist();
        }
    }
}

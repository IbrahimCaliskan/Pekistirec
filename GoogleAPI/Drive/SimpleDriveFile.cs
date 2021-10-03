using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAPI.Drive
{
    public class SimpleDriveFile
    {
        public string AlternateLink { get; set; }
        public string Title { get; set; }

        public SimpleDriveFile(Google.Apis.Drive.v2.Data.File driveFile)
        {
            this.AlternateLink = driveFile.AlternateLink;
            this.Title = driveFile.Title;
        }
    }
}

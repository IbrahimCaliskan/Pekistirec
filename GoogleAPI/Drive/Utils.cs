using Google.Apis.Authentication;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Requests;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace GoogleAPI.Drive
{
    public class Utils
    {
        /// <summary>
        /// Build a Drive service object.
        /// </summary>
        /// <param name="credentials">OAuth 2.0 credentials.</param>
        /// <returns>Drive service object.</returns>
        public static Google.Apis.Drive.v2.DriveService BuildService(IAuthenticator auth)
        {
            return new Google.Apis.Drive.v2.DriveService(auth);
        }

        public static File createFolder(DriveService service, string title, string description)
        {
            File body = new File();
            body.Title = title;
            body.Description = description;
            body.MimeType = "application/vnd.google-apps.folder";

            File file = service.Files.Insert(body).Fetch();
            return file;
        }

        /// <summary>
        /// Insert new file.
        /// </summary>
        /// <param name="service">Drive API service instance.</param>
        /// <param name="title">Title of the file to insert, including the extension.</param>
        /// <param name="description">Description of the file to insert.</param>
        /// <param name="parentId">Parent folder's ID.</param>
        /// <param name="mimeType">MIME type of the file to insert.</param>
        /// <param name="filename">Filename of the file to insert.</param>
        /// <returns>Inserted file metadata, null is returned if an API error occurred.</returns>
        public static File insertFile(DriveService service, String title, String description, String parentId, String mimeType, String filename)
        {
            // File's metadata.
            File body = new File();
            body.Title = title;
            body.Description = System.Text.Encoding.UTF32.GetString(System.Text.Encoding.UTF32.GetBytes(description));
            body.MimeType = mimeType;
            body.WritersCanShare = true;
            body.Editable = true;

            // Set the parent folder.
            if (!String.IsNullOrEmpty(parentId))
            {
                body.Parents = new List<ParentReference>() { new ParentReference() { Id = parentId } };
            }

            // File's content.
            byte[] byteArray = System.IO.File.ReadAllBytes(filename);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

            try
            {
                FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, mimeType);
                request.Convert = true;
                request.Upload();
                File file = request.ResponseBody;

                // Uncomment the following line to print the File ID.
                // Console.WriteLine("File ID: " + file.Id);

                return file;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///// <summary>
        ///// Insert new file.
        ///// </summary>
        ///// <param name="service">Drive API service instance.</param>
        ///// <param name="title">Title of the file to insert, including the extension.</param>
        ///// <param name="description">Description of the file to insert.</param>
        ///// <param name="parentId">Parent folder's ID.</param>
        ///// <param name="mimeType">MIME type of the file to insert.</param>
        ///// <param name="filename">Filename of the file to insert.</param>
        ///// <returns>Inserted file metadata, null is returned if an API error occurred.</returns>
        //public static File insertFile(DriveService service, String title, String description, String parentId, String mimeType, String filename)
        //{
        //    // File's metadata.
        //    File body = new File();
        //    body.Title = title;
        //    body.Description = description;
        //    body.MimeType = mimeType;            

        //    // Set the parent folder.
        //    if (!String.IsNullOrEmpty(parentId))
        //    {
        //        body.Parents = new List<ParentReference>() { new ParentReference() { Id = parentId } };
        //    }

        //    // File's content.
        //    byte[] byteArray = System.IO.File.ReadAllBytes(filename);
        //    System.IO.MemoryStream stream = new System.IO.MemoryStream(byteArray);

        //    try
        //    {
        //        FilesResource.InsertMediaUpload request = service.Files.Insert(body, stream, mimeType);
        //        request.Convert = true;                
        //        request.Upload();

        //        File file = request.ResponseBody;

        //        // Uncomment the following line to print the File ID.
        //        // Console.WriteLine("File ID: " + file.Id);

        //        return file;
        //    }
        //    catch (Exception)
        //    {                
        //        return null;
        //    }
        //}
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Configuration;

namespace Appdev3A_Assignment.Models
{
    public class BlobManager
    {
        private CloudBlobContainer blobContainer;

        public BlobManager(string ContainerName)
        {
            // Check if Container Name is null or empty  
            if (string.IsNullOrEmpty(ContainerName))
            {
                throw new ArgumentNullException("sampleimage", "Container Name can't be empty");
            }
            try
            {
                // Get azure table storage connection string.  
                string ConnectionString = ConfigurationManager.ConnectionStrings[ "AccountEndpoint=https://group14appdev.documents.azure.com:443/;AccountKey=qmEGindYtVSrKEs0PAjCkALxCZfrRJb5AxNN2Q8A846XAcRzixz3bjv6CDfWJD4Z6Tk5vkFmKjsD20SNd7UK2A==;"].ConnectionString;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                // Create the container and set the permission  
                if (blobContainer.CreateIfNotExists())
                {
                    blobContainer.SetPermissions(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                    );
                }
            }
            catch (Exception)
            {
                
            }
        }

        //Step 4
        //method for Upload file
        public string UploadFile(HttpPostedFileBase FileToUpload)
        {
            string AbsoluteUri;
            // Check HttpPostedFileBase is null or not 
            if (FileToUpload == null || FileToUpload.ContentLength == 0)
                return null;
            try
            {
                string FileName = Path.GetFileName(FileToUpload.FileName);
                CloudBlockBlob blockBlob;
                //Create a block blob
                blockBlob = blobContainer.GetBlockBlobReference(FileName);
                //Set the object's content type
                blockBlob.Properties.ContentType = FileToUpload.ContentType;
                //upload to blob
                blockBlob.UploadFromStream(FileToUpload.InputStream);
                // get file uri
                AbsoluteUri = blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
            return AbsoluteUri;
        }
        //Step 5
        //method to get all blob/files
        public List<string> BlobList()
        {
            List<string> _blobList = new List<string>();
            foreach (IListBlobItem item in blobContainer.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob _blobpage = (CloudBlockBlob)item;
                    _blobList.Add(_blobpage.Uri.AbsoluteUri.ToString());
                }
            }
            return _blobList;
        }
        //Step 6
        //method to Delete blob/file 
        public bool DeleteBlob(string AbsoluteUri)
        {
            try
            {
                Uri uriObj = new Uri(AbsoluteUri);
                string BlobName = Path.GetFileName(uriObj.LocalPath);

                //get block blob reference
                CloudBlockBlob blockBlob = blobContainer.GetBlockBlobReference(BlobName);
                // delete blob from container
                blockBlob.Delete();
                return true;
            }
            catch (Exception ExceptionObj)
            {

                throw ExceptionObj;
            }
        }


    }
}
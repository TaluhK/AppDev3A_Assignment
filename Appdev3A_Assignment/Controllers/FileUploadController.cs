using Appdev3A_Assignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Appdev3A_Assignment.Controllers
{
   
        // GET: FileUpload
        public class FileUploadController : Controller
        {   //Step 7
            // GET: FileUpload
            public ActionResult Index(string id)
            {
                return View();
            }

            [HttpPost]
            public ActionResult Index(HttpPostedFileBase uploadFile)
            {
                foreach (string file in Request.Files)
                {
                    uploadFile = Request.Files[file];
                }
            //Container Name - picture
            Models.BlobManager BlobManagerObj = new BlobManager("sampleimage");
                string FileAbsoluteUri = BlobManagerObj.UploadFile(uploadFile);

                return RedirectToAction("Get");
            }
            //Step 8
            //View and Delete
            public ActionResult Get()
            {
                // Container Name - picture
                BlobManager BlobManagerObj = new BlobManager("sampleimage");
                List<string> fileList = BlobManagerObj.BlobList();
                return View(fileList);
            }
            public ActionResult Delete(string uri)
            {
                // Container Name - picture  
                BlobManager BlobManagerObj = new BlobManager("sampleimage");
                BlobManagerObj.DeleteBlob(uri);
                return RedirectToAction("Get");
            }
        }
    
}
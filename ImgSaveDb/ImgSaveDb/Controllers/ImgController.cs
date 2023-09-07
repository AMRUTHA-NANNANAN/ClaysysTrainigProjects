using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImgSaveDb.Controllers
{using ImgSaveDb.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Helper_Code.Objects;
    using Models;

    
    public class ImgController : Controller
    {
        #region Private Properties  

        
        private db_imgEntities1 databaseManager = new db_imgEntities1();

        #endregion

        #region Index view method.  

        #region Get: /Img/Index method.  

        
        public ActionResult Index()
        {
              
            ImgViewModel model = new ImgViewModel { FileAttach = null, ImgLst = new List<ImgObj>() };

            try
            {
                  
                model.ImgLst = this.databaseManager.sp_get_all_files().Select(p => new ImgObj
                {
                    FileId = p.file_id,
                    FileName = p.file_name,
                    FileContentType = p.file_ext
                }).ToList();
            }
            catch (Exception ex)
            {
                 
                Console.Write(ex);
            }

              
            return this.View(model);
        }

        #endregion

        #region POST: /Img/Index  

       
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ImgViewModel model)
        {
          
            string fileContent = string.Empty;
            string fileContentType = string.Empty;

            try
            {
                 
                if (ModelState.IsValid)
                {
                      
                    byte[] uploadedFile = new byte[model.FileAttach.InputStream.Length];
                    model.FileAttach.InputStream.Read(uploadedFile, 0, uploadedFile.Length);

                     
                    fileContent = Convert.ToBase64String(uploadedFile);
                    fileContentType = model.FileAttach.ContentType;

                      
                    this.databaseManager.sp_insert_file(model.FileAttach.FileName, fileContentType, fileContent);
                }

               
                model.ImgLst = this.databaseManager.sp_get_all_files().Select(p => new ImgObj
                {
                    FileId = p.file_id,
                    FileName = p.file_name,
                    FileContentType = p.file_ext
                }).ToList();
            }
            catch (Exception ex)
            {
                 
                Console.Write(ex);
            }

             
            return this.View(model);
        }

        #endregion

        #endregion

        #region Download file methods  

        #region GET: /Img/DownloadFile  

         
        public ActionResult DownloadFile(int fileId)
        {
              
            ImgViewModel model = new ImgViewModel { FileAttach = null, ImgLst = new List<ImgObj>() };

            try
            {
                  
                var fileInfo = this.databaseManager.sp_get_file_details(fileId).First();

                  
                return this.GetFile(fileInfo.file_base6, fileInfo.file_ext);
            }
            catch (Exception ex)
            {
                 
                Console.Write(ex);
            }

              
            return this.View(model);
        }

        #endregion

        #endregion

        #region Helpers  

        #region Get file method.  

         
        private FileResult GetFile(string fileContent, string fileContentType)
        {
              
            FileResult file = null;

            try
            {
                 
                byte[] byteContent = Convert.FromBase64String(fileContent);
                file = this.File(byteContent, fileContentType);
            }
            catch (Exception ex)
            {
                 
                throw ex;
            }

              
            return file;
        }

        #endregion

        #endregion
    }
}
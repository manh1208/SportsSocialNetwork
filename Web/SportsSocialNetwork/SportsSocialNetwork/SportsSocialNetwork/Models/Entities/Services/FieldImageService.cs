using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Teek.Models;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IFieldImageService
    {
        void saveImage(int id, IEnumerable<HttpPostedFileBase> images);
    }
    public partial class FieldImageService : IFieldImageService
    {
        public void saveImage(int id, IEnumerable<HttpPostedFileBase> images)
        {
            string containFolder = "FieldImages";
            FieldImage fi = null;
            List<HttpPostedFileBase> li = images.ToList();
            FileUploader _fileUploaderService = new FileUploader();

            foreach (var item in li)
            {
                string filePath = _fileUploaderService.UploadImage(item, containFolder);
                fi = new FieldImage();
                fi.FieldId = id;
                fi.Image = filePath;
                this.Create(fi);
                this.Save();
            }
        }
    }
}
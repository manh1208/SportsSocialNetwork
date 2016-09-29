using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Teek.Models;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPlaceImageService
    {
        void saveImage(int id, IEnumerable<HttpPostedFileBase> uploadImages);
    }
    public partial class PlaceImageService : IPlaceImageService
    {
        public void saveImage(int id, IEnumerable<HttpPostedFileBase> uploadImages)
        {
            string containFolder = "PlaceImages";
            PlaceImage pi = null;
            List<HttpPostedFileBase> ui = uploadImages.ToList();
            FileUploader _fileUploadService = new FileUploader();
            for(int i = 0; i < ui.Count; i++)
            {
                string filePath = _fileUploadService.UploadImage(ui[i], containFolder);
                pi = new PlaceImage();
                pi.PlaceId = id;
                pi.Image = filePath;
                this.Create(pi);
                this.Save();
            }
        }
    }
}
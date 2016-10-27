using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Teek.Models;

namespace SportsSocialNetwork.Models.Entities.Services
{
    public partial interface IPostImageService
    {
        void saveImage(int id, IEnumerable<HttpPostedFileBase> images);

        IEnumerable<PostImage> GetAllPostImage(int postId);
    }
    public partial class PostImageService : IPostImageService
    {
        public void saveImage(int id, IEnumerable<HttpPostedFileBase> images)
        {
            string containFolder = "FieldImages";
            PostImage fi = null;
            List<HttpPostedFileBase> li = images.ToList();
            FileUploader _fileUploaderService = new FileUploader();

            foreach (var item in li)
            {
                string filePath = _fileUploaderService.UploadImage(item, containFolder);
                fi = new PostImage();
                fi.PostId = id;
                fi.Image = filePath;
                this.Create(fi);
                this.Save();
            }
        }

        public IEnumerable<PostImage> GetAllPostImage(int postId)
        {
            return this.GetActive(x => x.PostId == postId);

        }
    }
}
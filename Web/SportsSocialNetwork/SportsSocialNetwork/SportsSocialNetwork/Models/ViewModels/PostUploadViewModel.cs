using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.ViewModels
{
    public class PostUploadViewModel : PostViewModel
    {
        public List<HttpPostedFileBase> UploadImage { get; set; }
    }
}
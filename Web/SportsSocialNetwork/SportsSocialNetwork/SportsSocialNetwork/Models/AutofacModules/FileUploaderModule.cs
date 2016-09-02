using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;

namespace Teek.Models.AutofacModules
{

    public class FileUploaderModule : Autofac.Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileUploader>()
                .SingleInstance();
        }

    }

}
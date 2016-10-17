using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Windows.Forms;

namespace SportsSocialNetwork.Models.Utilities
{
    public static class Utils
    {
        public static string GetEnumDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

        public static string GetHostName()
        {
            //Return variable declaration
            var appPath = string.Empty;

            //Getting the current context of HTTP request
            var context = HttpContext.Current;

            //Checking the current context content
            if (context != null)
            {
                //Formatting the fully qualified website url/name
                appPath = string.Format("{0}://{1}{2}{3}",
                                        context.Request.Url.Scheme,
                                        context.Request.Url.Host,
                                        context.Request.Url.Port == 80
                                            ? string.Empty
                                            : ":" + context.Request.Url.Port,
                                        context.Request.ApplicationPath);
            }

            if (!appPath.EndsWith("/"))
                appPath += "/";

            return appPath;
        }

        public static string GenerateQRCode(string payloadString, QRCodeGenerator.ECCLevel eccLevel)
        {
            using (var generator = new QRCodeGenerator())
            {
                using (var data = generator.CreateQrCode(payloadString, eccLevel))
                {
                    using (var code = new QRCode(data))
                    {
                        using (var bitmap = code.GetGraphic(20))
                        {

                            var icon = new Bitmap(HostingEnvironment.MapPath("/Content/images/logo_new.png"));

                            PictureBox pictureBox = new PictureBox();
                            pictureBox.BackgroundImage = code.GetGraphic(20, Color.Black, Color.White, icon, 15);

                            //Set the SizeMode to center the image.
                            pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;

                            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;

                            var fileExtension = ".jpg";
                            var rootPath = Setting.SSNROOTPATH;

                            var fileName = $"{DateTime.Now.ToString("yyyyMMddHHmmsstttt")}_{Guid.NewGuid().ToString()}{fileExtension}";
                            var relativeFilePath = Path.Combine(rootPath.StartsWith("~") ? rootPath.Substring(1) : rootPath, "QRCodeImage", fileName);
                            var fullFilePath = HostingEnvironment.MapPath(relativeFilePath);
                            var fullFolderPath = Path.GetDirectoryName(fullFilePath);

                            if (!Directory.Exists(fullFolderPath))
                            {
                                Directory.CreateDirectory(fullFolderPath);
                            }

                            pictureBox.BackgroundImage.Save(fullFilePath, ImageFormat.Jpeg);
                            return relativeFilePath.Replace("\\", "/"); ;
                        }
                    }
                }
            }
        }
    }

   
}
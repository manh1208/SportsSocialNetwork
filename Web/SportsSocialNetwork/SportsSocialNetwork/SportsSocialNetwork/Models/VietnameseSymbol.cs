using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BanleWebsite.Models
{
    public class VietnameseSymbol
    {
        public object Name { get; internal set; }

        public string ClearSymbol(string text)
        {
            text = text.ToLower();
            StringBuilder sb = new StringBuilder();
            foreach (var item in text)
            {
                switch(item)
                {
                    case 'á': case 'à': case 'ả': case 'ã': case 'ạ': sb.Append('a'); break;
                    case 'í': case 'ì': case 'ỉ': case 'ĩ': case 'ị': sb.Append('i'); break;
                    case 'ú': case 'ù': case 'ủ': case 'ũ': case 'ụ': sb.Append('u'); break;
                    case 'é': case 'è': case 'ẻ': case 'ẽ': case 'ẹ': sb.Append('e'); break;
                    case 'ó': case 'ò': case 'ỏ': case 'õ': case 'ọ': sb.Append('o'); break;
                    case 'ấ': case 'ầ': case 'ẩ': case 'ẫ': case 'ậ': case 'â':  sb.Append('a'); break;
                    case 'ắ': case 'ằ': case 'ẳ': case 'ẵ': case 'ặ': case 'ă': sb.Append('a'); break;
                    case 'ứ': case 'ừ': case 'ử': case 'ữ': case 'ự': case 'ư': sb.Append('u'); break;
                    case 'ế': case 'ề': case 'ể': case 'ễ': case 'ệ': case 'ê': sb.Append('e'); break;
                    case 'ố': case 'ồ': case 'ổ': case 'ỗ': case 'ộ': case 'ô': sb.Append('o'); break;
                    case 'ớ': case 'ờ': case 'ở': case 'ỡ': case 'ợ': case 'ơ': sb.Append('o'); break;
                    case 'ý': case 'ỳ': case 'ỷ': case 'ỹ': case 'ỵ': sb.Append('y'); break;
                    case 'đ': sb.Append('d'); break;
                    default: sb.Append(item); break;
                }
            }
            string result = sb.ToString();
            sb.Clear();
            return result;
        }
    }

    //public string GenerateSlug()
    //{
    //    VietnameseSymbol vs = new VietnameseSymbol();
    //    string phrase = string.Format("{0}-{1}", vs.ClearSymbol(Name), ID);

    //    string str = RemoveAccent(phrase).ToLower();
    //    str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
    //    str = Regex.Replace(str, @"\s+", " ").Trim();
    //    str = str.Substring(0, str.Length <= 200 ? str.Length : 200).Trim();
    //    str = Regex.Replace(str, @"\s", "-");
    //    return str;
    //}

    //private string RemoveAccent(string text)
    //{
    //    byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(text);
    //    return System.Text.Encoding.ASCII.GetString(bytes);
    //}
}
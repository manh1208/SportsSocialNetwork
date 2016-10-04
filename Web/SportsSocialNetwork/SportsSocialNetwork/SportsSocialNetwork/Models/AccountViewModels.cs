using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

//using System.Threading;
//using System.Web.UI;
//using System.Timers;

namespace SportsSocialNetwork.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "Tên tài khoản không được để trống")]
        [Display(Name = "Username")]
        public string Username { get; set; }
                

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class UpdateViewModel
    {

        //[Required(ErrorMessage = "Tên tài khoản không thể sửa")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "Mật khẩu không được để trống")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        //[DataType(DataType.Password)]
        //[Display(Name = "Password")]
        //public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "Vui lòng nhập đúng mật khẩu của bạn")]
        //public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Họ và tên không nên để trống")]
        [DataType(DataType.Text)]
        [Display(Name = "Fullname")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Giới tính của bạn không nên để trống")]
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Đừng quên cho chúng tôi biết môn thể thao yêu thích của bạn")]
        [Display(Name = "Hobby")]
        public string Hobby { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Birthday")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yy}")]
        public System.DateTime Birthday { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "District")]
        public string District { get; set; }

        [Display(Name = "Ward")]
        public string Ward { get; set; }

        [Display(Name = "IsPlaceOwner")]
        public bool IsPlaceOwner { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage ="Tên tài khoản không được để trống")]        
        [Display(Name ="Username")]
        public string Username { get; set; }       

        [Required(ErrorMessage ="Email không được để trống")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Vui lòng nhập đúng mật khẩu của bạn")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Họ và tên không nên để trống")]
        [DataType(DataType.Text)]
        [Display(Name = "Fullname")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "Giới tính của bạn không nên để trống")]
        [Display(Name = "Gender")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Đừng quên cho chúng tôi biết môn thể thao yêu thích của bạn")]
        [Display(Name = "Hobby")]
        public string Hobby { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Birthday")]
        public System.DateTime Birthday { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name ="City")]
        public string City { get; set; }

        [Display(Name ="District")]
        public string District { get; set; }

        [Display(Name ="Ward")]
        public string Ward { get; set; }

        [Display(Name ="IsPlaceOwner")]
        public bool IsPlaceOwner { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

using SportsSocialNetwork.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SportsSocialNetwork.Areas.Admin.Models
{
    public class UpdateAccountViewModel :AspNetUserViewModel
    {
        public List<AspNetRoleViewModel> AspNetRoles { get; set; }
        public string RoleId { get; set; }
        public void CreateRole()
        {
            this.RoleId = this.AspNetRoles.FirstOrDefault().Id;
        }
    }

    public class PrepareUpdateViewModel
    {
        public AccountDetailViewModel Detail { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public IEnumerable<SelectListItem> Provinces { get; set; }
        public IEnumerable<SelectListItem> Districts { get; set; }
        public IEnumerable<SelectListItem> Wards { get; set; }

    }

    public class AccountDetailViewModel : AspNetUserViewModel
    {
        public string AddressString { get; set; }
        public string BirthdayString { get; set; }
        public List<AspNetRoleViewModel> AspNetRoles { get; set; }
        public string RoleName { get; set; }
        public string GenderString { get; set; }

        public void CreateAddressString()
        {
            this.AddressString = this.Address;
            this.AddressString += this.Ward != null ? " - " + this.Ward : "";
            this.AddressString += this.District != null ? " - " + this.District : "";
            this.AddressString += this.City != null ? " - " + this.City : "";
        }

        public void CreateBirthdayString()
        {
            if (this.Birthday.HasValue)
            {
                this.BirthdayString = this.Birthday.Value.ToString("dd-MM-yyyy");
            }
        }

        public void CreateRole()
        {
            this.RoleName = this.AspNetRoles.FirstOrDefault().Name;
        }
    }
}
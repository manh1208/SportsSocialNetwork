using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Enumerable
{
    public enum OrderStatus
    {
      
    }


    public enum UserRole
    {
        [Description("Thành viên")]
        Member = 1,
        [Description("Chủ sân")]
        PlaceOwner = 2,
        [Description("Quản trị viên")]
        Admin = 3,
        [Description("Moderator")]
        Moderator = 4
    }

    public enum Gender
    {
        [Description("Nam")]
        Male = 1,
        [Description("Nữ")]
        Female = 2,
        [Description("Khác")]
        Other = 3,
    }

    public enum PlaceStatus
    {
        [Description("Đang hoạt động")]
        Active = 1,
        [Description("Từ chối")]
        Unapproved = 2,
        [Description("Đang chờ")]
        Pending = 3,
        [Description("Đang sửa chữa")]
        Repairing = 4,

    }

    public enum UserStatus
    {
        [Description("Đang hoạt động")]
        Active = 1,
        [Description("Từ chối")]
        Unapproved = 2,
        [Description("Đang chờ")]
        Pending = 3,
        [Description("Bị cấm")]
        Banned = 4,
    }

    public enum EventStatus
    {
        [Description("Đang diễn ra")]
        Operating = 1,
        [Description("Kết thúc")]
        Closed = 0
    }
}
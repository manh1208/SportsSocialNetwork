using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SportsSocialNetwork.Models.Enumerable
{
    

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
        Other = 3
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
        Repairing = 4

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
        Banned = 4
    }

    public enum FieldStatus
    {
        [Description("Đang hoạt động")]
        Active = 1,
        [Description("Đang sửa chữa")]
        Repairing = 2,
        [Description("Ngừng hoạt động")]
        Deactive = 3
    }

    public enum OrderStatus
    {
        [Description("Đang chờ")]
        Pending = 1,
        [Description("Đã chấp nhận")]
        Approved = 2,
        [Description("Không chấp nhận")]
        Unapproved = 3,
        [Description("Hủy")]
        Cancel = 4,
        [Description("Đã nhận sân")]
        CheckedIn = 5,
    }

    public enum FieldScheduleStatus
    {
        [Description("Sửa chữa")]
        Repair = 1,
        [Description("Đã được đặt")]
        Booked = 2,
        [Description("Sự kiện")]
        Event = 3,
        [Description("Khác")]
        Other = 4
    }

    public enum EventStatus
    {
        [Description("Đang diễn ra")]
        Operating = 1,
        [Description("Kết thúc")]
        Closed = 0
    }

    public enum OrderPaidType
    {
        [Description("Chọn thanh toán online")]
        ChosePayOnline = 1,
        [Description("Chọn thanh toán bằng tiền mặt")]
        ChosePayByCash = 2,
        [Description("Đã thanh toán bằng tiền mặt")]
        PaidByCash = 3,
        [Description("Đã thanh toán online")]
        PaidOnline = 4
    }

    public enum NotificationType
    {
        [Description("Cập nhật đơn đặt sân")]
        UpdateOrder = 1,
    }


}
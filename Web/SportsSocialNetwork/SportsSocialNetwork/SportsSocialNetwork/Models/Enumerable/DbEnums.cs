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
        [Description("Thuê sân")]
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
        [Description("Online-Chưa thanh toán")]
        ChosePayOnline = 1,
        [Description("Tiền mặt-Chưa thanh toán")]
        ChosePayByCash = 2,
        [Description("Tiền mặt-Đã thanh toán")]
        PaidByCash = 3,
        [Description("Online-Đã thanh toán")]
        PaidOnline = 4
    }

    public enum NotificationType
    {
        [Description("Post")]
        Post = 1,
        [Description("Order")]
        Order = 2,
        [Description("Invitation")]
        Invitation = 3,
        [Description("Other")]
        Other = 4,
        [Description("GroupIvitation")]
        GroupInvitation = 5,
        [Description("GroupChallengeInvitation")]
        GroupChallengeInvitation = 6,
        [Description("GroupMemberAction")]
        GroupMemberAction = 7,
        [Description("GroupPost")]
        GroupPost = 8,
        [Description("ApprovePlaceOwner")]
        ApprovePlaceOwner = 9,
        [Description("UnApprovePlaceOwner")]
        UnApprovePlaceOwner = 10,
        [Description("ShareFriendWall")]
        ShareFrdWall = 11,
        [Description("ShareToGroup")]
        ShareGroup = 12
    }

    public enum ContentPostType
    {
        [Description("")]
        TextOnly = 1,
        [Description("")]
        TextAndImage = 2,
        [Description("")]
        ImageOnly = 3,
        [Description("")]
        MultiImages = 4,
        [Description("")]
        TextAndMultiImages = 5,
        [Description("")]
        ShareEventPost = 6,
        [Description("")]
        ShareOrderPost = 7,
        [Description("")]
        SharePostPost = 8,
        [Description("")]
        ShareNewsPost = 9,
    }

    public enum GroupMemberStatus
    {
        [Description("Đã duyệt")]
        Approved = 1,
        [Description("Đang chờ duyệt")]
        Pending = 2
    }

    public enum JoinLeaveGroupResult
    {
        Leaved = 1,
        RequestSent = 2,
        ReJoined = 3,
        CancelRequest = 4,
        CannotLeave = 5
    }

    public enum GroupMemberRole
    {
        NotMember = 1,
        Member = 2,
        Admin = 3,
        PendingMember = 4
    }

    public enum ChallengeStatus
    {
        [Description("Chưa diễn ra")]
        NotOperate = 1,
        [Description("Đã đấu xong")]
        Done = 2,
        [Description("Đang chờ chấp nhận")]
        Pending = 3,
        [Description("Hủy")]
        NotAvailable = 4
    }

    public enum SharedReceiver
    {
        SenderWall = 1,
        FriendWall = 2,
        Group = 3
    }

}
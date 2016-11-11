var _loadMoreCmtActionName = "";
var _deletePostActionName = "";
var _loadAPostActionName = "";
var _loadACommentActionName = "";
var _deleteCommentActionName = "";
var _likeUnlikePostActionName = "";
var _curUserId = "";
var _goNewFeedActionName = "";
$('body').on('focus', ".magnific", function () {
    $(this).magnificPopup({
        type: 'image',
        zoom: {
            enabled: true,
            duration: 300, // don't foget to change the duration also in CSS
            opener: function (element) {
                return element.find('img');
            }
        }
    });
});

$('body').on('focus', ".imageMagnific", function () {
    $(this).magnificPopup({
        delegate: 'a',
        type: 'image',
        tLoading: 'Đang tải hình ảnh #%curr%...',
        mainClass: 'mfp-img-mobile',
        gallery: {
            enabled: true,
            navigateByImgClick: true,
            preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
        },
        image: {
            tError: '<a href="%url%">The image #%curr%</a> could not be loaded.',
            titleSrc: function (item) {
                return item.el.attr('title') +
                  '<small>by amazingSurge</small>';
            }
        },
        zoom: {
            enabled: true,
            duration: 300, // don't foget to change the duration also in CSS
            opener: function (element) {
                return element.find('img');
            }
        }
    });
});

// Example Popup Gallery
// ---------------------
//$('.imageMagnific').magnificPopup({
//    delegate: 'a',
//    type: 'image',
//    tLoading: 'Loading image #%curr%...',
//    mainClass: 'mfp-img-mobile',
//    gallery: {
//        enabled: true,
//        navigateByImgClick: true,
//        preload: [0, 1] // Will preload 0 - before current, and 1 after the current image
//    },
//    image: {
//        tError: '<a href="%url%">The image #%curr%</a> could not be loaded.',
//        titleSrc: function (item) {
//            return item.el.attr('title') +
//              '<small>by amazingSurge</small>';
//        }
//    },
//    zoom: {
//        enabled: true,
//        duration: 300, // don't foget to change the duration also in CSS
//        opener: function (element) {
//            return element.find('img');
//        }
//    }
//});

function loadGroupPost(groupId, curUserId, skip, take, actionName, loadMoreCmtActionName, deletePostActionName, loadAPostActionName, deleteCommentActionName, loadACommentActionName, likeUnlikePostActionName) {
    _loadMoreCmtActionName = loadMoreCmtActionName;
    _curUserId = curUserId;
    _deletePostActionName = deletePostActionName;
    _loadAPostActionName = loadAPostActionName;
    _deleteCommentActionName = deleteCommentActionName;
    _loadACommentActionName = loadACommentActionName;
    _likeUnlikePostActionName = likeUnlikePostActionName;
    $.ajax({
        url: actionName,
        type: 'POST',
        data: {
            groupId: groupId,
            curUserId: curUserId,
            skip: skip,
            take: take
        },

        success: function (data) {
            if (data.Succeed) {
                $(data.AdditionalData).each(function () {
                    var content = "";
                    if (this.ContentType == 1) {
                        content = textPost(this);
                    }
                    if (this.ContentType == 2) {
                        content = textImagePost(this);
                    }
                    if (this.ContentType == 3) {
                        content = imagePost(this);
                    }
                    if (this.ContentType == 4) {
                        content = multiImagePost(this);
                    }
                    if (this.ContentType == 5) {
                        content = multiImageTextPost(this);
                    }
                    var moreCmtBtn = "";
                    if (this.CommentCount > 3) {
                        moreCmtBtn = "<div id='moreCmt_" + this.Id + "'><a href='javascript:void(0)' onclick='loadMoreComt(" + this.Id + ")'>Xem thêm bình luận</a>";
                    }
                    var spanLike = "";
                    if (this.Liked) {
                        spanLike = "<span><i style='color:#62a8ea' id='likeIcon_" + this.Id + "' onclick='likeUnlikePost(" + this.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + this.Id + "' style='margin-right:10px'> " + this.LikeCount + " lượt thích</span>";
                    } else {
                        spanLike = "<span><i  id='likeIcon_" + this.Id + "' onclick='likeUnlikePost(" + this.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + this.Id + "' style='margin-right:10px'> " + this.LikeCount + " lượt thích</span>";
                    }
                    var reportDiv = "";
                    var spanLike = "";
                    if (this.Liked) {
                        spanLike = "<span><i style='color:#62a8ea' id='likeIcon_" + this.Id + "' onclick='likeUnlikePost(" + this.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + this.Id + "' style='margin-right:10px'> " + this.LikeCount + " lượt thích</span>";
                    } else {
                        spanLike = "<span><i  id='likeIcon_" + this.Id + "' onclick='likeUnlikePost(" + this.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + this.Id + "' style='margin-right:10px'> " + this.LikeCount + " lượt thích</span>";
                    }
                    var reportDiv = "";
                    if (this.AspNetUser.Id == curUserId) {
                        reportDiv = '<div style="position: absolute;right:5px;top:15px;width: 30px;">' +
                          '<div style="position: relative;width: 100%;text-align: center;margin-top: 5px;">' +
                          '<div class="dropdown">' +
                                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-lg fa-angle-down"></i>' +
                                       '</a>' +
                                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deletePost(' + this.Id + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editPost(' + this.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                                       '</ul>' +
                                   '</div>' +
                                   '</div>' +
                                   '</div>';
                    }
                    var post = "<li class='list-group-item panel' style='margin-bottom:10px' role='tabpanel' id='post_" + this.Id + "'>"
                                   + "<div class='media'>"
                                       + "<div class='media-left'>"
                                           + "<a class='avatar' href='javascript:void(0)'>"
                                                + "<img style='height:50px' class='img-responsive' src='" + this.AspNetUser.AvatarImage + "' alt='...'>"
                                            + "</a>"
                                        + "</div>"
                                        + "<div class='media-body'>"
                                            + "<p class='media-heading' style='margin-bottom:0'>"
                                            + '<a style="font-weight:bold" class="comment-author" href="/profile/index?userid='+this.AspNetUser.Id+'">' + this.AspNetUser.FullName + '</a>'
                                                + "<span style='text-shadow: none'>đã đăng một bài viết</span>"
                                            + "</p>"
                                            + "<small>" + this.PostAge + "</small>"
                                            + "<div class='profile-brief' style='white-space: pre-wrap;margin-bottom:20px'>" + content + "</div>"
                                            + "<div class='comment-actions'>"
                                            + "<div style='font-size:13px' class='text-left'>"
                                                + spanLike
                                                + "<span><i class='text-like fa fa-lg fa-comments-o'></i></span><span style='margin-right:10px' id='commentOfPost_" + this.Id + "'> " + this.CommentCount + " lượt bình luận</span>"
                                            + "</div>"
                                            + "</div>"
                                             + '<div class="panel" style="margin-bottom:0px;margin-top:15px">'
                                             + '<form id="comment-form_' + this.Id + '" class="comment-form" method="post" autocomplete="off"><input type="hidden" name="postId" value="' + this.Id + '"/>'
                                            + '<input name="content" id="contentDetail_' + this.Id + '" type="text" class="form-control input-cmnt" style="padding-right: 35px" placeholder="Viết bình luận của bạn..."/>'
                                            + '<button type="button" class="btn btn-pure btn-primary fa fa-camera" style="position: absolute;top: 2px;right: 0px;transition: right 0.2s; z-index:2" onclick="addImageComment(' + this.Id + ')"></button>'
                                            + '<div style="height:0px;overflow:hidden">'
                                            + '<input type="file" id="selectImageComment_' + this.Id + '" name="image" /></div></div></form>'
                                            + '<div id="previewImageComment_' + this.Id + '" class="example margin-0" style="display:none"><div data-role="container">'
                                            + '<div data-role="content"><div class="Document"><ul id="resultComment_' + this.Id + '" class="list-inline"></ul></div></div></div></div>'
                                            + "<div class='comments'  style='margin-top:20px' id='postComments_" + this.Id + "'>"
                                                    + postComment(this)
                                            + "</div>"
                                            + moreCmtBtn
                                        + "</div>"
                                    + "</div>"
                                    + reportDiv
                                + "</li>";
                    $(post).hide().appendTo("#listPost").fadeIn("slow");
                    document.getElementById('selectImageComment_' + this.Id).addEventListener('change', function () {
                        var tmp = $(this).attr('id');
                        var result = tmp.split("_");
                        handleFileSelectComment(event, result[1]);
                    }, false);
                })
            } else {

            }
        },
        error: function (error) {
        }
    });
}

function loadProfilePost(userId, curUserId, skip, take, actionName, loadMoreCmtActionName, deletePostActionName, loadAPostActionName, deleteCommentActionName, loadACommentActionName, likeUnlikePostActionName) {
    _loadMoreCmtActionName = loadMoreCmtActionName;
    _deletePostActionName = deletePostActionName;
    _loadAPostActionName = loadAPostActionName;
    _deleteCommentActionName = deleteCommentActionName;
    _loadACommentActionName = loadACommentActionName;
    _likeUnlikePostActionName = likeUnlikePostActionName;
    _curUserId = curUserId;
    $.ajax({
        url: actionName,
        type: 'POST',
        data: {
            userId: userId,
            curUserId: curUserId,
            skip: skip,
            take: take
        },

        success: function (data) {
            if (data.Succeed) {
                $(data.AdditionalData).each(function () {
                    var content = "";
                    if (this.ContentType == 1) {
                        content = textPost(this);
                    }
                    if (this.ContentType == 2) {
                        content = textImagePost(this);
                    }
                    if (this.ContentType == 3) {
                        content = imagePost(this);
                    }
                    if (this.ContentType == 4) {
                        content = multiImagePost(this);
                    }
                    if (this.ContentType == 5) {
                        content = multiImageTextPost(this);
                    }
                    var moreCmtBtn = "";
                    if (this.CommentCount > 3) {
                        moreCmtBtn = "<div id='moreCmt_" + this.Id + "'><a href='javascript:void(0)' onclick='loadMoreComt(" + this.Id + ")'>Xem thêm bình luận</a>";
                    }
                    var spanLike = "";
                    if (this.Liked) {
                        spanLike = "<span><i style='color:#62a8ea' id='likeIcon_" + this.Id + "' onclick='likeUnlikePost(" + this.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + this.Id + "' style='margin-right:10px'> " + this.LikeCount + " lượt thích</span>";
                    } else {
                        spanLike = "<span><i  id='likeIcon_" + this.Id + "' onclick='likeUnlikePost(" + this.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + this.Id + "' style='margin-right:10px'> " + this.LikeCount + " lượt thích</span>";
                    }
                    var reportDiv = "";
                    if (this.AspNetUser.Id == userId) {
                        reportDiv = '<div style="position: absolute;right:5px;top:15px;width: 30px;">' +
                          '<div style="position: relative;width: 100%;text-align: center;margin-top: 5px;">' +
                          '<div class="dropdown">' +
                                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-lg fa-angle-down"></i>' +
                                       '</a>' +
                                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deletePost(' + this.Id + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editPost(' + this.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                                       '</ul>' +
                                   '</div>' +
                                   '</div>' +
                                   '</div>';
                    }
                    var post = "<li class='list-group-item panel' style='margin-bottom:10px' role='tabpanel' id='post_" + this.Id + "'>"
                                   + "<div class='media'>"
                                       + "<div class='media-left'>"
                                           + "<a class='avatar' href='javascript:void(0)'>"
                                                + "<img style='height:50px' class='img-responsive' src='" + this.AspNetUser.AvatarImage + "' alt='...'>"
                                            + "</a>"
                                        + "</div>"
                                        + "<div class='media-body'>"
                                            + "<p class='media-heading' style='margin-bottom:0'>"
                                            + '<a style="font-weight:bold" class="comment-author" href="/profile/index?userid=' + this.AspNetUser.Id + '">' + this.AspNetUser.FullName + '</a>'
                                                + "<span style='text-shadow: none'>đã đăng một bài viết</span>"
                                            + "</p>"
                                            + "<small>" + this.PostAge + "</small>"
                                            + "<div class='profile-brief' style='white-space: pre-wrap;margin-bottom:20px'>" + content + "</div>"
                                            + "<div class='comment-actions'>"
                                            + "<div style='font-size:13px' class='text-left'>"
                                                + spanLike
                                                + "<span><i class='text-like fa fa-lg fa-comments-o'></i></span><span style='margin-right:10px' id='commentOfPost_" + this.Id + "'> " + this.CommentCount + " lượt bình luận</span>"
                                            + "</div>"
                                            + "</div>"
                                             + '<div class="panel" style="margin-bottom:0px;margin-top:15px">'
                                             + '<form id="comment-form_' + this.Id + '" class="comment-form" method="post" autocomplete="off"><input type="hidden" name="postId" value="' + this.Id + '"/>'
                                            + '<input name="content" id="contentDetail_' + this.Id + '" type="text" class="form-control input-cmnt" style="padding-right: 35px" placeholder="Viết bình luận của bạn..."/>'
                                            + '<button type="button" class="btn btn-pure btn-primary fa fa-camera" style="position: absolute;top: 2px;right: 0px;transition: right 0.2s; z-index:2" onclick="addImageComment(' + this.Id + ')"></button>'
                                            + '<div style="height:0px;overflow:hidden">'
                                            + '<input type="file" id="selectImageComment_' + this.Id + '" name="image" /></div></div></form>'
                                            + '<div id="previewImageComment_' + this.Id + '" class="example margin-0" style="display:none"><div data-role="container">'
                                            + '<div data-role="content"><div class="Document"><ul id="resultComment_' + this.Id + '" class="list-inline"></ul></div></div></div></div>'
                                            + "<div class='comments'  style='margin-top:20px' id='postComments_" + this.Id + "'>"
                                                    + postComment(this)
                                            + "</div>"
                                            + moreCmtBtn
                                        + "</div>"
                                    + "</div>"
                                    + reportDiv
                                + "</li>";
                    $(post).hide().appendTo("#listPost").fadeIn("slow");
                    document.getElementById('selectImageComment_' + this.Id).addEventListener('change', function () {
                        var tmp = $(this).attr('id');
                        var result = tmp.split("_");
                        handleFileSelectComment(event, result[1]);
                    }, false);
                })
            } else {

            }
        },
        error: function (error) {
        }
    });
}

function loadNewFeedPost(userId, skip, take, actionName, loadMoreCmtActionName, deletePostActionName, loadAPostActionName, deleteCommentActionName, loadACommentActionName, likeUnlikePostActionName) {
    _loadMoreCmtActionName = loadMoreCmtActionName;
    _deletePostActionName = deletePostActionName;
    _loadAPostActionName = loadAPostActionName;
    _deleteCommentActionName = deleteCommentActionName;
    _loadACommentActionName = loadACommentActionName;
    _likeUnlikePostActionName = likeUnlikePostActionName;
    _curUserId = userId;
    $.ajax({
        url: actionName,
        type: 'POST',
        data: {
            userId: userId,
            skip: skip,
            take: take
        },
        success: function (data) {
            if (data.Succeed) {
                $(data.AdditionalData).each(function () {
                    var content = "";
                    if (this.ContentType == 1) {
                        content = textPost(this);
                    }
                    if (this.ContentType == 2) {
                        content = textImagePost(this);
                    }
                    if (this.ContentType == 3) {
                        content = imagePost(this);
                    }
                    if (this.ContentType == 4) {
                        content = multiImagePost(this);
                    }
                    if (this.ContentType == 5) {
                        content = multiImageTextPost(this);
                    }
                    var moreCmtBtn = "";
                    if (this.CommentCount > 3) {
                        moreCmtBtn = "<div id='moreCmt_"+this.Id+"'><a href='javascript:void(0)' onclick='loadMoreComt(" + this.Id + ")'>Xem thêm bình luận</a>";
                    }
                    var spanLike = "";
                    if (this.Liked) {
                        spanLike = "<span><i style='color:#62a8ea' id='likeIcon_" + this.Id + "' onclick='likeUnlikePost(" + this.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + this.Id + "' style='margin-right:10px'> " + this.LikeCount + " lượt thích</span>";
                    } else {
                        spanLike = "<span><i  id='likeIcon_" + this.Id + "' onclick='likeUnlikePost(" + this.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + this.Id + "' style='margin-right:10px'> " + this.LikeCount + " lượt thích</span>";
                    }
                    var reportDiv = "";
                    if (this.AspNetUser.Id == userId) {
                        reportDiv = '<div style="position: absolute;right:5px;top:15px;width: 30px;">' +
                          '<div style="position: relative;width: 100%;text-align: center;margin-top: 5px;">' +
                          '<div class="dropdown">' +
                                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-lg fa-angle-down"></i>' +
                                       '</a>' +
                                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deletePost(' + this.Id + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editPost(' + this.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                                       '</ul>' +
                                   '</div>' +
                                   '</div>' +
                                   '</div>';
                    }
                    var post = "<li class='list-group-item panel' style='margin-bottom:10px' role='tabpanel' id='post_" + this.Id + "'>"
                                   + "<div class='media'>"
                                       + "<div class='media-left'>"
                                           + "<a class='avatar' href='javascript:void(0)'>"
                                                + "<img style='height:50px' class='img-responsive' src='" + this.AspNetUser.AvatarImage + "' alt='...'>"
                                            + "</a>"
                                        + "</div>"
                                        + "<div class='media-body'>"
                                            + "<p class='media-heading' style='margin-bottom:0'>"
                                            + '<a style="font-weight:bold" class="comment-author" href="/profile/index?userid=' + this.AspNetUser.Id + '">' + this.AspNetUser.FullName + '</a>'
                                                + "<span style='text-shadow: none'>đã đăng một bài viết</span>"
                                            + "</p>"
                                            + "<small>" + this.PostAge + "</small>"
                                            + "<div class='profile-brief' style='white-space: pre-wrap;margin-bottom:20px'>" + content + "</div>"
                                            + "<div class='comment-actions'>"
                                            + "<div style='font-size:13px' class='text-left'>"
                                                + spanLike
                                                + "<span><i class='text-like fa fa-lg fa-comments-o'></i></span><span style='margin-right:10px' id='commentOfPost_" + this.Id + "'> " + this.CommentCount + " lượt bình luận</span>"
                                            + "</div>"
                                            + "</div>"
                                             + '<div class="panel" style="margin-bottom:0px;margin-top:15px">'
                                             + '<form id="comment-form_' + this.Id + '" class="comment-form" method="post" autocomplete="off"><input type="hidden" name="postId" value="' + this.Id + '"/>'
                                            + '<input name="content" id="contentDetail_' + this.Id + '" type="text" class="form-control input-cmnt" style="padding-right: 35px" placeholder="Viết bình luận của bạn..."/>'
                                            + '<button type="button" class="btn btn-pure btn-primary fa fa-camera" style="position: absolute;top: 2px;right: 0px;transition: right 0.2s; z-index:2" onclick="addImageComment(' + this.Id + ')"></button>'
                                            + '<div style="height:0px;overflow:hidden">'
                                            + '<input type="file" id="selectImageComment_' + this.Id + '" name="image" /></div></div></form>'
                                            + '<div id="previewImageComment_' + this.Id + '" class="example margin-0" style="display:none"><div data-role="container">'
                                            + '<div data-role="content"><div class="Document"><ul id="resultComment_' + this.Id + '" class="list-inline"></ul></div></div></div></div>'
                                            + "<div class='comments'  style='margin-top:20px' id='postComments_" + this.Id + "'>"
                                                    + postComment(this)
                                            + "</div>"
                                            + moreCmtBtn
                                        + "</div>"
                                    + "</div>"
                                    + reportDiv
                                + "</li>";
                    $(post).hide().appendTo("#listPost").fadeIn("slow");
                    document.getElementById('selectImageComment_' + this.Id).addEventListener('change', function () {
                        var tmp = $(this).attr('id');
                        var result = tmp.split("_");
                        handleFileSelectComment(event, result[1]);
                    }, false);
                })
            } else {

            }
        },
        error: function (error) {
        }
    });
}

function loadSpecificPost(userId, postId, actionName, loadMoreCmtActionName, loadMoreCmtActionName, deletePostActionName, deleteCommentActionName, loadACommentActionName, likeUnlikePostActionName, goNewFeedActionName) {
    _loadMoreCmtActionName = loadMoreCmtActionName;
    _deletePostActionName = deletePostActionName;
    _deleteCommentActionName = deleteCommentActionName;
    _loadACommentActionName = loadACommentActionName;
    _loadMoreCmtActionName = loadMoreCmtActionName;
    _likeUnlikePostActionName = likeUnlikePostActionName;
    _curUserId = userId;
    goNewFeedActionName = _goNewFeedActionName;
    $.ajax({
        url: actionName,
        type: 'POST',
        data: {
            postId: postId
        },
        success: function (data) {
            if (data.Succeed) {
                var dt = data.AdditionalData;
                    var content = "";
                    if (dt.ContentType == 1) {
                        content = textPost(dt);
                    }
                    if (dt.ContentType == 2) {
                        content = textImagePost(dt);
                    }
                    if (dt.ContentType == 3) {
                        content = imagePost(dt);
                    }
                    if (dt.ContentType == 4) {
                        content = multiImagePost(dt);
                    }
                    if (dt.ContentType == 5) {
                        content = multiImageTextPost(dt);
                    }
                    var moreCmtBtn = "";
                    if (dt.CommentCount > 3) {
                        moreCmtBtn = "<div id='moreCmt_" + dt.Id + "'><a href='javascript:void(0)' onclick='loadMoreComt(" + dt.Id + ")'>Xem thêm bình luận</a>";
                    }
                    var spanLike = "";
                    if (dt.Liked) {
                        spanLike = "<span><i style='color:#62a8ea' id='likeIcon_" + dt.Id + "' onclick='likeUnlikePost(" + dt.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + dt.Id + "' style='margin-right:10px'> " + dt.LikeCount + " lượt thích</span>";
                    } else {
                        spanLike = "<span><i  id='likeIcon_" + dt.Id + "' onclick='likeUnlikePost(" + dt.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + dt.Id + "' style='margin-right:10px'> " + dt.LikeCount + " lượt thích</span>";
                    }
                    var reportDiv = "";
                    if (dt.AspNetUser.Id == userId) {
                        reportDiv = '<div style="position: absolute;right:5px;top:15px;width: 30px;">' +
                          '<div style="position: relative;width: 100%;text-align: center;margin-top: 5px;">' +
                          '<div class="dropdown">' +
                                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-lg fa-angle-down"></i>' +
                                       '</a>' +
                                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deletePost(' + dt.Id + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editPost(' + dt.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                                       '</ul>' +
                                   '</div>' +
                                   '</div>' +
                                   '</div>';
                    }
                    var post = "<li class='list-group-item panel' style='margin-bottom:10px' role='tabpanel' id='post_" + dt.Id + "'>"
                                   + "<div class='media'>"
                                       + "<div class='media-left'>"
                                           + "<a class='avatar' href='javascript:void(0)'>"
                                                + "<img style='height:50px' class='img-responsive' src='" + dt.AspNetUser.AvatarImage + "' alt='...'>"
                                            + "</a>"
                                        + "</div>"
                                        + "<div class='media-body'>"
                                            + "<p class='media-heading' style='margin-bottom:0'>"
                                            + '<a style="font-weight:bold" class="comment-author" href="/profile/index?userid=' + dt.AspNetUser.Id + '">' + dt.AspNetUser.FullName + '</a>'
                                                + "<span style='text-shadow: none'>đã đăng một bài viết</span>"
                                            + "</p>"
                                            + "<small>" + dt.PostAge + "</small>"
                                            + "<div class='profile-brief' style='white-space: pre-wrap;margin-bottom:20px'>" + content + "</div>"
                                            + "<div class='comment-actions'>"
                                            + "<div style='font-size:13px' class='text-left'>"
                                                + spanLike
                                                + "<span><i class='text-like fa fa-lg fa-comments-o'></i></span><span style='margin-right:10px' id='commentOfPost_" + dt.Id + "'> " + dt.CommentCount + " lượt bình luận</span>"
                                            + "</div>"
                                            + "</div>"
                                             + '<div class="panel" style="margin-bottom:0px;margin-top:15px">'
                                             + '<form id="comment-form_' + dt.Id + '" class="comment-form" method="post" autocomplete="off"><input type="hidden" name="postId" value="' + dt.Id + '"/>'
                                            + '<input name="content" id="contentDetail_' + dt.Id + '" type="text" class="form-control input-cmnt" style="padding-right: 35px" placeholder="Viết bình luận của bạn..."/>'
                                            + '<button type="button" class="btn btn-pure btn-primary fa fa-camera" style="position: absolute;top: 2px;right: 0px;transition: right 0.2s; z-index:2" onclick="addImageComment(' + dt.Id + ')"></button>'
                                            + '<div style="height:0px;overflow:hidden">'
                                            + '<input type="file" id="selectImageComment_' + dt.Id + '" name="image" /></div></div></form>'
                                            + '<div id="previewImageComment_' + dt.Id + '" class="example margin-0" style="display:none"><div data-role="container">'
                                            + '<div data-role="content"><div class="Document"><ul id="resultComment_' + dt.Id + '" class="list-inline"></ul></div></div></div></div>'
                                            + "<div class='comments'  style='margin-top:20px' id='postComments_" + dt.Id + "'>"
                                                    + postComment(dt)
                                            + "</div>"
                                            + moreCmtBtn
                                        + "</div>"
                                    + "</div>"
                                    + reportDiv
                                + "</li>";
                    $(post).hide().appendTo("#listPost").fadeIn("slow");
                    document.getElementById('selectImageComment_' + dt.Id).addEventListener('change', function () {
                        var tmp = $(this).attr('id');
                        var result = tmp.split("_");
                        handleFileSelectComment(event, result[1]);
                    }, false);
            } else {

            }
        },
        error: function (error) {
        }
    });
}

function addImageComment(id) {
    $("#selectImageComment_" + id).click();
}

var storedFilesComment = [];

function removeFileComment(id) {
    for (var i = 0; i < storedFilesComment.length; i++) {
        if (storedFilesComment[i].id == id) {
            storedFilesComment.splice(i, 1);
        }
    }
    $("#selectImageComment_"+id).val("");
    $("#resultComment_" + id).empty();
    $("#previewImageComment_" + id).hide();
}

function createComment(commentActionName, loadSavedCommentActionName, userId, id) {
    var formData = new FormData(document.getElementById(id));
    formData.append('userId', userId);
    var formId = id.split("_");
    var commentContent = $("#contentDetail_" + formId[1]).val().trim();
    if (commentContent || storedFilesComment.length > 0) {
        var cmtId = "-1";
        $.ajax({
            type: 'POST',
            url: commentActionName,
            async: false,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.Succeed && data.Message == "Bình luận thành công") {
                    document.getElementById(id).reset();
                    var result = id.split("_");
                    removeFileComment(result[1]);
                    cmtId = data.Data.Id;
                } else {
                    showMessage("Không thể cập nhật.", "error", "OK");
                }
            },
            error: function (result) {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            },
        });

        $.ajax({
            type: 'POST',
            url: loadSavedCommentActionName,
            async: false,
            data: { "cmtId": cmtId },
            success: function (result) {
                if (result.Succeed) {
                    prependComment(result.AdditionalData);
                } else {
                    showErrors(result.Errors);
                    showMessage("Không thể cập nhật.", "error", "OK");

                }
            },
            error: function (result) {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            },
        });
    }
}


function handleFileSelectComment(e, id) {
    var files = e.target.files;
    if (files.length > 0) {
        $("#previewImageComment_" + id).show();
    } else {
        $("#previewImageComment_" + id).hide();
    }
    var filesArr = Array.prototype.slice.call(files);
    var output = document.getElementById("resultComment_" + id);
    filesArr.forEach(function (f) {

        if (!f.type.match("image.*")) {
            return;
        }
        var dataForm = { id: id, value: f };
        storedFilesComment.push(dataForm);

        var reader = new FileReader();
        reader.onload = function (e) {
            var li = document.createElement("li");
            li.innerHTML = "<img class='loadImage' src='" + e.target.result + "'" +
                        "title='" + f.name + "'/> <a href='javascript:void(0)' data-file='" + f.name + "' onclick='removeFileComment(" + id + ")' class='fa fa-times' style='position:absolute'></a>";
            $("#resultComment_" + id).empty();
            output.insertBefore(li, null);

        }
        reader.readAsDataURL(f);
    });

}

function prependPost(data, userId) {
    var content = "";
    if (data.ContentType == 1) {
        content = textPost(data);
    }
    if (data.ContentType == 2) {
        content = textImagePost(data);
    }
    if (data.ContentType == 3) {
        content = imagePost(data);
    }
    if (data.ContentType == 4) {
        content = multiImagePost(data);
    }
    if (data.ContentType == 5) {
        content = multiImageTextPost(data);
    }
    var moreCmtBtn = "";
    if (this.CommentCount > 3) {
        moreCmtBtn = "<div id='moreCmt_" + data.Id + "'><a href='javascript:void(0)' onclick='loadMoreComt(" + data.Id + ")'>Xem thêm bình luận</a>";
    }
    var spanLike = "";
    if (this.Liked) {
        spanLike = "<span><i style='color:#62a8ea' id='likeIcon_" + data.Id + "' onclick='likeUnlikePost(" + data.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + data.Id + "' style='margin-right:10px'> " + data.LikeCount + " lượt thích</span>";
    } else {
        spanLike = "<span><i  id='likeIcon_" + data.Id + "' onclick='likeUnlikePost(" + data.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + data.Id + "' style='margin-right:10px'> " + data.LikeCount + " lượt thích</span>";
    }
    var reportDiv = '<div style="position: absolute;right:5px;top:15px;width: 30px;">' +
          '<div style="position: relative;width: 100%;text-align: center;margin-top: 5px;">' +
          '<div class="dropdown">' +
                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-lg fa-angle-down"></i>' +
                       '</a>' +
                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deletePost(' + data.Id + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editPost(' + data.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                       '</ul>' +
                   '</div>' +
                   '</div>' +
                   '</div>';
    var post = "<li class='list-group-item panel' style='margin-bottom:10px' role='tabpanel' id='post_" + data.Id + "'>"
                                   + "<div class='media'>"
                                       + "<div class='media-left'>"
                                           + "<a class='avatar' href='javascript:void(0)'>"
                                                + "<img style='height:50px' class='img-responsive' src='" + data.AspNetUser.AvatarImage + "' alt='...'>"
                                            + "</a>"
                                        + "</div>"
                                        + "<div class='media-body'>"
                                            + "<p class='media-heading' style='margin-bottom:0'>"
                                            + '<a style="font-weight:bold" class="comment-author" href="/profile/index?userid=' + data.AspNetUser.Id + '">' + data.AspNetUser.FullName + '</a>'
                                                + "<span style='text-shadow: none'>đã đăng một bài viết</span>"
                                            + "</p>"
                                            + "<small>" + data.PostAge + "</small>"
                                            + "<div class='profile-brief' style='white-space: pre-wrap;margin-bottom:20px'>" + content + "</div>"
                                            + "<div class='comment-actions'>"
                                            + "<div style='font-size:13px' class='text-left'>"
                                                + spanLike
                                                + "<span><i class='text-like fa fa-lg fa-comments-o'></i></span><span style='margin-right:10px' id='commentOfPost_" + data.Id + "'> " + data.CommentCount + " lượt bình luận</span>"
                                            + "</div>"
                                            + "</div>"
                                             + '<div class="panel" style="margin-bottom:0px;margin-top:15px">'
                                             + '<form id="comment-form_' + data.Id + '" class="comment-form" method="post" autocomplete="off"><input type="hidden" name="postId" value="' + data.Id + '"/>'
                                            + '<input name="content" id="contentDetail_' + data.Id + '" type="text" class="form-control input-cmnt" style="padding-right: 35px" placeholder="Viết bình luận của bạn..."/>'
                                            + '<button type="button" class="btn btn-pure btn-primary fa fa-camera" style="position: absolute;top: 2px;right: 0px;transition: right 0.2s; z-index:2" onclick="addImageComment(' + data.Id + ')"></button>'
                                            + '<div style="height:0px;overflow:hidden">'
                                            + '<input type="file" id="selectImageComment_' + data.Id + '" name="image" /></div></div></form>'
                                            + '<div id="previewImageComment_' + data.Id + '" class="example margin-0" style="display:none"><div data-role="container">'
                                            + '<div data-role="content"><div class="Document"><ul id="resultComment_' + data.Id + '" class="list-inline"></ul></div></div></div></div>'
                                            + "<div class='comments'  style='margin-top:20px' id='postComments_" + data.Id + "'>"
                                                    + postComment(data)
                                            + "</div>"
                                            + moreCmtBtn
                                        + "</div>"
                                    + "</div>"
                                    + reportDiv
                                + "</li>";
    $(post).hide().prependTo("#listPost").fadeIn("slow");
    document.getElementById('selectImageComment_' + data.Id).addEventListener('change', function () {
        var tmp = $(this).attr('id');
        var result = tmp.split("_");
        handleFileSelectComment(event, result[1]);
    }, false);
}

function prependComment(data) {
    var cmt = "";
    var cmtImage = "";
    reportDiv = '<div style="position: absolute;right: 0px;top:10px;width: 30px;">' +
  '<div style="position: relative;width: 100%;text-align: center;">' +
  '<div class="dropdown" >' +
                '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-md fa-angle-down"></i>' +
               '</a>' +
               '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                   '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deleteComment(' + data.Id + ',' + data.PostId + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                   '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editComment(' + data.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
               '</ul>' +
           '</div>' +
           '</div>' +
           '</div>';

    var cmtImage = "";
    if (data.Image != null && data.Image != "") {
        cmtImage = "<div style='max-width:150px;'>"
                        + "<a class='inline-block magnific' href='" + data.Image + "' data-plugin='magnificPopup'"
                            + "data-close-btn-inside='false' data-fixed-contentPos='true' data-main-class='mfp-margin-0s mfp-with-zoom'"
                            + "data-zoom='{'enabled': 'true','duration':'300'}'>"
                            + "<img class='img-responsive' src='" + data.Image + "' alt='...' />"
                        + "</a>"
                    + "</div>";
    }
    cmt += "<div class='comment media panel' style='border:0px' id='comment_" + data.Id + "'>"
                            + "<div class='media-heading'>"
                                + reportDiv
                            + "</div>"
                        + "<div class='media-left'>"
                            + "<a class='avatar avatar-lg' href='javascript:void(0)'>"
                                + "<img style='height:50px;width:50px' src='" + data.AspNetUser.AvatarImage + "' alt='...'>"
                            + "</a>"
                        + "</div>"
                        + "<div class='comment-body media-body'>"
                            + "<a style='font-weight:bold' class='comment-author' href='/profile/index?userid="+data.AspNetUser.Id+"'>" + data.AspNetUser.FullName + "</a>"
                            + "<div class='comment-meta'>"
                                + "<span class='date'>" + data.CommentAge + "</span>"
                            + "</div>"
                            + "<div class='comment-content'>"
                                + "<p>" + data.Comment + "</p>"
                                + cmtImage
                            + "</div>"
                        + "</div>"
                    + "</div>";

    var element = "#postComments_" + data.PostId;
    $(cmt).hide().prependTo(element).fadeIn("slow");
    refreshLikeAndCommentNumber(data.PostId);
}

function appendComment(data) {
    var cmt = "";
    var cmtImage = "";
    var reportDiv = "";
    if (data.UserId == _curUserId) {
        reportDiv = '<div style="position: absolute;right: 0px;top:10px;width: 30px;">' +
      '<div style="position: relative;width: 100%;text-align: center;">' +
      '<div class="dropdown" >' +
                    '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-md fa-angle-down"></i>' +
                   '</a>' +
                   '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                       '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deleteComment(' + data.Id + ',' + data.PostId + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                       '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editComment(' + data.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                   '</ul>' +
               '</div>' +
               '</div>' +
               '</div>';
    } else {
        if (data.Post.UserId == _curUserId) {
            reportDiv = '<div style="position: absolute;right: 0px;top:10px;width: 30px;">' +
          '<div style="position: relative;width: 100%;text-align: center;">' +
          '<div class="dropdown" >' +
                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-md fa-angle-down"></i>' +
                       '</a>' +
                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deleteComment(' + data.Id + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +

                       '</ul>' +
                   '</div>' +
                   '</div>' +
                   '</div>';
        }
    }
    var cmtImage = "";
    if (data.Image != null && data.Image != "") {
        cmtImage = "<div style='max-width:150px;'>"
                        + "<a class='inline-block magnific' href='" + data.Image + "' data-plugin='magnificPopup'"
                            + "data-close-btn-inside='false' data-fixed-contentPos='true' data-main-class='mfp-margin-0s mfp-with-zoom'"
                            + "data-zoom='{'enabled': 'true','duration':'300'}'>"
                            + "<img class='img-responsive' src='" + data.Image + "' alt='...' />"
                        + "</a>"
                    + "</div>";
    }
    cmt += "<div class='comment media panel' style='border:0px' id='comment_" + data.Id + "'>"
                            + "<div class='media-heading'>"
                                + reportDiv
                            + "</div>"
                        + "<div class='media-left'>"
                            + "<a class='avatar avatar-lg' href='javascript:void(0)'>"
                                + "<img style='height:50px;width:50px' src='" + data.AspNetUser.AvatarImage + "' alt='...'>"
                            + "</a>"
                        + "</div>"
                        + "<div class='comment-body media-body'>"
                            + "<a style='font-weight:bold' class='comment-author' href='/profile/index?userid="+data.AspNetUser.Id+"'>" + data.AspNetUser.FullName + "</a>"
                            + "<div class='comment-meta'>"
                                + "<span class='date'>" + data.CommentAge + "</span>"
                            + "</div>"
                            + "<div class='comment-content'>"
                                + "<p>" + data.Comment + "</p>"
                                + cmtImage
                            + "</div>"
                        + "</div>"
                    + "</div>";
    var element = "#postComments_" + data.PostId;
    $(cmt).hide().appendTo(element).fadeIn("slow");
}

function loadMoreComt(postId) {
    var element = "#postComments_" + postId + " > div";
    var displayedCmt = $(element).length;
    var skip = displayedCmt;
    var take = 3;

    $.ajax({
        url: _loadMoreCmtActionName,
        type: 'POST',
        data: {
            postId: postId,
            skip: skip,
            take: take
        },
        success: function (data) {
            if (data.Succeed) {
                $(data.AdditionalData).each(function () {
                    appendComment(this);
                })
            } else {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            }
        }
    });

    $.ajax({
        type: 'POST',
        url: _loadAPostActionName,
        async: false,
        data: { "postId": postId },
        success: function (result) {
            if (result.Succeed) {
                if (result.AdditionalData.CommentCount < skip + take) {
                    $("#moreCmt_" + postId).remove();
                }
            }
        }
    });
}

function postComment(data) {
    var cmt = "";
    $(data.PostComments).each(function () {
        var reportDiv = "";
        if (this.UserId == _curUserId) {
            reportDiv = '<div style="position: absolute;right: 0px;top:10px;width: 30px;">' +
          '<div style="position: relative;width: 100%;text-align: center;">' +
          '<div class="dropdown" >' +
                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-md fa-angle-down"></i>' +
                       '</a>' +
                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deleteComment(' + this.Id + ',' + this.PostId + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editComment(' + this.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                       '</ul>' +
                   '</div>' +
                   '</div>' +
                   '</div>';
        } else {
            if (this.Post.UserId == _curUserId) {
                reportDiv = '<div style="position: absolute;right: 0px;top:10px;width: 30px;">' +
              '<div style="position: relative;width: 100%;text-align: center;">' +
              '<div class="dropdown" >' +
                            '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-md fa-angle-down"></i>' +
                           '</a>' +
                           '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                               '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deleteComment(' + this.Id + ',' + this.PostId + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +

                           '</ul>' +
                       '</div>' +
                       '</div>' +
                       '</div>';
            }
        }
        var cmtImage = "";
        if (this.Image != null && this.Image != "") {
            cmtImage = "<div style='max-width:150px;'>"
                            + "<a class='inline-block magnific' href='" + this.Image + "' data-plugin='magnificPopup'"
                                + "data-close-btn-inside='false' data-fixed-contentPos='true' data-main-class='mfp-margin-0s mfp-with-zoom'"
                                + "data-zoom='{'enabled': 'true','duration':'300'}'>"
                                + "<img class='img-responsive' src='" + this.Image + "' alt='...' />"
                            + "</a>"
                        + "</div>";
        }
        cmt += "<div class='comment media panel' style='border:0px' id='comment_" + this.Id + "'>"
                                + "<div class='media-heading'>"
                                    + reportDiv
                                + "</div>"
                            + "<div class='media-left'>"
                                + "<a class='avatar avatar-lg' href='javascript:void(0)'>"
                                    + "<img style='height:50px;width:50px' src='" + this.AspNetUser.AvatarImage + "' alt='...'>"
                                + "</a>"
                            + "</div>"
                            + "<div class='comment-body media-body'>"
                                + "<a style='font-weight:bold' class='comment-author' href='/profile/index?userid="+this.AspNetUser.Id+"'>" + this.AspNetUser.FullName + "</a>"
                                + "<div class='comment-meta'>"
                                    + "<span class='date'>" + this.CommentAge + "</span>"
                                + "</div>"
                                + "<div class='comment-content'>"
                                    + "<p>" + this.Comment + "</p>"
                                    + cmtImage
                                + "</div>"
                            + "</div>"
                        + "</div>";
    })
    return cmt;
}

function textPost(data) {
    var hashtag = getHashTagSport(data);
    if (hashtag != null && hashtag != "") {
        hashtag = "<br/>" + hashtag;
    }
    var content = data.PostContent;
    return content + hashtag;
}

function textImagePost(data) {
    var hashtag = getHashTagSport(data);
    if (hashtag != null && hashtag != "") {
        hashtag = "<br/>" + hashtag;
    }
    var content = data.PostContent + hashtag
                          + "<div class='media' style='margin-top:20px;'>"
                             + " <div class='col-xs-12'>"
                                  + "<div style='max-width:500px;'>"
                                       + "<a class='inline-block magnific' href='" + data.PostImages[0].Image + "' data-plugin='magnificPopup'"
                                         + "data-close-btn-inside='false' data-fixed-contentPos='true' data-main-class='mfp-margin-0s mfp-with-zoom'"
                                          + "data-zoom='{'enabled': 'true','duration':'300'}'>"
                                           + "<img class='img-responsive' src='" + data.PostImages[0].Image + "' alt='...' />"
                                       + "</a>"
                                   + "</div>"
                               + "</div>"
                           + "</div>";
    return content;
}

function imagePost(data) {
    var hashtag = getHashTagSport(data);
    if (hashtag != null && hashtag != "") {
        hashtag += "<br/>";
    }
    var content = "<div class='media'>"
                             + " <div class='col-xs-12'>"
                                  + "<div style='max-width:500px;'>"
                                       + "<a class='inline-block magnific' href='" + data.PostImages[0].Image + "' data-plugin='magnificPopup'"
                                         + "data-close-btn-inside='false' data-fixed-contentPos='true' data-main-class='mfp-margin-0s mfp-with-zoom'"
                                          + "data-zoom='{'enabled': 'true','duration':'300'}'>"
                                           + "<img class='img-responsive' src='" + data.PostImages[0].Image + "' alt='...' />"
                                       + "</a>"
                                   + "</div>"
                               + "</div>"
                           + "</div>";
    return hashtag + content;
}

function multiImagePost(data) {
    var hashtag = getHashTagSport(data);
    if (hashtag != null && hashtag != "") {
        hashtag += "<br/>";
    }
    var content = "<div class='example' id='exampleGallery'>";
    var images = "";
    $(data.PostImages).each(function () {

        images += "<a class='inline-block' style='margin-right:5px;' href='" + this.Image + "' title='' data-effect='mfp-zoom-in'>"
                     + "<img class='img-responsive' src='" + this.Image + "' alt='...' width='200'>"
                 + "</a>";
    })

    content = content + images + "</div>";
    return hashtag + content;

}

function multiImageTextPost(data) {
    var hashtag = getHashTagSport(data);
    if (hashtag != null && hashtag != "") {
        hashtag = "<br/>" + hashtag;
    }
    var content = data.PostContent + hashtag
        + "<div class='example imageMagnific' id='exampleGallery'>";
    var images = "";
    $(data.PostImages).each(function () {

        images += "<a class='inline-block' style='margin-right:5px;' href='" + this.Image + "' title='' data-effect='mfp-zoom-in'>"
                     + "<img class='img-responsive' src='" + this.Image + "' alt='...' width='200'>"
                 + "</a>";
    })

    content = content + images + "</div>";
    return content;
}

function getHashTagSport(data) {
    var hashtag = "";
    $(data.PostSports).each(function () {
        hashtag += "<a href='#" + this.Sport.Id + "'>#" + this.Sport.Name + " </a>";
    })
    return hashtag;
}

function deletePost(id) {
    showConfirmMessage("Bạn có chắc chắn muốn xóa?",
                        function (isConfirm) {
                            if (isConfirm) {
                                $.ajax({
                                    url: _deletePostActionName,
                                    async: false,
                                    type: 'POST',
                                    data: {
                                        id: id
                                    },
                                    success: function (data) {
                                        if (data.Succeed) {
                                            var loc = (location.href).toLowerCase();
                                            if (loc.indexOf("postdetail") != -1) {
                                                location.href = _goNewFeedActionName;
                                            } else {
                                                removePostInList(id);
                                                showMessage("Đã xóa", "success", "OK");
                                            }
                                        }
                                    },
                                    error: function () {

                                    }
                                });
                            }
                        }
                        )
   
}


function removePostInList(id) {
    $('#post_' + id).hide('slow', function () { $('#post_' + id).remove(); });
}

function editPost(id) {
    $.ajax({
        type: 'POST',
        url: _loadAPostActionName,
        async: false,
        data: { "postId": id },
        success: function (result) {
            if (result.Succeed) {
                prepareEditPostForm(result.AdditionalData);
            } else {
                showErrors(result.Errors);
                showMessage("Không thể cập nhật.", "error", "OK");

            }
        },
        error: function (result) {
            showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
        },
    });
}

var storedFilesExist = [];
var storedFilesEdit = [];
var ExistFilesDelete = [];

function prepareEditPostForm(data) {
    var sports = [];
    for (i = 0; i < data.PostSports.length; i++) {
        sports.push(data.PostSports[i].SportId);
    }
    $("#postEditId").val(data.Id);
    $('#PostContentEdit').val('');
    $('#PostContentEdit').val(data.PostContent);
    $('#sportSelectEdit').val("");
    $('#sportSelectEdit').select2({
        class: 'form-control',
        width: '100%',
        multiple: true,
        maximumSelectionSize: 5,
        placeholder: "Chọn môn thể thao của bạn",
        data: array
    }).select2('val', sports);
    storedFilesExist = [];
    storedFilesEdit = [];
    $("#resultEdit").empty();
    var output = document.getElementById("resultEdit");
    if (data.PostImages.length > 0) {
        for (i = 0; i < data.PostImages.length; i++) {
            storedFilesExist.push(data.PostImages[i].Id);
            var li = document.createElement("li");
            li.innerHTML = "<img class='loadImage' src='" + data.PostImages[i].Image + "'/> <a href='javascript:void(0)'  onclick='removeFileExist(this," + data.PostImages[i].Id + ")' class='fa fa-times' style='position:absolute'></a>";
            output.insertBefore(li, null);
        }
        $("#previewImageEdit").show();
    } else {
        $("#previewImageEdit").hide();
    }
    $('#editPostForm').modal('show');
}

function removeFileExist(e, id) {
    for (var i = 0; i < storedFilesExist.length; i++) {
        if (storedFilesExist[i] === id) {
            ExistFilesDelete.push(storedFilesExist[i]);
            storedFilesExist.splice(i, 1);
            break;
        }
    }
    if (storedFilesExist.length > 0 || storedFilesEdit.length > 0) {
        $("#previewImageEdit").show("slow");
    } else {
        $("#previewImageEdit").hide("slow");
    }
    $(e).parent().hide('slow', function () { $(e).parent().remove(); });
}

function removeFileEdit(e) {
    var file = $(e).data("file");
    for (var i = 0; i < storedFilesEdit.length; i++) {
        if (storedFilesEdit[i].name === file) {
            storedFilesEdit.splice(i, 1);
            break;
        }
    }
    if (storedFilesEdit.length > 0 || storedFilesExist.length > 0) {
        $("#previewImageEdit").show("slow");
    } else {
        $("#previewImageEdit").hide("slow");
    }
    $(e).parent().hide('slow', function () { $(e).parent().remove(); });
}

function addImageEdit() {
    $("#selectImageEdit").click();
}

document.getElementById('selectImageEdit').addEventListener('change', handleFileEditSelect, false);

function handleFileEditSelect(e) {
    var files = e.target.files;
    if (files.length > 0) {
        $("#previewImageEdit").show();
    } else {
        $("#previewImageEdit").hide();
    }
    var filesArr = Array.prototype.slice.call(files);
    var output = document.getElementById("resultEdit");
    filesArr.forEach(function (f) {

        if (!f.type.match("image.*")) {
            return;
        }
        storedFilesEdit.push(f);

        var reader = new FileReader();
        reader.onload = function (e) {
            var li = document.createElement("li");
            li.innerHTML = "<img class='loadImage' src='" + e.target.result + "'" +
                        "title='" + f.name + "'/> <a href='javascript:void(0)' data-file='" + f.name + "' onclick='removeFileEdit(this)' class='fa fa-times' style='position:absolute'></a>";
            output.insertBefore(li, null);

        }
        reader.readAsDataURL(f);
    });

}

function UpdatePost(updatePostActionName, loadSavedPostActionName, userId, postId) {
    var postContent = $("#PostContentEdit").val().trim();
    if (storedFilesEdit.length != 0 || storedFilesExist.length != 0 || postContent) {
        var formData = new FormData(document.getElementById('post-formEdit'));
        for (var i = 0, len = storedFilesEdit.length; i < len; i++) {
            formData.append('uploadImages', storedFilesEdit[i]);
        }
        for (var j = 0; j < ExistFilesDelete.length; j++) {
            formData.append('deleteImages', ExistFilesDelete[j]);
        }
        for (var k = 0; k < storedFilesExist.length; k++) {
            formData.append('notDeleteImages', storedFilesExist[k]);
        }
        $.ajax({
            type: 'POST',
            url: updatePostActionName,
            async: false,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (result) {
                if (result.Succeed) {
                    $('#editPostForm').modal('toggle');
                    storedFilesExist = [];
                    storedFilesEdit = [];
                    ExistFilesDelete = [];
                } else {
                    showErrors(result.Errors);
                    showMessage("Không thể cập nhật.", "error", "OK");

                }
            },
            error: function (result) {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            },
        });
        $.ajax({
            type: 'POST',
            url: loadSavedPostActionName,
            async: false,
            data: { "postId": postId },
            success: function (result) {
                if (result.Succeed) {
                    refreshPost(result.AdditionalData);
                } else {
                    showErrors(result.Errors);
                    showMessage("Không thể cập nhật.", "error", "OK");

                }
            },
            error: function (result) {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            },
        });
    }
}

function refreshPost(data) {
    var content = "";
    if (data.ContentType == 1) {
        content = textPost(data);
    }
    if (data.ContentType == 2) {
        content = textImagePost(data);
    }
    if (data.ContentType == 3) {
        content = imagePost(data);
    }
    if (data.ContentType == 4) {
        content = multiImagePost(data);
    }
    if (data.ContentType == 5) {
        content = multiImageTextPost(data);
    }
    var moreCmtBtn = "";
    if (this.CommentCount > 3) {
        moreCmtBtn = "<div id='moreCmt_" + data.Id + "'><a href='javascript:void(0)' onclick='loadMoreComt(" + data.Id + ")'>Xem thêm bình luận</a>";
    }
    var spanLike = "";
    if (data.Liked) {
        spanLike = "<span><i style='color:#62a8ea' id='likeIcon_" + data.Id + "' onclick='likeUnlikePost(" + data.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + data.Id + "' style='margin-right:10px'> " + data.LikeCount + " lượt thích</span>";
    } else {
        spanLike = "<span><i  id='likeIcon_" + data.Id + "' onclick='likeUnlikePost(" + data.Id + ")' class='text-like fa fa-lg fa-thumbs-o-up' href='javascript:void(0)' role='button'></i></span><span id='likeOfPost_" + data.Id + "' style='margin-right:10px'> " + data.LikeCount + " lượt thích</span>";
    }
    var reportDiv = "";
    if (data.AspNetUser.Id == _curUserId) {
        reportDiv = '<div style="position: absolute;right:5px;top:15px;width: 30px;">' +
          '<div style="position: relative;width: 100%;text-align: center;margin-top: 5px;">' +
          '<div class="dropdown">' +
                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-lg fa-angle-down"></i>' +
                       '</a>' +
                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deletePost(' + data.Id + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editPost(' + data.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                       '</ul>' +
                   '</div>' +
                   '</div>' +
                   '</div>';
    }
    var post = "<li class='list-group-item panel' style='margin-bottom:10px' role='tabpanel' id='post_" + data.Id + "'>"
                   + "<div class='media'>"
                       + "<div class='media-left'>"
                           + "<a class='avatar' href='javascript:void(0)'>"
                                + "<img style='height:50px' class='img-responsive' src='" + data.AspNetUser.AvatarImage + "' alt='...'>"
                            + "</a>"
                        + "</div>"
                        + "<div class='media-body'>"
                           + "<p class='media-heading' style='margin-bottom:0'>"
                            + '<a style="font-weight:bold" class="comment-author" href="/profile/index?userid=' + data.AspNetUser.Id + '">' + data.AspNetUser.FullName + '</a>'
                                + "<span style='text-shadow: none'>đã đăng một bài viết</span>"
                            + "</p>"
                            + "<small>" + data.PostAge + "</small>"
                            + "<div class='profile-brief' style='white-space: pre-wrap;margin-bottom:20px'>" + content + "</div>"
                            + "<div class='comment-actions'>"
                             + "<div style='font-size:13px' class='text-left'>"
                                                + spanLike
                                                + "<span><i class='text-like fa fa-lg fa-comments-o'></i></span><span style='margin-right:10px' id='commentOfPost_" + data.Id + "'> " + data.CommentCount + " lượt bình luận</span>"
                                            + "</div>"
                            + "</div>"
                             + '<div class="panel" style="margin-bottom:0px;margin-top:15px">'
                             + '<form id="comment-form_' + data.Id + '" class="comment-form" method="post" autocomplete="off"><input type="hidden" name="postId" value="' + data.Id + '"/>'
                            + '<input name="content" id="contentDetail_' + data.Id + '" type="text" class="form-control input-cmnt" style="padding-right: 35px" placeholder="Viết bình luận của bạn..."/>'
                            + '<button type="button" class="btn btn-pure btn-primary fa fa-camera" style="position: absolute;top: 2px;right: 0px;transition: right 0.2s; z-index:2" onclick="addImageComment(' + data.Id + ')"></button>'
                            + '<div style="height:0px;overflow:hidden">'
                            + '<input type="file" id="selectImageComment_' + data.Id + '" name="image" /></div></div></form>'
                            + '<div id="previewImageComment_' + data.Id + '" class="example margin-0" style="display:none"><div data-role="container">'
                            + '<div data-role="content"><div class="Document"><ul id="resultComment_' + data.Id + '" class="list-inline"></ul></div></div></div></div>'
                            + "<div class='comments'  style='margin-top:20px' id='postComments_" + data.Id + "'>"
                                    + postComment(data)
                            + "</div>"
                            + moreCmtBtn
                        + "</div>"
                    + "</div>"
                    + reportDiv
                + "</li>";
    $("#post_" + data.Id).replaceWith(post);
    document.getElementById('selectImageComment_' + data.Id).addEventListener('change', function () {
        var tmp = $(this).attr('id');
        var result = tmp.split("_");
        handleFileSelectComment(event, result[1]);
    }, false);
}

function deleteComment(id, postId) {
    showConfirmMessage("Bạn có chắc chắn muốn xóa?",
                        function (isConfirm) {
                            if (isConfirm) {
                                $.ajax({
                                    url: _deleteCommentActionName,
                                    async: false,
                                    type: 'POST',
                                    data: {
                                        id: id
                                    },
                                    success: function (data) {
                                        if (data.Succeed) {
                                            removeCommentInList(id);
                                            refreshLikeAndCommentNumber(postId);
                                            showMessage("Đã xóa", "success", "OK");
                                        }
                                    },
                                    error: function () {

                                    }
                                });
                            }
                        })

}

function removeCommentInList(id) {
    $('#comment_' + id).hide('slow', function () { $('#comment_' + id).remove(); });
}

function editComment(id) {
    $.ajax({
        type: 'POST',
        url: _loadACommentActionName,
        async: false,
        data: { "cmtId": id },
        success: function (result) {
            if (result.Succeed) {
                prepareEditCommentForm(result.AdditionalData);
            } else {
                showErrors(result.Errors);
                showMessage("Không thể cập nhật.", "error", "OK");

            }
        },
        error: function (result) {
            showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
        },
    });
}

var tmpForHtml = "";
var deleteOldImg = false;
var hasOldImg = false;
var storedFilesCommentEdit = [];

function prepareEditCommentForm(data) {

    var html = $("#comment_" + data.Id).html();
    tmpForHtml = '<div class="comment media panel" id="comment_' + data.Id + '">' + html + '</div>';
    var editForm =
        '<div class="comment media panel" id="commentEdit_' + data.Id + '">'
        + "<div class='media-heading'>"
        + '<div style="position: absolute;right: 0px;top:0px;width: 30px;">'
        + '<div style="position: relative;width: 100%;text-align: center;">'
        + '<a style="font-size:10px" href="javascript:void(0)" onclick="cancelEditComment(' + data.Id + ')">Hủy</a>'
        + "</div>"
        + "</div>"
        + "</div>"
        + "<div class='media-left'>"
        + "<a class='avatar avatar-lg' href='javascript:void(0)'>"
        + "<img src='" + data.AspNetUser.AvatarImage + "' alt='...'>"
        + "</a>"
        + "</div>"
        + "<div class='comment-body media-body'>"
        + '<div class="panel" style="margin-bottom:0px;margin-top:15px">'
        + '<form id="comment-formEdit_' + data.Id + '" class="comment-formEdit" method="post" autocomplete="off"><input type="hidden" name="postId" value="' + data.PostId + '"/>'
        + '<input name="content" id="contentDetailEdit_' + data.Id + '" type="text" class="form-control input-cmnt" style="padding-right: 35px" value="' + data.Comment + '"/>'
        + '<button type="button" class="btn btn-pure btn-primary fa fa-camera" style="position: absolute;top: 2px;right: 0px;transition: right 0.2s; z-index:2" onclick="addImageCommentEdit(' + data.Id + ')"></button>'
        + '<div style="height:0px;overflow:hidden">'
        + '<input type="file" id="selectImageCommentEdit_' + data.Id + '" name="image" /></div></div></form>'
        + '<div id="previewImageCommentEdit_' + data.Id + '" class="example margin-0" style="display:none"><div data-role="container">'
        + '<div data-role="content"><div class="Document"><ul id="resultCommentEdit_' + data.Id + '" class="list-inline"></ul></div></div></div></div>'
        + '</div>'
    + '</div>'

    $("#comment_" + data.Id).replaceWith(editForm);
    if (data.Image != null) {
        var output = document.getElementById("resultCommentEdit_" + data.Id);
        var li = document.createElement("li");
        li.innerHTML = "<img class='loadImage' src='" + data.Image + "'" +
                    "/> <a href='javascript:void(0)' onclick='removeFileCommentEdit(" + data.Id + ")' class='fa fa-times' style='position:absolute'></a>";
        output.insertBefore(li, null);
        $("#previewImageCommentEdit_" + data.Id).show();
        hasOldImg = true;
    }

    document.getElementById('selectImageCommentEdit_' + data.Id).addEventListener('change', function () {
        var tmp = $(this).attr('id');
        var result = tmp.split("_");
        handleFileSelectCommentEdit(event, result[1]);
    }, false);

}

function cancelEditComment(id) {
    $("#commentEdit_" + id).replaceWith(tmpForHtml);
    tmpForHtml = "";
}

function removeFileCommentEdit(id) {
    deleteOldImg = true;
    $("#resultCommentEdit_" + id).empty();
    $("#selectImageCommentEdit_" + id).val("");
    $("#previewImageCommentEdit_" + id).hide();
}

function addImageCommentEdit(id) {
    $("#selectImageCommentEdit_" + id).click();
}


function handleFileSelectCommentEdit(e, id) {
    var files = e.target.files;
    if (files.length > 0) {
        $("#previewImageCommentEdit_" + id).show();
    } else {
        $("#previewImageCommentEdit_" + id).hide();
    }
    var filesArr = Array.prototype.slice.call(files);
    var output = document.getElementById("resultCommentEdit_" + id);
    filesArr.forEach(function (f) {

        if (!f.type.match("image.*")) {
            return;
        }
        var dataForm = { id: id, value: f };
        storedFilesCommentEdit.push(dataForm);

        var reader = new FileReader();
        reader.onload = function (e) {
            var li = document.createElement("li");
            li.innerHTML = "<img class='loadImage' src='" + e.target.result + "'" +
                        "title='" + f.name + "'/> <a href='javascript:void(0)' data-file='" + f.name + "' onclick='removeFileCommentEdit(" + id + ")' class='fa fa-times' style='position:absolute'></a>";
            $("#resultCommentEdit_" + id).empty();
            output.insertBefore(li, null);

        }
        reader.readAsDataURL(f);
        deleteOldImg = true;
    });

}

function updateComment(updateCommentActionName, loadSavedCommentActionName, id) {
    var formData = new FormData(document.getElementById(id));
    var formId = id.split("_");
    formData.append("deleteOldImg", deleteOldImg);
    formData.append("commentId", formId[1]);
    var commentContent = $("#contentDetailEdit_" + formId[1]).val().trim();
    if (commentContent || storedFilesCommentEdit.length > 0 || (!deleteOldImg && hasOldImg)) {
        var cmtId = "-1";
        $.ajax({
            type: 'POST',
            url: updateCommentActionName,
            async: false,
            data: formData,
            cache: false,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.Succeed) {
                    hasOldImg = false;
                    removeFileCommentEdit(formId[1]);
                    deleteOldImg = false;
                } else {
                    showMessage("Không thể cập nhật.", "error", "OK");
                }
            },
            error: function (result) {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            },
        });

        $.ajax({
            type: 'POST',
            url: loadSavedCommentActionName,
            async: false,
            data: { "cmtId": formId[1] },
            success: function (result) {
                if (result.Succeed) {
                    refreshComment(result.AdditionalData);
                } else {
                    showErrors(result.Errors);
                    showMessage("Không thể cập nhật.", "error", "OK");

                }
            },
            error: function (result) {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            },
        });
    }
}

function refreshComment(data) {
    var cmt = "";
    if (data.UserId == _curUserId) {
        reportDiv = '<div style="position: absolute;right: 0px;top:10px;width: 30px;">' +
      '<div style="position: relative;width: 100%;text-align: center;">' +
      '<div class="dropdown" >' +
                    '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-md fa-angle-down"></i>' +
                   '</a>' +
                   '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                       '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deleteComment(' + data.Id + ',' + data.PostId + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +
                       '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="editComment(' + data.Id + ')"><i style="color: #004D40;" class="fa fa-fw fa-edit"></i> Chỉnh sửa</a></li>' +
                   '</ul>' +
               '</div>' +
               '</div>' +
               '</div>';
    } else {
        if (data.Post.UserId == _curUserId) {
            reportDiv = '<div style="position: absolute;right: 0px;top:10px;width: 30px;">' +
          '<div style="position: relative;width: 100%;text-align: center;">' +
          '<div class="dropdown" >' +
                        '<a href="javascript:void(0)" class="dropdown-toggle" data-toggle="dropdown" style="color: black;"> <i class="fa fa-md fa-angle-down"></i>' +
                       '</a>' +
                       '<ul class="dropdown-menu" role="menu" style="min-width: 0px;min-height: 0px;padding: 0;">' +
                           '<li><a href="javascript:void(0)" style="font-weight: bold;" onclick="deleteComment(' + data.Id + ',' + data.PostId + ')"><i style="color: #BF360C;" class="fa fa-fw fa-times"></i> Xóa</a></li>' +

                       '</ul>' +
                   '</div>' +
                   '</div>' +
                   '</div>';
        }
    }
    var cmtImage = "";
    if (data.Image != null && data.Image != "") {
        cmtImage = "<div style='max-width:150px;'>"
                        + "<a class='inline-block magnific' href='" + data.Image + "' data-plugin='magnificPopup'"
                            + "data-close-btn-inside='false' data-fixed-contentPos='true' data-main-class='mfp-margin-0s mfp-with-zoom'"
                            + "data-zoom='{'enabled': 'true','duration':'300'}'>"
                            + "<img class='img-responsive' src='" + data.Image + "' alt='...' />"
                        + "</a>"
                    + "</div>";
    }
    cmt += "<div class='comment media panel' style='border:0px' id='comment_" + data.Id + "'>"
                            + "<div class='media-heading'>"
                                + reportDiv
                            + "</div>"
                        + "<div class='media-left'>"
                            + "<a class='avatar avatar-lg' href='javascript:void(0)'>"
                                + "<img style='height:50px;width:50px' src='" + data.AspNetUser.AvatarImage + "' alt='...'>"
                            + "</a>"
                        + "</div>"
                        + "<div class='comment-body media-body'>"
                            + "<a style='font-weight:bold' class='comment-author' href=/profile/index?userid="+data.AspNetUser.Id+"'>" + data.AspNetUser.FullName + "</a>"
                            + "<div class='comment-meta'>"
                                + "<span class='date'>" + data.CommentAge + "</span>"
                            + "</div>"
                            + "<div class='comment-content'>"
                                + "<p>" + data.Comment + "</p>"
                                + cmtImage
                            + "</div>"
                        + "</div>"
                    + "</div>";
    $("#commentEdit_" + data.Id).replaceWith(cmt);
}

function likeUnlikePost(id) {
    $.ajax({
        type: 'POST',
        url: _likeUnlikePostActionName,
        data: { "postId": id, "userId": _curUserId },
        dataType: 'json',
        success: function (data) {
            if (data.Succeed) {
                if (data.Message == "Đã thích") {
                    $("#likeIcon_" + id).css("color", "#62a8ea");
                } else if (data.Message == "Đã bỏ thích") {
                    $("#likeIcon_" + id).removeAttr("style");
                }
                refreshLikeAndCommentNumber(id);
            } else {
                showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
            }
        },
        error: function (data) {
            showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
        },
    });
}

function refreshLikeAndCommentNumber(id) {
    $.ajax({
        type: 'POST',
        url: _loadAPostActionName,
        async: false,
        data: { "postId": id },
        success: function (result) {
            if (result.Succeed) {
                $("#likeOfPost_" + id).empty();
                $("#likeOfPost_" + id).append(" " + result.AdditionalData.LikeCount + " lượt thích");
                $("#commentOfPost_" + id).empty();
                $("#commentOfPost_" + id).append(" " + result.AdditionalData.CommentCount + " lượt bình luận");
            } else {
                showErrors(result.Errors);
                showMessage("Không thể cập nhật.", "error", "OK");

            }
        },
        error: function (result) {
            showMessage("Có lỗi xảy ra. Vui lòng thử lại sau.", "error", "OK");
        },
    });
}


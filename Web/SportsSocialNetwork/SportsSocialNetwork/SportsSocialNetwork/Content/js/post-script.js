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

function loadGroupPost(groupId, curUserId, skip, take, actionName) {
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
                    if (postComment(this) != null && postComment(this) != "") {
                        moreCmtBtn = "<div><a href='#'>Xem thêm bình luận</a>";
                    }
                    var post = "<li class='list-group-item'>"
                                   + "<div class='media'>"
                                       + "<div class='media-left'>"
                                           + "<a class='avatar' href='javascript:void(0)'>"
                                                + "<img class='img-responsive' src='" + this.AspNetUser.AvatarImage + "' alt='...'>"
                                            + "</a>"
                                        + "</div>"
                                        + "<div class='media-body'>"
                                            + "<h4 class='media-heading'>"
                                                + this.AspNetUser.FullName
                                                + "<span>đã đăng một bài viết</span>"
                                            + "</h4>"
                                            + "<small>" + this.PostAge + "</small>"
                                            + "<div class='profile-brief'>" + content + "</div>"
                                            + "<div class='comment-actions'>"
                                                + "<a class='text-like fa fa-thumbs-o-up' href='javascript:void(0)' role='button'> " + this.LikeCount + "</a>"
                                                + "<a class='text-like fa fa-comments-o' href='javascript:void(0)' role='button'> " + this.CommentCount + "</a>"
                                            + "</div>"
                                            + "<div class='comments' id='postComments_"+ this.Id +"'>"
                                                    + postComment(this)
                                            + "</div>"
                                            + moreCmtBtn
                                        + "</div>"
                                    + "</div>"
                                + "</li>";
                    $("#listPost").append(post);

                })
            } else {

            }
        },
        error: function (error) {
        }
    });
}

function loadProfilePost(userId, curUserId, skip, take, actionName) {
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
                    if (postComment(this) != null && postComment(this) != "") {
                        moreCmtBtn = "<div><a href='#'>Xem thêm bình luận</a>";
                    }
                    var post = "<li class='list-group-item'>"
                                   + "<div class='media'>"
                                       + "<div class='media-left'>"
                                           + "<a class='avatar' href='javascript:void(0)'>"
                                                + "<img class='img-responsive' src='" + this.AspNetUser.AvatarImage + "' alt='...'>"
                                            + "</a>"
                                        + "</div>"
                                        + "<div class='media-body'>"
                                            + "<h4 class='media-heading'>"
                                                + this.AspNetUser.FullName
                                                + "<span>đã đăng một bài viết</span>"
                                            + "</h4>"
                                            + "<small>" + this.PostAge + "</small>"
                                            + "<div class='profile-brief'>" + content + "</div>"
                                            + "<div class='comment-actions'>"
                                                + "<a class='text-like fa fa-thumbs-o-up' href='javascript:void(0)' role='button'> " + this.LikeCount + "</a>"
                                                + "<a class='text-like fa fa-comments-o' href='javascript:void(0)' role='button'> " + this.CommentCount + "</a>"
                                            + "</div>"
                                            + "<div class='comments' id='postComments_" + this.Id + "'>"
                                                    + postComment(this)
                                            + "</div>"
                                            + moreCmtBtn
                                        + "</div>"
                                    + "</div>"
                                + "</li>";
                    $("#listPost").append(post);

                })
            } else {

            }
        },
        error: function (error) {
        }
    });
}

function prependPost(data) {
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
    if (postComment(data) != null && postComment(data) != "") {
        moreCmtBtn = "<div><a href='#'>Xem thêm bình luận</a>";
    }
    var post = "<li class='list-group-item'>"
                   + "<div class='media'>"
                       + "<div class='media-left'>"
                           + "<a class='avatar' href='javascript:void(0)'>"
                                + "<img class='img-responsive' src='" + data.AspNetUser.AvatarImage + "' alt='...'>"
                            + "</a>"
                        + "</div>"
                        + "<div class='media-body'>"
                            + "<h4 class='media-heading'>"
                                + data.AspNetUser.FullName
                                + "<span>đã đăng một bài viết</span>"
                            + "</h4>"
                            + "<small>" + data.PostAge + "</small>"
                            + "<div class='profile-brief'>" + content + "</div>"
                            + "<div class='comment-actions'>"
                                + "<a class='text-like fa fa-thumbs-o-up' href='javascript:void(0)' role='button'> " + data.LikeCount + "</a>"
                                + "<a class='text-like fa fa-comments-o' href='javascript:void(0)' role='button'> " + data.CommentCount + "</a>"
                            + "</div>"
                            + "<div class='comments' id='postComments_" + this.Id + "'>"
                                    + postComment(data)
                            + "</div>"
                            + moreCmtBtn
                        + "</div>"
                    + "</div>"
                + "</li>";
    $("#listPost").prepend(post);
}

function appendComment(data) {
    var cmt = "";
    var cmtImage = "";
    if (data.Image != null) {
        cmtImage = "<div style='max-width:150px;'>"
                        + "<a class='inline-block magnific' href='" + data.Image + "' data-plugin='magnificPopup'"
                            + "data-close-btn-inside='false' data-fixed-contentPos='true' data-main-class='mfp-margin-0s mfp-with-zoom'"
                            + "data-zoom='{'enabled': 'true','duration':'300'}'>"
                            + "<img class='img-responsive' src='" + data.Image + "' alt='...' />"
                        + "</a>"
                    + "</div>";
    }
    cmt += "<div class='comment media'>"
                        + "<div class='media-left'>"
                            + "<a class='avatar avatar-lg' href='javascript:void(0)'>"
                                + "<img src='" + data.AspNetUser.AvatarImage + "' alt='...'>"
                            + "</a>"
                        + "</div>"
                        + "<div class='comment-body media-body'>"
                            + "<a class='comment-author' href='javascript:void(0)'>" + data.AspNetUser.FullName + "</a>"
                            + "<div class='comment-meta'>"
                                + "<span class='date'>" + data.CommentAge + "</span>"
                            + "</div>"
                            + "<div class='comment-content'>"
                                + "<p>" + data.Comment + "</p>"
                                + cmtImage
                            + "</div>"
                            //+ "<div class='comment-actions'>"
                            //    + "<a class='text-like icon wb-heart' href='javascript:void(0)' role='button'></a>"
                            //    + "<a href='javascript:void(0)' role='button'>Reply</a>"
                            //+ "</div>"
                        + "</div>"
                    + "</div>";
    var element = "#postComments_" + data.PostId;
    $(element).append(cmt);
}

function loadMoreComt(postId, actionName) {
    $.ajax({
        url: actionName,
        type: 'POST',
        data: {
            postId: postId
        },
        success: function (data) {
            if (data.Succeed) {
                $(data.AdditionalData).each(function () {
                    appendComment(this);
                })
            }
        },
        error: function () {

        }
    })
}

function postComment(data) {
    var cmt = "";
    $(data.PostComments).each(function () {
        var cmtImage = "";
        if (this.Image != null) {
            cmtImage = "<div style='max-width:150px;'>"
                            + "<a class='inline-block magnific' href='" + this.Image + "' data-plugin='magnificPopup'"
                                + "data-close-btn-inside='false' data-fixed-contentPos='true' data-main-class='mfp-margin-0s mfp-with-zoom'"
                                + "data-zoom='{'enabled': 'true','duration':'300'}'>"
                                + "<img class='img-responsive' src='" + this.Image + "' alt='...' />"
                            + "</a>"
                        + "</div>";
        }
        cmt += "<div class='comment media'>"
                            + "<div class='media-left'>"
                                + "<a class='avatar avatar-lg' href='javascript:void(0)'>"
                                    + "<img src='" + this.AspNetUser.AvatarImage + "' alt='...'>"
                                + "</a>"
                            + "</div>"
                            + "<div class='comment-body media-body'>"
                                + "<a class='comment-author' href='javascript:void(0)'>" + this.AspNetUser.FullName + "</a>"
                                + "<div class='comment-meta'>"
                                    + "<span class='date'>" + this.CommentAge + "</span>"
                                + "</div>"
                                + "<div class='comment-content'>"
                                    + "<p>" + this.Comment + "</p>"
                                    + cmtImage
                                + "</div>"
                                //+ "<div class='comment-actions'>"
                                //    + "<a class='text-like icon wb-heart' href='javascript:void(0)' role='button'></a>"
                                //    + "<a href='javascript:void(0)' role='button'>Reply</a>"
                                //+ "</div>"
                            + "</div>"
                        + "</div>";
    })
    return cmt;
}

function textPost(data) {
    var hashtag = getHashTagSport(data);
    if (hashtag != null && hashtag != "") {
        hashtag += "<br/>";
    }
    var content = data.PostContent;
    return hashtag + content;
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
    $(data.PostSports).each(function() {
        hashtag += "<a href='" + this.Sport.Id + "'>#" + this.Sport.Name + " </a>";
    })
    return hashtag;
}
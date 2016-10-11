package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 9/6/16.
 */
public class Feed extends Post {
    @SerializedName("CreateDateString")
    private String createDateString;
    @SerializedName("EditDateString")
    private String editDateString;
    @SerializedName("UserName")
    private String userName;
    @SerializedName("LikeCount")
    private int likeCount;
    @SerializedName("CommentCount")
    private int commentCount;
    @SerializedName("Liked")
    private boolean liked;

    public String getCreateDateString() {
        return createDateString;
    }

    public void setCreateDateString(String createDateString) {
        this.createDateString = createDateString;
    }

    public String getEditDateString() {
        return editDateString;
    }

    public void setEditDateString(String editDateString) {
        this.editDateString = editDateString;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public int getLikeCount() {
        return likeCount;
    }

    public void setLikeCount(int likeCount) {
        this.likeCount = likeCount;
    }

    public int getCommentCount() {
        return commentCount;
    }

    public void setCommentCount(int commentCount) {
        this.commentCount = commentCount;
    }

    public boolean isLiked() {
        return liked;
    }

    public void setLiked(boolean liked) {
        this.liked = liked;
    }
}

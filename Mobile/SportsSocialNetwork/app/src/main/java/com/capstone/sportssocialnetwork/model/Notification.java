package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 6/18/2016.
 */
public class Notification {
    @SerializedName("Id")
    private int id;
    @SerializedName("UserId")
    private String userId;
    @SerializedName("Title")
    private String title;
    @SerializedName("Message")
    private String message;
    @SerializedName("CreateDate")
    private String createDate;
    @SerializedName("CreateDateString")
    private String createDateString;
    @SerializedName("MarkRead")
    private boolean Read;
    @SerializedName("PostId")
    private Integer postId;
    @SerializedName("InvitationId")
    private Integer invitationId;
    @SerializedName("OrderId")
    private Integer orderId;


    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public boolean isRead() {
        return Read;
    }

    public void setRead(boolean read) {
        Read = read;
    }

    public String getCreateDateString() {
        return createDateString;
    }

    public void setCreateDateString(String createDateString) {
        this.createDateString = createDateString;
    }

    public Integer getPostId() {
        return postId;
    }

    public void setPostId(Integer postId) {
        this.postId = postId;
    }

    public Integer getInvitationId() {
        return invitationId;
    }

    public void setInvitationId(Integer invitationId) {
        this.invitationId = invitationId;
    }

    public Integer getOrderId() {
        return orderId;
    }

    public void setOrderId(Integer orderId) {
        this.orderId = orderId;
    }
}

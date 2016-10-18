package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 10/17/16.
 */
public class Order {
    @SerializedName("Id")
    private String id;
    @SerializedName("CreateDate")
    private String createDate;
    @SerializedName("StartTime")
    private String startTime;
    @SerializedName("EndTime")
    private String endTime;
    @SerializedName("UserName")
    private String username;
    @SerializedName("FullName")
    private String fullName;
    @SerializedName("FieldName")
    private String fieldName;
    @SerializedName("PlaceName")
    private String placeName;
    @SerializedName("Status")
    private String status;
    @SerializedName("PaidType")
    private String paidType;
    @SerializedName("QRCodeUrl")
    private String qRCodeUrl;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public String getStartTime() {
        return startTime;
    }

    public void setStartTime(String startTime) {
        this.startTime = startTime;
    }

    public String getEndTime() {
        return endTime;
    }

    public void setEndTime(String endTime) {
        this.endTime = endTime;
    }

    public String getUsername() {
        return username;
    }

    public void setUsername(String username) {
        this.username = username;
    }

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }

    public String getFieldName() {
        return fieldName;
    }

    public void setFieldName(String fieldName) {
        this.fieldName = fieldName;
    }

    public String getPlaceName() {
        return placeName;
    }

    public void setPlaceName(String placeName) {
        this.placeName = placeName;
    }

    public String getStatus() {
        return status;
    }

    public void setStatus(String status) {
        this.status = status;
    }

    public String getPaidType() {
        return paidType;
    }

    public void setPaidType(String paidType) {
        this.paidType = paidType;
    }


    public String getqRCodeUrl() {
        return qRCodeUrl;
    }

    public void setqRCodeUrl(String qRCodeUrl) {
        this.qRCodeUrl = qRCodeUrl;
    }
}

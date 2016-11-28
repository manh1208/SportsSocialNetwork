package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 10/17/16.
 */
public class Order {
    @SerializedName("Id")
    private int id;
    @SerializedName("CreateDateString")
    private String createDate;
    @SerializedName("StartTimeString")
    private String startTime;
    @SerializedName("EndTimeString")
    private String endTime;
    @SerializedName("UserName")
    private String username;
    @SerializedName("FullName")
    private String fullName;
    @SerializedName("FieldId")
    private int fieldId;
    @SerializedName("FieldName")
    private String fieldName;
    @SerializedName("PlaceId")
    private int placeId;
    @SerializedName("PlaceName")
    private String placeName;
    @SerializedName("Status")
    private int status;
    @SerializedName("StatusString")
    private String statusString;
    @SerializedName("PaidType")
    private int paidType;
    @SerializedName("PaidTypeString")
    private String paidTypeString;
    @SerializedName("QRCodeUrl")
    private String qRCodeUrl;
    @SerializedName("Price")
    private double price;

    public int getId() {
        return id;
    }

    public void setId(int id) {
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


    public String getqRCodeUrl() {
        return qRCodeUrl;
    }

    public void setqRCodeUrl(String qRCodeUrl) {
        this.qRCodeUrl = qRCodeUrl;
    }

    public int getFieldId() {
        return fieldId;
    }

    public void setFieldId(int fieldId) {
        this.fieldId = fieldId;
    }

    public int getPlaceId() {
        return placeId;
    }

    public void setPlaceId(int placeId) {
        this.placeId = placeId;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public String getStatusString() {
        return statusString;
    }

    public void setStatusString(String statusString) {
        this.statusString = statusString;
    }

    public int getPaidType() {
        return paidType;
    }

    public void setPaidType(int paidType) {
        this.paidType = paidType;
    }

    public String getPaidTypeString() {
        return paidTypeString;
    }

    public void setPaidTypeString(String paidTypeString) {
        this.paidTypeString = paidTypeString;
    }

    public Double getPrice() {
        return price;
    }

    public void setPrice(double price) {
        this.price = price;
    }
}

package com.capstone.sportssocialnetwork.model.request;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 10/21/16.
 */
public class OrderRequestModel {

    @SerializedName("userId")
    private String userId;
    @SerializedName("fieldId")
    private int fieldId;
    @SerializedName("startTime")
    private String startTime;
    @SerializedName("endTime")
    private String endTime;
    @SerializedName("note")
    private String note;
    @SerializedName("price")
    private String price;
    @SerializedName("paidType")
    private int paidType;
    @SerializedName("PlayDate")
    private String playDate;

    public OrderRequestModel(String userId, int fieldId, String startTime, String endTime, String note, String price, int paidType, String playDate) {
        this.userId = userId;
        this.fieldId = fieldId;
        this.startTime = startTime;
        this.endTime = endTime;
        this.note = note;
        this.price = price;
        this.paidType = paidType;
        this.playDate = playDate;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public int getFieldId() {
        return fieldId;
    }

    public void setFieldId(int fieldId) {
        this.fieldId = fieldId;
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

    public String getNote() {
        return note;
    }

    public void setNote(String note) {
        this.note = note;
    }

    public String getPrice() {
        return price;
    }

    public void setPrice(String price) {
        this.price = price;
    }

    public int getPaidType() {
        return paidType;
    }

    public void setPaidType(int paidType) {
        this.paidType = paidType;
    }

    public String getPlayDate() {
        return playDate;
    }

    public void setPlayDate(String playDate) {
        this.playDate = playDate;
    }
}

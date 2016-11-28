package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 10/16/16.
 */
public class CheckIn {

    @SerializedName("Id")
    private int    id;
    @SerializedName("CreateDateString")
    private String createDate;
    @SerializedName("StartTimeString")
    private String startTime;
    @SerializedName("EndTimeString")
    private String endTime;
    @SerializedName("FullName")
    private String fullName;
    @SerializedName("FieldName")
    private String fieldName;
    @SerializedName("PlaceName")
    private String placeName;
    @SerializedName("Status")
    private int status;
    @SerializedName("PaidType")
    private int paidType;

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

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public int getPaidType() {
        return paidType;
    }

    public void setPaidType(int paidType) {
        this.paidType = paidType;
    }
}

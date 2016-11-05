package com.capstone.sportssocialnetwork.model.request;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 11/5/16.
 */

public class CreateFieldScheduleRequestModel {
    @SerializedName("FieldId")
    private int fieldId;
    @SerializedName("Type")
    private int type;
    @SerializedName("StartTime")
    private String startTime;
    @SerializedName("EndTime")
    private String endTime;
    @SerializedName("Description")
    private String description;

    public CreateFieldScheduleRequestModel() {
    }

    public CreateFieldScheduleRequestModel(int fieldId, int type, String startTime, String endTime, String description) {
        this.fieldId = fieldId;
        this.type = type;
        this.startTime = startTime;
        this.endTime = endTime;
        this.description = description;
    }

    public int getFieldId() {
        return fieldId;
    }

    public void setFieldId(int fieldId) {
        this.fieldId = fieldId;
    }

    public int getType() {
        return type;
    }

    public void setType(int type) {
        this.type = type;
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

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }
}

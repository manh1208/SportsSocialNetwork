package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 11/5/16.
 */

public class FieldSchedule {

    @SerializedName("TypeString")
    private String typeString;
    @SerializedName("StartDateStr")
    private String startDateString;
    @SerializedName("EndDateStr")
    private String endDateString;
    @SerializedName("StartTimeStr")
    private String startTimeString;
    @SerializedName("EndTimeStr")
    private String endTimeString;
    @SerializedName("Days")
    private String RepeatDay;
    @SerializedName("Id")
    private int id;
    @SerializedName("FieldId")
    private int fieldId;
    @SerializedName("FieldName")
    private String fieldName;
    @SerializedName("Type")
    private int Type;
    @SerializedName("Description")
    private String Description;
    @SerializedName("Active")
    private boolean active;


    public String getTypeString() {
        return typeString;
    }

    public void setTypeString(String typeString) {
        this.typeString = typeString;
    }

    public String getStartTimeString() {
        return startTimeString;
    }

    public void setStartTimeString(String startTimeString) {
        this.startTimeString = startTimeString;
    }

    public String getEndTimeString() {
        return endTimeString;
    }

    public void setEndTimeString(String endTimeString) {
        this.endTimeString = endTimeString;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getFieldId() {
        return fieldId;
    }

    public void setFieldId(int fieldId) {
        this.fieldId = fieldId;
    }

    public int getType() {
        return Type;
    }

    public void setType(int type) {
        Type = type;
    }

    public String getDescription() {
        return Description;
    }

    public void setDescription(String description) {
        Description = description;
    }

    public boolean isActive() {
        return active;
    }

    public void setActive(boolean active) {
        this.active = active;
    }

    public String getFieldName() {
        return fieldName;
    }

    public void setFieldName(String fieldName) {
        this.fieldName = fieldName;
    }

    public String getStartDateString() {
        return startDateString;
    }

    public void setStartDateString(String startDateString) {
        this.startDateString = startDateString;
    }

    public String getEndDateString() {
        return endDateString;
    }

    public void setEndDateString(String endDateString) {
        this.endDateString = endDateString;
    }

    public String getRepeatDay() {
        return RepeatDay;
    }

    public void setRepeatDay(String repeatDay) {
        RepeatDay = repeatDay;
    }
}

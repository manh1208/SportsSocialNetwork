package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 11/3/16.
 */

public class Event {
    @SerializedName("Id")
    private int id;
    @SerializedName("Name")
    private String name;
    @SerializedName("StartDate")
    private String startDate;
    @SerializedName("StartDateString")
    private String startDateString;
    @SerializedName("EndDate")
    private String endDate;
    @SerializedName("EndDateString")
    private String endDateString;
    @SerializedName("PlaceId")
    private int placeId;
    @SerializedName("Description")
    private String description;
    @SerializedName("Image")
    private String image;
    @SerializedName("Status")
    private int status;
    @SerializedName("Active")
    private boolean active;
    @SerializedName("PlaceName")
    private String placeName;


    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getStartDate() {
        return startDate;
    }

    public void setStartDate(String startDate) {
        this.startDate = startDate;
    }

    public String getStartDateString() {
        return startDateString;
    }

    public void setStartDateString(String startDateString) {
        this.startDateString = startDateString;
    }

    public String getEndDate() {
        return endDate;
    }

    public void setEndDate(String endDate) {
        this.endDate = endDate;
    }

    public String getEndDateString() {
        return endDateString;
    }

    public void setEndDateString(String endDateString) {
        this.endDateString = endDateString;
    }

    public int getPlaceId() {
        return placeId;
    }

    public void setPlaceId(int placeId) {
        this.placeId = placeId;
    }

    public String getDescription() {
        return description;
    }

    public void setDescription(String description) {
        this.description = description;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }

    public int getStatus() {
        return status;
    }

    public void setStatus(int status) {
        this.status = status;
    }

    public boolean isActive() {
        return active;
    }

    public void setActive(boolean active) {
        this.active = active;
    }

    public String getPlaceName() {
        return placeName;
    }

    public void setPlaceName(String placeName) {
        this.placeName = placeName;
    }
}

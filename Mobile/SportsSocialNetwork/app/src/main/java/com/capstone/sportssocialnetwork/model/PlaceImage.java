package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 10/27/16.
 */

public class PlaceImage {
    @SerializedName("Id")
    private int id;
    @SerializedName("PlaceId")
    private int placeId;
    @SerializedName("Image")
    private String image;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getPlaceId() {
        return placeId;
    }

    public void setPlaceId(int placeId) {
        this.placeId = placeId;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }
}

package com.capstone.sportssocialnetwork.model.response;

import com.capstone.sportssocialnetwork.model.Place;
import com.capstone.sportssocialnetwork.model.Sport;
import com.google.gson.annotations.SerializedName;

import java.util.List;

/**
 * Created by ManhNV on 10/13/16.
 */
public class PlaceResponseModel extends Place {
    @SerializedName("SportList")
    private List<Sport> sports;

    public List<Sport> getSports() {
        return sports;
    }

    public void setSports(List<Sport> sports) {
        this.sports = sports;
    }
}

package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Place;
import com.capstone.sportssocialnetwork.model.response.PlaceResponseModel;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 10/13/16.
 */
public interface IPlaceService {

    @GET("/api/place/showallplaces")
    Call<ResponseModel<List<PlaceResponseModel>>> getAllPlace(@Query("skip")int skip,
                                                              @Query("take")int tack);

    @POST("/api/place/showplacedetail")
    Call<ResponseModel<PlaceResponseModel>> getPlaceDetail(@Query("id") int id);

//    @POST"/api/order/checkin"
}

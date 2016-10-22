package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.CheckIn;
import com.capstone.sportssocialnetwork.model.Order;
import com.capstone.sportssocialnetwork.model.request.OrderRequestModel;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.Body;
import retrofit2.http.POST;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 10/17/16.
 */
public interface IOrderService {
    @POST("/api/order/CheckInOrder")
    Call<ResponseModel<CheckIn>> checkInOrder(@Query("orderCode")String code);

    @POST("/api/order/showallorderofplaceowner")
    Call<ResponseModel<List<Order>>> getPlaceOwnerOrder(@Query("ownerId")String id);

    @POST("/api/order/getprice")
    Call<ResponseModel<Double>> getPrice(@Query("fieldId")int fieldId,
                                       @Query("startTime")String startTime,
                                       @Query("endTime")String endTime,
                                         @Query("playDate")String playDate);

    @POST("/api/order/showallorderofuser")
    Call<ResponseModel<List<Order>>> getMyOrder(@Query("userId")String userId);

    @POST("/api/order/changeorderstatus")
    Call<ResponseModel<Order>> changeStatusOrder(@Query("id")int id,
                                                 @Query("status")int status);

    @POST("/api/order/createorder")
    Call<ResponseModel<Order>> createOrder(@Body OrderRequestModel model);
}

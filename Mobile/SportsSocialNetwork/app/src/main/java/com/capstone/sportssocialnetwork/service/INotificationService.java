package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Notification;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.POST;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 10/26/16.
 */
public interface INotificationService {
    @POST("/api/notification/loadnoti")
    Call<ResponseModel<List<Notification>>> getNotifications(@Query("userId")String userId,
                                                             @Query("skip")int skip,
                                                             @Query("take")int take);

    @POST("/api/notification/markallasread")
    Call<ResponseModel<String>> martAllAsRead(@Query("userID")String userId);

}

package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.model.User;

import retrofit2.Call;
import retrofit2.http.POST;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 10/13/16.
 */
public interface IAccountService {
    @POST("/api/aspnetuser/login")
    Call<ResponseModel<User>> login(@Query("username")String username,
                                    @Query("password")String password);

    @POST("/api/aspnetuser/showprofile")
    Call<ResponseModel<User>> getUserProfile(@Query("userId")String userId);

    @POST("/api/aspnetuser/savetoken")
    Call<ResponseModel<String>> sentToken(@Query("userId")String userId,
                                          @Query("token")String token);

}

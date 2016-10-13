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
}

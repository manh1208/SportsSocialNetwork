package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Invitation;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.model.User;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.POST;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 10/13/16.
 */
public interface IAccountService {
    @POST("/api/aspnetuser/login")
    Call<ResponseModel<User>> login(@Query("username") String username,
                                    @Query("password") String password);

    @POST("/api/aspnetuser/showprofile")
    Call<ResponseModel<User>> getUserProfile(@Query("userId") String userId,@Query("currentUserId")String currentUserId);

    @POST("/api/aspnetuser/savetoken")
    Call<ResponseModel<String>> sentToken(@Query("userId") String userId,
                                          @Query("token") String token);

    @POST("/api/follow/getpeoplefollowyou")
    Call<ResponseModel<List<User>>> getFollowed(@Query("userId") String userId);

    @POST("/api/follow/getfollowlist")
    Call<ResponseModel<List<User>>> getFollowing(@Query("userId") String userId);

    @POST("/api/invitation/getsentinvitation")
    Call<ResponseModel<List<Invitation>>> getSentInvitation(@Query("userId") String userId);

    @POST("/api/invitation/getreceivedinvitation")
    Call<ResponseModel<List<Invitation>>> getReceiveInvitation(@Query("userId") String userId);

    @POST("/api/invitation/acceptinvitation")
    Call<ResponseModel<String>> acceptInvitation(@Query("id") int id);

    @POST("/api/invitation/refuseinvitation")
    Call<ResponseModel<String>> refuseInvitation(@Query("id") int id);

    @POST("/api/follow/followunfollowuser")
    Call<ResponseModel<String>> followUser(@Query("userId")String userId,
                                           @Query("followerId")String followerId);

    @GET("/api/aspnetuser/finduser")
    Call<ResponseModel<List<User>>> findUser(@Query("query")String query,
                                             @Query("skip")int skip,
                                             @Query("take")int take);


}

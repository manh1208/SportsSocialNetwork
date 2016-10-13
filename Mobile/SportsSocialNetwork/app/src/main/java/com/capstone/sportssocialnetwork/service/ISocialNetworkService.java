package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.POST;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 10/9/16.
 */
public interface ISocialNetworkService {

    @POST("api/post/showallpost")
    Call<ResponseModel<List<Feed>>> getAllPost(@Query("currentUserId") String currentUserId,
                                               @Query("take")int take,
                                                @Query("skip")int skip);
}

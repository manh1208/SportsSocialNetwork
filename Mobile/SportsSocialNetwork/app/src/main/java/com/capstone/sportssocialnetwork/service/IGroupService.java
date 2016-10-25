package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.POST;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 10/24/16.
 */
public interface IGroupService {

    @POST("/api/group/showalljoinedgroup")
    Call<ResponseModel<List<Group>>> getGroups(@Query("userId")String userId);
}

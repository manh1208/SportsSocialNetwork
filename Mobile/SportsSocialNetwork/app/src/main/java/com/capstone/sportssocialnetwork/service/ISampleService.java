package com.capstone.sportssocialnetwork.service;


import com.capstone.sportssocialnetwork.model.ResponseModel;
import com.capstone.sportssocialnetwork.model.SampleModel;

import retrofit2.Call;
import retrofit2.http.GET;
import retrofit2.http.Path;

/**
 * Created by ManhNV on 8/17/16.
 */
public interface ISampleService {

    @GET("/api/getsomething/{id}")
    Call<ResponseModel<SampleModel>> getSomething(@Path("id") int ahihi);
}

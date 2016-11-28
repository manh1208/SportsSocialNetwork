package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Category;
import com.capstone.sportssocialnetwork.model.News;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;

import java.util.List;

import retrofit2.Call;
import retrofit2.http.POST;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 11/10/16.
 */

public interface INewsService {

    @POST("/api/news/loadallcategory")
    Call<ResponseModel<List<Category>>> getAllCategory();

    @POST("/api/news/loadnewsofcategory")
    Call<ResponseModel<List<News>>> getNewsOfCategory(@Query("categoryId") int categoryid);

    @POST("/api/news/shownewsdetail")
    Call<ResponseModel<News>> getNewsDetail(@Query("id") int id);
}

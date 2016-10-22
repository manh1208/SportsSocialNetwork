package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Comment;
import com.capstone.sportssocialnetwork.model.Post;
import com.capstone.sportssocialnetwork.model.response.PostDetailResponseModel;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;

import java.util.List;

import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.http.Multipart;
import retrofit2.http.POST;
import retrofit2.http.Part;
import retrofit2.http.Query;

/**
 * Created by ManhNV on 10/22/16.
 */
public interface IPostService {

    @Multipart
    @POST("/api/post/post")
    Call<ResponseModel<Post>> createPost(@Part("userId") RequestBody userId,
                                         @Part("postContent") RequestBody postContent,
                                         @Part MultipartBody.Part file,
                                         @Part("groupId") RequestBody groupId);

    @POST("/api/post/ShowPostDetail")
    Call<ResponseModel<PostDetailResponseModel>> getPostDetail(@Query("postId") int postId,
                                                               @Query("currentUserId") String userId,
                                                               @Query("skip") int skip,
                                                               @Query("take") int take);

    @POST("/api/postcomment/getcomment")
    Call<ResponseModel<List<Comment>>> getComment(@Query("postId") int postId,
                                                  @Query("skip")int skip,
                                                  @Query("take")int take);
}

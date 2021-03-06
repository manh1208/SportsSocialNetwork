package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Comment;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.Post;
import com.capstone.sportssocialnetwork.model.PostImage;
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
                                                  @Query("skip") int skip,
                                                  @Query("take") int take);

    @Multipart
    @POST("/api/postcomment/comment")
    Call<ResponseModel<Comment>> commentPost(@Part("postId") RequestBody postId,
                                             @Part("userId") RequestBody userId,
                                             @Part("content") RequestBody content,
                                             @Part MultipartBody.Part file);

    @POST("/api/like/likeunlikepost")
    Call<ResponseModel<String>> likePost(@Query("postId") int postId,
                                         @Query("userId") String userId);

    @POST("/api/post/showalluserpost")
    Call<ResponseModel<List<Feed>>> getUserPost(@Query("userId") String userId,
                                                @Query("currentUserId") String currentUserId,
                                                @Query("take") int take,
                                                @Query("skip") int skip);

    @POST("/api/postimage/getallpostimage")
    Call<ResponseModel<List<PostImage>>> getAllUserPostImage(@Query("userId")String userId);

}

package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.model.GroupMember;
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
 * Created by ManhNV on 10/24/16.
 */
public interface IGroupService {

    @POST("/api/group/showalljoinedgroup")
    Call<ResponseModel<List<Group>>> getGroups(@Query("userId") String userId);

    @POST("/api/post/showallgroupposts")
    Call<ResponseModel<List<Feed>>> getGroupPost(@Query("groupId") int groupId,
                                                 @Query("currentUserId") String userId,
                                                 @Query("skip") int skip,
                                                 @Query("take") int take);

    @POST("/api/group/showgroupdetail")
    Call<ResponseModel<Group>> getGroupDetail(@Query("id") int groupId,
                                              @Query("currentUser") String userId);

    @POST("/api/groupmember/showmemberlist")
    Call<ResponseModel<List<GroupMember>>> getGroupMember(@Query("groupId") int groupId);

    @POST("/api/groupmember/leavegroup")
    Call<ResponseModel<String>> leaveGroup(@Query("groupId") int groupId,
                                           @Query("userId") String userId);

    @POST("/api/groupmember/kickmember")
    Call<ResponseModel<GroupMember>> kickFromGroup(@Query("id")int groupMemberId);

    @Multipart
    @POST("/api/group/creategroup")
    Call<ResponseModel<Group>> createGroup(@Part("name")RequestBody name,
                                           @Part("description")RequestBody description,
                                           @Part("sportId")int sportId,
                                           @Part("userId")RequestBody userId,
                                           @Part MultipartBody.Part avatar
                                           );
}

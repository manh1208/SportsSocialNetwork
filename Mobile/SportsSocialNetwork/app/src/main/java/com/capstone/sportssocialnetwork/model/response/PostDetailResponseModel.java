package com.capstone.sportssocialnetwork.model.response;

import com.capstone.sportssocialnetwork.model.Comment;
import com.capstone.sportssocialnetwork.model.Feed;
import com.google.gson.annotations.SerializedName;

import java.util.List;

/**
 * Created by ManhNV on 10/23/16.
 */
public class PostDetailResponseModel {
    @SerializedName("Post")
    private Feed post;
    @SerializedName("CommentList")
    private List<Comment> comments;

    public Feed getPost() {
        return post;
    }

    public void setPost(Feed post) {
        this.post = post;
    }

    public List<Comment> getComments() {
        return comments;
    }

    public void setComments(List<Comment> comments) {
        this.comments = comments;
    }
}

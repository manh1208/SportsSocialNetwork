package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

/**
 * Created by ManhNV on 11/10/16.
 */

public class News {
    @SerializedName("Categories")
    private Category category;
    @SerializedName("Id")
    private int id;
    @SerializedName("UserId")
    private String userId;
    @SerializedName("CreateDate")
    private String createDate;
    @SerializedName("Title")
    private String title;
    @SerializedName("NewsContent")
    private String content;
    @SerializedName("Image")
    private String image;
    @SerializedName("CategoryId")
    private int categoryId;
    @SerializedName("NumOfRead")
    private int numOfRead;

    public Category getCategory() {
        return category;
    }

    public void setCategory(Category category) {
        this.category = category;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public String getImage() {
        return image;
    }

    public void setImage(String image) {
        this.image = image;
    }

    public int getCategoryId() {
        return categoryId;
    }

    public void setCategoryId(int categoryId) {
        this.categoryId = categoryId;
    }

    public int getNumOfRead() {
        return numOfRead;
    }

    public void setNumOfRead(int numOfRead) {
        this.numOfRead = numOfRead;
    }
}

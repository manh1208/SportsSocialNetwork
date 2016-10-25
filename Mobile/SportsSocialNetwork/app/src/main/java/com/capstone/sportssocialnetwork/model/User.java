package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

import java.util.List;

/**
 * Created by ManhNV on 10/12/16.
 */
public class User {
    @SerializedName("Id")
    private String Id;
    @SerializedName("FullName")
    private String fullName;
    @SerializedName("AvatarImage")
    private String avatar;
    @SerializedName("AspNetRoles")
    private List<Role> roles;


    public String getId() {
        return Id;
    }

    public void setId(String id) {
        Id = id;
    }

    public String getFullName() {
        return fullName;
    }

    public void setFullName(String fullName) {
        this.fullName = fullName;
    }

    public String getAvatar() {
        return avatar;
    }

    public void setAvatar(String avatar) {
        this.avatar = avatar;
    }

    public Role getRole() {
        if (roles!=null && roles.size()>0) {
            return roles.get(0);
        }else{
            return null;
        }
    }

    public List<Role> getRoles() {
        return roles;
    }

    public void setRoles(List<Role> roles) {
        this.roles = roles;
    }
}

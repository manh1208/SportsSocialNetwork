package com.capstone.sportssocialnetwork.model;

import android.view.View;

import com.capstone.sportssocialnetwork.R;
import com.google.gson.annotations.SerializedName;

import java.util.List;

/**
 * Created by ManhNV on 11/7/16.
 */

public class Invitation {
    @SerializedName("CreateDateString")
    private String createDateString;
    @SerializedName("UserInvitations")
    private List<UserInvitation> userInvitation;
    @SerializedName("Id")
    private int id;
    @SerializedName("SenderName")
    private String name;
    @SerializedName("Subject")
    private String subject;
    @SerializedName("SenderId")
    private String senderId;
    @SerializedName("InvitationContent")
    private String content;
    @SerializedName("CreateDate")
    private String createDate;

    public String getCreateDateString() {
        return createDateString;
    }

    public void setCreateDateString(String createDateString) {
        this.createDateString = createDateString;
    }

    public List<UserInvitation> getUserInvitation() {
        return userInvitation;
    }

    public void setUserInvitation(List<UserInvitation> userInvitation) {
        this.userInvitation = userInvitation;
    }

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getSenderId() {
        return senderId;
    }

    public void setSenderId(String senderId) {
        this.senderId = senderId;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public String getCreateDate() {
        return createDate;
    }

    public void setCreateDate(String createDate) {
        this.createDate = createDate;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public String getSubject() {
        return subject;
    }

    public void setSubject(String subject) {
        this.subject = subject;
    }

    public Boolean isAccepted(String userId){
        for (UserInvitation userInvitation: getUserInvitation()
                ) {
            if (userInvitation.getUser().getId().equals(userId)){
                return userInvitation.isAccepted();
            }
        }
        return null;
    }

    public UserInvitation getUserInvitation(String userId){
        for (UserInvitation userInvitation: getUserInvitation()
                ) {
            if (userInvitation.getUser().getId().equals(userId)){
                return userInvitation;
            }
        }
        return null;
    }

    public int getAcceptedCount(){
        int count = 0;
        for (UserInvitation userInvitation: getUserInvitation()
                ) {
            if (userInvitation.isAccepted()!=null && userInvitation.isAccepted()){
                count++;
            }

        }
        return count;
    }
}

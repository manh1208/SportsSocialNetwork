package com.capstone.sportssocialnetwork.model;

import java.util.HashMap;
import java.util.Map;

/**
 * Created by ManhNV on 11/11/16.
 */

public class Message {
    private String sender;
    private String message;
    private String userId;


    public Message() {
    }

    public String getSender() {
        return sender;
    }

    public void setSender(String sender) {
        this.sender = sender;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public String getUserId() {
        return userId;
    }

    public void setUserId(String userId) {
        this.userId = userId;
    }

    public Message(String sender, String message, String userId) {
        this.sender = sender;
        this.message = message;
        this.userId = userId;
    }

    public Map<String, Object> toMap() {
        HashMap<String, Object> result = new HashMap<>();
        result.put("sender", sender);
        result.put("message", message);
        result.put("userId", userId);
        return result;
    }

    public void fromMap(Map<String, String> obj){
        this.sender =  obj.get("sender")+"";
        this.message =  obj.get("message")+"";
        this.userId = obj.get("userId")+"";
    }
}

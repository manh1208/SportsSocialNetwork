package com.capstone.sportssocialnetwork.model;

import com.google.gson.annotations.SerializedName;

import java.util.List;

/**
 * Created by ManhNV on 8/17/16.
 */
public class ResponseModel<T> {
    @SerializedName("Succeed")
    private boolean succeed;
    @SerializedName("Message")
    private String message;
    @SerializedName("Errors")
    private List<String> errors;
    @SerializedName("Data")
    private T data;

    public boolean isSucceed() {
        return succeed;
    }

    public String getMessage() {
        return message;
    }

    public List<String> getErrors() {
        return errors;
    }

    public String getErrorsString() {
        String errorsString = this.message+"\n";
        if (getErrors()!=null && getErrors().size()>0) {
            for (String item : getErrors()
                    ) {
                errorsString += item + "\n";
            }
        }
        return errorsString;
    }

    public T getData() {
        return data;
    }
}

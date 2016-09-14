package com.capstone.sportssocialnetwork.model;

import android.os.Parcel;
import android.os.Parcelable;

import com.felipecsl.asymmetricgridview.library.model.AsymmetricItem;

/**
 * Created by ManhNV on 9/11/16.
 */
public class Image implements AsymmetricItem, Parcelable {

    public Image(){

    }

    protected Image(Parcel in) {
    }

    public static final Creator<Image> CREATOR = new Creator<Image>() {
        @Override
        public Image createFromParcel(Parcel in) {
            return new Image(in);
        }

        @Override
        public Image[] newArray(int size) {
            return new Image[size];
        }
    };

    @Override
    public int getColumnSpan() {
        return 1;
    }

    @Override
    public int getRowSpan() {
        return 1;
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel parcel, int i) {
    }
}

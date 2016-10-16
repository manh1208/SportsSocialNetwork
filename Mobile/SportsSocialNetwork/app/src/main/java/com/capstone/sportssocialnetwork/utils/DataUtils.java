package com.capstone.sportssocialnetwork.utils;

import android.content.Context;
import android.content.SharedPreferences;

/**
 * Created by ManhNV on 10/9/16.
 */
public class DataUtils {
//    public static String URL = "http://192.168.43.200:26011";
    public static String URL = "http://192.168.150.149:26011";

    private static DataUtils INSTANCE = null;
    private SharedPreferences mPreferences;
    private Context mContext;
    private Utilities utilities;

    private DataUtils(Context context) {
        mContext = context;
        mPreferences = mContext.getSharedPreferences("SSN", 0);
        utilities = new Utilities();
    }

    public static DataUtils getINSTANCE(Context context) {
        if (INSTANCE == null) {
            INSTANCE = new DataUtils(context);
        }
        return INSTANCE;
    }

    public SharedPreferences getPreferences() {
        return mPreferences;
    }
}

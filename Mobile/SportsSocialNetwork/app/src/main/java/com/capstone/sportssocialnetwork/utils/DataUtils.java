package com.capstone.sportssocialnetwork.utils;

import android.content.Context;
import android.content.SharedPreferences;

/**
 * Created by ManhNV on 10/9/16.
 */
public class DataUtils {
//    public static String URL = "http://192.168.43.200:26011";
//    public static String URL = "http://192.168.150.155:26011";
//    public static String URL = "http://192.168.1.111:26011";
    public static String URL = "http://192.168.43.213:26011";

    public static String FORMAT_DATE_TIME = "dd/MM/yyyy HH:mm:ss";
    public static String FORMAT_DATE = "dd/MM/yyyy";
    public static String FORMAT_TIME = "HH:mm";


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

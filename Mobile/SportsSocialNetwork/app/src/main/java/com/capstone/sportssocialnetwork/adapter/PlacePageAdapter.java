package com.capstone.sportssocialnetwork.adapter;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import com.capstone.sportssocialnetwork.fragment.PlaceDetailFragment;
import com.capstone.sportssocialnetwork.fragment.PlaceFieldFragment;
import com.capstone.sportssocialnetwork.fragment.PlaceImageFragment;

import java.util.ArrayList;

/**
 * Created by ManhNV on 9/8/16.
 */
public class PlacePageAdapter extends FragmentPagerAdapter {
    private int placeId;

    public PlacePageAdapter(FragmentManager fm, int placeId) {
        super(fm);
        this.placeId = placeId;
    }

    @Override
    public Fragment getItem(int position) {
        Fragment fragment  = null;
        switch (position){
            case 0:
                fragment =  new PlaceDetailFragment();
                break;
            case 1:
                fragment = new PlaceFieldFragment();
                break;
            case 2:
                fragment =  new PlaceImageFragment();
                break;
        }
        if (fragment!=null) {
            Bundle bundle = new Bundle();
            bundle.putInt("placeId", placeId);
            fragment.setArguments(bundle);
        }
        return fragment;
    }

    @Override
    public int getCount() {
        return 3;

    }

    @Override
    public CharSequence getPageTitle(int position) {
        switch (position){
            case 0:
                return "Thông tin chi tiết";
            case 1:
                return "Danh sách sân";
            case 2:
                return "Hình ảnh";
        }
        return "";
    }
}

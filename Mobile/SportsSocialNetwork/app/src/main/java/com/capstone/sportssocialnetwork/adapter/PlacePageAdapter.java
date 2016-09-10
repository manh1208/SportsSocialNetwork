package com.capstone.sportssocialnetwork.adapter;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import com.capstone.sportssocialnetwork.fragment.PlaceDetailFragment;
import com.capstone.sportssocialnetwork.fragment.PlaceFieldFragment;

import java.util.ArrayList;

/**
 * Created by ManhNV on 9/8/16.
 */
public class PlacePageAdapter extends FragmentPagerAdapter {


    public PlacePageAdapter(FragmentManager fm) {
        super(fm);
    }

    @Override
    public Fragment getItem(int position) {
        switch (position){
            case 0:
                return new PlaceDetailFragment();
            case 1:
                return new PlaceFieldFragment();
        }
        return null;
    }

    @Override
    public int getCount() {
        return 2;

    }

    @Override
    public CharSequence getPageTitle(int position) {
        switch (position){
            case 0:
                return "Thông tin chi tiết";
            case 1:
                return "Danh sách sân";
        }
        return "";
    }
}

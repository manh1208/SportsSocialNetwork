package com.capstone.sportssocialnetwork.adapter;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import com.capstone.sportssocialnetwork.fragment.PlaceDetailFragment;
import com.capstone.sportssocialnetwork.fragment.PlaceFieldFragment;
import com.capstone.sportssocialnetwork.fragment.PlaceImageFragment;
import com.capstone.sportssocialnetwork.fragment.ProfileContentFragment;
import com.capstone.sportssocialnetwork.fragment.ProfileImageFragment;
import com.capstone.sportssocialnetwork.fragment.ProfilePostFragment;

/**
 * Created by ManhNV on 10/3/16.
 */
public class ProfilePageAdapter extends FragmentPagerAdapter {


    public ProfilePageAdapter(FragmentManager fm) {
        super(fm);
    }

    @Override
    public Fragment getItem(int position) {
        switch (position){
            case 0:
                return new ProfileContentFragment();
            case 1:
                return new ProfilePostFragment();
            case 2:
                return new ProfileImageFragment();
        }
        return null;
    }

    @Override
    public int getCount() {
        return 3;

    }

    @Override
    public CharSequence getPageTitle(int position) {
        switch (position){
            case 0:
                return "Thông tin cá nhân";
            case 1:
                return "Bài đăng";
            case 2:
                return "Hình ảnh";
        }
        return "";
    }
}
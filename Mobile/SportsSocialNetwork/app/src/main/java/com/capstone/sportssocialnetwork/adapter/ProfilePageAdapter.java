package com.capstone.sportssocialnetwork.adapter;

import android.os.Bundle;
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

    private String userId;

    public ProfilePageAdapter(FragmentManager fm, String userId) {
        super(fm);
        this.userId = userId;
    }

    @Override
    public Fragment getItem(int position) {
        Fragment fragment = null;
        switch (position) {
            case 0:
                fragment = new ProfileContentFragment();
                break;
            case 1:
                fragment = new ProfilePostFragment();
                break;
            case 2:
                fragment = new ProfileImageFragment();
                break;

        }

        if (fragment != null) {
            Bundle bundle = new Bundle();
            bundle.putString("userId", userId);
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
        switch (position) {
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
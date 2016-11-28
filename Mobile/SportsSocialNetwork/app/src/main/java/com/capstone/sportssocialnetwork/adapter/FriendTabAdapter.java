package com.capstone.sportssocialnetwork.adapter;

import android.app.FragmentManager;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentPagerAdapter;
import android.support.v4.view.PagerAdapter;
import android.view.View;

import com.capstone.sportssocialnetwork.fragment.FollowedFriendFragment;
import com.capstone.sportssocialnetwork.fragment.FollowingFriendFragment;

/**
 * Created by ManhNV on 11/5/16.
 */
public class FriendTabAdapter extends FragmentPagerAdapter {
    public FriendTabAdapter(android.support.v4.app.FragmentManager fragmentManager) {
        super(fragmentManager);
    }


    @Override
    public int getCount() {
        return 2;
    }

    @Override
    public Fragment getItem(int position) {
        switch (position){
            case 0 : return new FollowingFriendFragment();
            case 1 : return new FollowedFriendFragment();
        }
        return null;
    }


    @Override
    public CharSequence getPageTitle(int position) {
        switch (position){
            case 0 :
                return "Đang theo dõi";
            case 1:
                return "Theo dõi";
        }
        return null;
    }
}

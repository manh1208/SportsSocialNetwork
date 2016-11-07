package com.capstone.sportssocialnetwork.adapter;

import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentPagerAdapter;

import com.capstone.sportssocialnetwork.fragment.FollowedFriendFragment;
import com.capstone.sportssocialnetwork.fragment.FollowingFriendFragment;
import com.capstone.sportssocialnetwork.fragment.InvitationReceiveFragment;
import com.capstone.sportssocialnetwork.fragment.InvitationSendFragment;

/**
 * Created by ManhNV on 11/5/16.
 */
public class InvitationTabAdapter extends FragmentPagerAdapter {
    public InvitationTabAdapter(android.support.v4.app.FragmentManager fragmentManager) {
        super(fragmentManager);
    }


    @Override
    public int getCount() {
        return 2;
    }

    @Override
    public Fragment getItem(int position) {
        switch (position){
            case 0 : return new InvitationSendFragment();
            case 1 : return new InvitationReceiveFragment();
        }
        return null;
    }


    @Override
    public CharSequence getPageTitle(int position) {
        switch (position){
            case 0 :
                return "Đã gửi";
            case 1:
                return "Đã nhận";
        }
        return null;
    }
}

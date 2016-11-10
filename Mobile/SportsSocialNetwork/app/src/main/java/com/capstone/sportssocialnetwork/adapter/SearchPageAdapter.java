package com.capstone.sportssocialnetwork.adapter;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;
import android.util.Log;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.fragment.GroupSearchFragment;
import com.capstone.sportssocialnetwork.fragment.PeopleFragment;
import com.capstone.sportssocialnetwork.fragment.PlaceFieldFragment;
import com.capstone.sportssocialnetwork.listener.UpdateSearchFragment;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by ManhNV on 9/8/16.
 */
public class SearchPageAdapter extends FragmentPagerAdapter {
    private String query;

    public SearchPageAdapter(FragmentManager fm,String query) {
        super(fm);
        this.query = query;

    }

    @Override
    public Fragment getItem(int position) {
        Fragment fragment  = null;
        switch (position){
            case 0:
                fragment =  new PeopleFragment();
                break;
            case 1:
                fragment = new GroupSearchFragment();
                break;
        }
        if (fragment!=null) {
            Bundle bundle = new Bundle();
            bundle.putString("query", query);
            fragment.setArguments(bundle);
        }
        return fragment;
    }

    @Override
    public int getItemPosition(Object object) {
        UpdateSearchFragment fragment = (UpdateSearchFragment) object;
        fragment.update(query);
//        if (fragment!=null) {
//            Bundle bundle = new Bundle();
//            bundle.putString("query", query);
//            fragment.se(bundle);
//        }
        return POSITION_UNCHANGED;
    }


    @Override
    public int getCount() {
        return 2;

    }

    public void setQuery(String query) {
        this.query = query;
        notifyDataSetChanged();
        Log.i("SearchPage","Update");
//        Toast.makeText(this, "", Toast.LENGTH_SHORT).show();
    }

    @Override
    public CharSequence getPageTitle(int position) {
        switch (position){
            case 0:
                return "Mọi người";
            case 1:
                return "Nhóm";
        }
        return "";
    }
}

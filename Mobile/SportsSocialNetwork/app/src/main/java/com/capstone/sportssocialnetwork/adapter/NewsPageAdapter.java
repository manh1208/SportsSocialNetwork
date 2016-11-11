package com.capstone.sportssocialnetwork.adapter;

import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentPagerAdapter;

import com.capstone.sportssocialnetwork.fragment.NewsCategoryFragment;
import com.capstone.sportssocialnetwork.model.Category;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by ManhNV on 11/10/16.
 */

public class NewsPageAdapter extends FragmentPagerAdapter {
    private List<Category> categories;

    public NewsPageAdapter(FragmentManager fragmentManager) {
        super(fragmentManager);
        categories = new ArrayList<>();
    }

    @Override
    public Fragment getItem(int position) {

        NewsCategoryFragment fragment = new NewsCategoryFragment();
        Bundle bundle = new Bundle();
        bundle.putInt("categoryId", categories.get(position).getId());
        fragment.setArguments(bundle);
        return fragment;

    }

    @Override
    public int getCount() {
        return categories.size();
    }

    @Override
    public CharSequence getPageTitle(int position) {
        return categories.get(position).getName();


    }

    public void setCategories(List<Category> categories) {
        this.categories = categories;
        notifyDataSetChanged();
    }
}
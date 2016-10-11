package com.capstone.sportssocialnetwork.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.GridView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.ImageAdapter;
import com.capstone.sportssocialnetwork.adapter.PlaceDetailAdapter;

/**
 * Created by ManhNV on 10/3/16.
 */
public class ProfileImageFragment extends Fragment {
    GridView lvProfileImage;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile_image, container, false);
        initView(v);
        prepareData();
        return v;
    }

    private void prepareData() {
        ImageAdapter adapter = new ImageAdapter(getActivity());
        lvProfileImage.setAdapter(adapter);
    }

    private void initView(View v) {
        lvProfileImage = (GridView) v.findViewById(R.id.lv_profile_image);
    }
}

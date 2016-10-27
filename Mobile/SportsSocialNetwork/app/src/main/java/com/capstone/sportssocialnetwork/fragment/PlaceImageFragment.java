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
import com.capstone.sportssocialnetwork.adapter.PlaceImageAdapter;
import com.capstone.sportssocialnetwork.model.PostImage;

import java.util.ArrayList;

/**
 * Created by ManhNV on 9/10/16.
 */
public class PlaceImageFragment extends Fragment {
    GridView lvPlaceImage;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile_image, container, false);
        initView(v);
        prepareData();
        return v;
    }
    private void prepareData() {
//        PlaceImageAdapter adapter = new PlaceImageAdapter(getActivity());
//        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getActivity().getApplicationContext());
//        lvPlaceImage.setLayoutManager(mLayoutManager);
//        lvPlaceImage.setItemAnimator(new DefaultItemAnimator());
//        lvPlaceImage.setAdapter(adapter);
        ImageAdapter adapter =  new ImageAdapter(getActivity(),R.layout.item_image,new ArrayList<PostImage>());
        lvPlaceImage.setAdapter(adapter);
    }

    private void initView(View v) {
        lvPlaceImage = (GridView) v.findViewById(R.id.lv_profile_image);
    }
}

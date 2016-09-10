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

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.PlaceDetailAdapter;
import com.capstone.sportssocialnetwork.adapter.PlaceImageAdapter;

/**
 * Created by ManhNV on 9/10/16.
 */
public class PlaceImageFragment extends Fragment {
    RecyclerView lvPlaceImage;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_place_image, container, false);
        initView(v);
        prepareData();
        return v;
    }
    private void prepareData() {
        PlaceImageAdapter adapter = new PlaceImageAdapter(getActivity());
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getActivity().getApplicationContext());
        lvPlaceImage.setLayoutManager(mLayoutManager);
        lvPlaceImage.setItemAnimator(new DefaultItemAnimator());
        lvPlaceImage.setAdapter(adapter);
    }

    private void initView(View v) {
        lvPlaceImage = (RecyclerView) v.findViewById(R.id.lv_place_image);
    }
}

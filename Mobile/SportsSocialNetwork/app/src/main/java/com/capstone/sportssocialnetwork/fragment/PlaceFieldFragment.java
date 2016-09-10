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

/**
 * Created by ManhNV on 9/8/16.
 */
public class PlaceFieldFragment extends Fragment {
    RecyclerView lvPlaceField;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_place_field, container, false);
        initView(v);
        prepareData();
        return v;
    }
    private void prepareData() {
        PlaceDetailAdapter adapter = new PlaceDetailAdapter(getActivity());
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getActivity().getApplicationContext());
        lvPlaceField.setLayoutManager(mLayoutManager);
        lvPlaceField.setItemAnimator(new DefaultItemAnimator());
        lvPlaceField.setAdapter(adapter);
    }

    private void initView(View v) {
        lvPlaceField = (RecyclerView) v.findViewById(R.id.lv_place_field);
    }
}

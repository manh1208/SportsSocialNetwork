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
import android.widget.ArrayAdapter;
import android.widget.Spinner;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.PlaceDetailAdapter;
import com.capstone.sportssocialnetwork.adapter.PlaceFieldAdapter;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by ManhNV on 9/8/16.
 */
public class PlaceFieldFragment extends Fragment {
    RecyclerView lvPlaceField;
    Spinner spinerSport;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_place_field, container, false);
        initView(v);
        prepareData();
        createSpinner();
        return v;
    }

    private void createSpinner() {
        List<String> sports = new ArrayList<String>();
        sports.add("Tất cả");
        sports.add("Bóng đá");
        sports.add("Bóng rổ");
        sports.add("Bóng chuyền");
        ArrayAdapter<String> arrayAdapter = new ArrayAdapter(getActivity(), R.layout.item_spinner, sports);
        arrayAdapter.setDropDownViewResource(R.layout.item_spinner);
        spinerSport.setAdapter(arrayAdapter);
        spinerSport.setSelection(0);
    }

    private void prepareData() {
        PlaceFieldAdapter adapter = new PlaceFieldAdapter(getActivity());
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getActivity().getApplicationContext());
        lvPlaceField.setLayoutManager(mLayoutManager);
        lvPlaceField.setItemAnimator(new DefaultItemAnimator());
        lvPlaceField.setAdapter(adapter);
    }

    private void initView(View v) {
        lvPlaceField = (RecyclerView) v.findViewById(R.id.lv_place_field);
        spinerSport  = (Spinner) v.findViewById(R.id.spinner_sport);
    }


}

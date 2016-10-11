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
import com.capstone.sportssocialnetwork.adapter.ProfileDetailAdapter;

/**
 * Created by ManhNV on 10/3/16.
 */
public class ProfileContentFragment extends Fragment {

    RecyclerView lvProfileDetail;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile_content, container, false);
        initView(v);
        prepareData();
        return v;
    }

    private void prepareData() {
        ProfileDetailAdapter adapter = new ProfileDetailAdapter(getActivity());
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getActivity().getApplicationContext());
        lvProfileDetail.setLayoutManager(mLayoutManager);
        lvProfileDetail.setItemAnimator(new DefaultItemAnimator());
        lvProfileDetail.setAdapter(adapter);
    }

    private void initView(View v) {
        lvProfileDetail = (RecyclerView) v.findViewById(R.id.lv_profile_detail);
    }
}

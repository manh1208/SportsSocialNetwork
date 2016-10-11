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
import com.capstone.sportssocialnetwork.adapter.ProfilePostAdapter;

/**
 * Created by ManhNV on 10/3/16.
 */
public class ProfilePostFragment extends Fragment {
    RecyclerView lvProfilePost;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile_post, container, false);
        initView(v);
        prepareData();
        return v;
    }

    private void prepareData() {
        ProfilePostAdapter adapter = new ProfilePostAdapter(getActivity());
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getActivity().getApplicationContext());
        lvProfilePost.setLayoutManager(mLayoutManager);
        lvProfilePost.setItemAnimator(new DefaultItemAnimator());
        lvProfilePost.setAdapter(adapter);
    }

    private void initView(View v) {
        lvProfilePost = (RecyclerView) v.findViewById(R.id.lv_profile_post);
    }
}

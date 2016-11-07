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
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.ImageAdapter;
import com.capstone.sportssocialnetwork.adapter.PlaceDetailAdapter;
import com.capstone.sportssocialnetwork.model.PostImage;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 10/3/16.
 */
public class ProfileImageFragment extends Fragment {
    GridView lvProfileImage;
    ImageAdapter adapter;
    RestService service;
    String userId;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile_image, container, false);
        initView(v);
        prepareData();
        return v;
    }

    private void prepareData() {
        adapter = new ImageAdapter(getActivity(),R.layout.item_image,new ArrayList<PostImage>());
        lvProfileImage.setAdapter(adapter);
    }

    private void initView(View v) {
        userId = getArguments().getString("userId");
        service = new RestService();
        lvProfileImage = (GridView) v.findViewById(R.id.lv_profile_image);
    }

    private void loadData(){
        service.getPostService().getAllUserPostImage(userId).enqueue(new Callback<ResponseModel<List<PostImage>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<PostImage>>> call, Response<ResponseModel<List<PostImage>>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setImages(response.body().getData());
                    }else{
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<PostImage>>> call, Throwable t) {
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    public void onResume() {
        super.onResume();
        loadData();
    }
}

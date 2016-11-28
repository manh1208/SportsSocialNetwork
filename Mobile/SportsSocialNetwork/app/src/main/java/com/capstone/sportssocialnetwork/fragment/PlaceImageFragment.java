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
import com.capstone.sportssocialnetwork.model.PlaceImage;
import com.capstone.sportssocialnetwork.model.PostImage;
import com.capstone.sportssocialnetwork.model.response.PlaceResponseModel;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;

import java.util.ArrayList;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/10/16.
 */
public class PlaceImageFragment extends Fragment {
    GridView lvPlaceImage;
    private int placeId;
    private PlaceResponseModel place;
    private RestService service;
    private PlaceImageAdapter adapter;

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
         adapter =  new PlaceImageAdapter(getActivity(),R.layout.item_image,new ArrayList<PlaceImage>());
        lvPlaceImage.setAdapter(adapter);
    }

    private void loadData() {

        Call<ResponseModel<PlaceResponseModel>> call = service.getPlaceService().getPlaceDetail(placeId);
        call.enqueue(new Callback<ResponseModel<PlaceResponseModel>>() {
            @Override
            public void onResponse(Call<ResponseModel<PlaceResponseModel>> call, Response<ResponseModel<PlaceResponseModel>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        place = response.body().getData();
                        adapter.setImages(place.getPlaceImages());

                    }
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<PlaceResponseModel>> call, Throwable t) {

            }
        });
    }

    private void initView(View v) {
        service = new RestService();
        placeId = getArguments().getInt("placeId");
        lvPlaceImage = (GridView) v.findViewById(R.id.lv_profile_image);
    }

    @Override
    public void onResume() {
        super.onResume();
        if (place==null){
            loadData();
        }else{
            adapter.setImages(place.getPlaceImages());
        }
    }
}

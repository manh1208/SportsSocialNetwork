package com.capstone.sportssocialnetwork.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.ManagePlaceAdapter;
import com.capstone.sportssocialnetwork.model.Place;
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
 * Created by ManhNV on 10/14/16.
 */
public class ManagePlaceFragment extends Fragment {
    ViewHolder viewHolder;
    RestService service;
    String userId;
    ManagePlaceAdapter adapter;


    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v= inflater.inflate(R.layout.fragment_manage_place,container,false);
        initView(v);
        prepareDate();
        return v;
    }

    private void prepareDate() {
        viewHolder.lvPlace.setAdapter(adapter);
    }

    private void initView(View v) {
        viewHolder = new ViewHolder(v);
        service = new RestService();
        userId = DataUtils.getINSTANCE(getActivity()).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        adapter = new ManagePlaceAdapter(getActivity(),R.layout.item_place_manage,new ArrayList<Place>());
    }

    private final class ViewHolder{
        ListView lvPlace;
        ViewHolder(View v){
            lvPlace = (ListView) v.findViewById(R.id.lv_manage_place);
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        loadData();
    }

    private void loadData() {
        Call<ResponseModel<List<Place>>> call = service.getPlaceService().getAllPlaceOwnerPlace(userId);
        call.enqueue(new Callback<ResponseModel<List<Place>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Place>>> call, Response<ResponseModel<List<Place>>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setPlaces(response.body().getData());
                    }else{
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Place>>> call, Throwable t) {
                Toast.makeText(getActivity(), "Loi ket noi server", Toast.LENGTH_SHORT).show();
            }
        });
    }
}
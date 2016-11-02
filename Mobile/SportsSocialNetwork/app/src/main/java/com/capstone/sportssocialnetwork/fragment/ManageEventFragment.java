package com.capstone.sportssocialnetwork.fragment;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.GroupInformationActivity;
import com.capstone.sportssocialnetwork.activity.MainBottomBarActivity;
import com.capstone.sportssocialnetwork.adapter.ManageEventAdapter;
import com.capstone.sportssocialnetwork.model.Event;
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
 * Created by ManhNV on 11/2/16.
 */

public class ManageEventFragment extends Fragment {
    private ViewHolder viewHolder;
    private RestService service;
    private String userId;
    private ManageEventAdapter manageEventAdapter;


    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {

        View v = inflater.inflate(R.layout.fragment_manage_event,container,false);
        initView(v);
        event();
        return v;
    }

    private void event() {
        viewHolder.layoutRefresh.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
                loadData();
            }
        });
    }

    private void initView(View v) {
        viewHolder = new ViewHolder(v);
        service = new RestService();
        userId = DataUtils.getINSTANCE(getActivity()).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        manageEventAdapter = new ManageEventAdapter(getActivity(),R.layout.item_manage_event,new ArrayList<Event>());
        viewHolder.lvEvents.setAdapter(manageEventAdapter);
    }

    private void loadData(){
        service.getPlaceService().getEventOfPlaceOwner(userId).enqueue(new Callback<ResponseModel<List<Event>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Event>>> call, Response<ResponseModel<List<Event>>> response) {
                if (viewHolder.layoutRefresh.isRefreshing()){
                    viewHolder.layoutRefresh.setRefreshing(false);
                }
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                       manageEventAdapter.setEventList(response.body().getData());
                    } else {
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Event>>> call, Throwable t) {
                if (viewHolder.layoutRefresh.isRefreshing()){
                    viewHolder.layoutRefresh.setRefreshing(false);
                }
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private final class ViewHolder{
        ListView lvEvents;
        SwipeRefreshLayout layoutRefresh;
        ViewHolder(View v){
            lvEvents = (ListView) v.findViewById(R.id.lv_manage_event_list);
            layoutRefresh = (SwipeRefreshLayout) v.findViewById(R.id.layout_refresh);
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        loadData();
    }
}

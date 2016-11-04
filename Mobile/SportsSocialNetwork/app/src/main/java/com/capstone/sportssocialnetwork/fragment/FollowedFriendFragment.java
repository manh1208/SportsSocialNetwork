package com.capstone.sportssocialnetwork.fragment;

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
import com.capstone.sportssocialnetwork.adapter.FriendAdapter;
import com.capstone.sportssocialnetwork.model.User;
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
 * Created by ManhNV on 11/5/16.
 */

public class FollowedFriendFragment extends Fragment {
    ViewHolder viewHolder;
    FriendAdapter adapter;
    RestService service;
    private String userId;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_followed_friend, container, false);
        viewHolder = new ViewHolder(v);
        init();
        event();
        return v;
    }

    private void init() {
        userId = DataUtils.getINSTANCE(getActivity()).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        service = new RestService();
        adapter = new FriendAdapter(getActivity(),R.layout.item_friend,new ArrayList<User>(),true);
        viewHolder.lvFollowed.setAdapter(adapter);
    }

    private void event() {
        viewHolder.refreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
                loadData();
            }
        });
    }

    private void loadData() {
        service.getAccountService().getFollowed(userId).enqueue(new Callback<ResponseModel<List<User>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<User>>> call, Response<ResponseModel<List<User>>> response) {
                if (viewHolder.refreshLayout.isRefreshing()){
                    viewHolder.refreshLayout.setRefreshing(false);
                }
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setUsers(response.body().getData());
                    }else{
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(getActivity(),response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<User>>> call, Throwable t) {
                if (viewHolder.refreshLayout.isRefreshing()){
                    viewHolder.refreshLayout.setRefreshing(false);
                }
                Toast.makeText(getActivity(),R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private final class ViewHolder{
        SwipeRefreshLayout refreshLayout;
        ListView lvFollowed;
        ViewHolder(View v){
            refreshLayout = (SwipeRefreshLayout) v.findViewById(R.id.layout_refresh);
            lvFollowed = (ListView) v.findViewById(R.id.lv_friend_followed);

        }

    }

    @Override
    public void onResume() {
        super.onResume();
        loadData();
    }
}

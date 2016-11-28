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
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.adapter.ProfileDetailAdapter;
import com.capstone.sportssocialnetwork.model.User;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 10/3/16.
 */
public class ProfileContentFragment extends Fragment {

    RecyclerView lvProfileDetail;
    private User user;
    ProfileDetailAdapter adapter;
    private String userId;
    private String currentUserId;
    private RestService service;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile_content, container, false);
        initView(v);
        prepareData();
        return v;
    }

    private void prepareData() {
        adapter = new ProfileDetailAdapter(getActivity(),new User());
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getActivity().getApplicationContext());
        lvProfileDetail.setLayoutManager(mLayoutManager);
        lvProfileDetail.setItemAnimator(new DefaultItemAnimator());
        lvProfileDetail.setAdapter(adapter);
    }

    private void initView(View v) {
        service = new RestService();
        currentUserId = DataUtils.getINSTANCE(getActivity()).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        userId = getArguments().getString("userId");
        lvProfileDetail = (RecyclerView) v.findViewById(R.id.lv_profile_detail);
    }

    @Override
    public void onResume() {
        super.onResume();
        if (user==null){
            loadData();
        }else{
            adapter.setUser(user);
        }
    }

    private void loadData() {
        service.getAccountService().getUserProfile(userId,currentUserId).enqueue(new Callback<ResponseModel<User>>() {
            @Override
            public void onResponse(Call<ResponseModel<User>> call, Response<ResponseModel<User>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        user = response.body().getData();
                        adapter.setUser(user);
                    }else{
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<User>> call, Throwable t) {
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }
}

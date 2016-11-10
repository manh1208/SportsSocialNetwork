package com.capstone.sportssocialnetwork.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.GroupSearchResultAdapter;
import com.capstone.sportssocialnetwork.listener.UpdateSearchFragment;
import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 11/10/16.
 */
public class GroupSearchFragment extends Fragment implements UpdateSearchFragment {

    private String query;
    private boolean isLoading;
    private RestService service;
    private ListView lvGroupSearchResult;
    private GroupSearchResultAdapter groupSearchResultAdapter;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_group_search, container, false);
        service = new RestService();
        lvGroupSearchResult = (ListView) v.findViewById(R.id.lv_group_search_result);
        groupSearchResultAdapter = new GroupSearchResultAdapter(getActivity(),R.layout.item_menu_profile,new ArrayList<Group>());
        lvGroupSearchResult.setAdapter(groupSearchResultAdapter);
        return v;
    }

    @Override
    public void update(String query) {
        if (this.query!=null && !this.query.toLowerCase().equals(query.toLowerCase())) {
            isLoading = false;
        }
        this.query = query;
        loadData();
    }

    private void loadData() {
        if (query != null && !isLoading) {
            isLoading = true;
            service.getGroupService().findGroup(query,0,20).enqueue(new Callback<ResponseModel<List<Group>>>() {
                @Override
                public void onResponse(Call<ResponseModel<List<Group>>> call, Response<ResponseModel<List<Group>>> response) {
                    isLoading=false;
                    if (response.isSuccessful()){
                        if (response.body().isSucceed()){
                            groupSearchResultAdapter.setGroups(response.body().getData());
                        }else{
                            Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                        }
                    }else{
                        Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<ResponseModel<List<Group>>> call, Throwable t) {
                    isLoading=false;
                    Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
                }
            });
        }
    }

    @Override
    public void onResume() {
        super.onResume();
    }
}

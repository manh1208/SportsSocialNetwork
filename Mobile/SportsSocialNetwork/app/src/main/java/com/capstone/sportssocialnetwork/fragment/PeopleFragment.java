package com.capstone.sportssocialnetwork.fragment;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.adapter.GroupSearchResultAdapter;
import com.capstone.sportssocialnetwork.adapter.PeopleSearchResultAdapter;
import com.capstone.sportssocialnetwork.listener.UpdateSearchFragment;
import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.model.User;
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
public class PeopleFragment extends Fragment implements UpdateSearchFragment {
    private String query;
    private boolean isLoading;
    private ListView lvPeopleSearchResult;
    private PeopleSearchResultAdapter adapter;
    private RestService service;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_people,container,false);
        service = new RestService();
        lvPeopleSearchResult = (ListView) v.findViewById(R.id.lv_people_search_result);
        adapter = new PeopleSearchResultAdapter(getActivity(),R.layout.item_friend,new ArrayList<User>());
        lvPeopleSearchResult.setAdapter(adapter);
        lvPeopleSearchResult.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                User user = adapter.getItem(position);
                Intent intent = new Intent(getActivity(), ProfileActivity.class);
                intent.putExtra("userId",user.getId());
                getActivity().startActivity(intent);
            }
        });
        return v;
    }

    @Override
    public void update(String query) {
        if (this.query!=null && !this.query.toLowerCase().equals(query.toLowerCase())) {
            isLoading = false;
        }
        this.query =query;
        loadData();
//        Toast.makeText(getActivity(), "Update: Mọi người: "+this.query, Toast.LENGTH_SHORT).show();
//        Log.e("PeopleFragment","Update: Mọi người: "+this.query);
    }

    private void loadData(){
        if (query!=null && !isLoading) {
            isLoading=true;
            service.getAccountService().findUser(query,0,20).enqueue(new Callback<ResponseModel<List<User>>>() {
                @Override
                public void onResponse(Call<ResponseModel<List<User>>> call, Response<ResponseModel<List<User>>> response) {
                    isLoading=false;
                    if (response.isSuccessful()){
                        if (response.body().isSucceed()){
                            adapter.setUsers(response.body().getData());
                        }else{
                            Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                        }
                    }else{
                        Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                    }
                }

                @Override
                public void onFailure(Call<ResponseModel<List<User>>> call, Throwable t) {
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

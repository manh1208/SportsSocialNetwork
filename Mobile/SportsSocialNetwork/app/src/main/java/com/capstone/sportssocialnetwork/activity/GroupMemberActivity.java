package com.capstone.sportssocialnetwork.activity;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.GroupMemberAdapter;
import com.capstone.sportssocialnetwork.model.GroupMember;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class GroupMemberActivity extends AppCompatActivity implements View.OnClickListener {
    private int groupId;
    private String groupName;
    private ListView lvGroupMember;
    private GroupMemberAdapter adapter;
    private RestService service;
    private FloatingActionButton fabAddMember;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_group_member);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        init();
        prepareData();
    }

    private void prepareData() {
        adapter = new GroupMemberAdapter(this,R.layout.item_group_member,new ArrayList<GroupMember>());
        lvGroupMember.setAdapter(adapter);
    }

    private void init() {
        groupId = getIntent().getIntExtra("groupId",-1);
        groupName = getIntent().getStringExtra("groupName");
        getSupportActionBar().setTitle(groupName+" - Thành viên");
        service =new RestService();
        lvGroupMember = (ListView) findViewById(R.id.lv_group_member);
        fabAddMember = (FloatingActionButton) findViewById(R.id.fab_group_add_member);
        fabAddMember.setOnClickListener(this);
    }

    private void loadData(){
        service.getGroupService().getGroupMember(groupId).enqueue(new Callback<ResponseModel<List<GroupMember>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<GroupMember>>> call, Response<ResponseModel<List<GroupMember>>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setGroupMembers(response.body().getData());
                    }else{
                        Toast.makeText(GroupMemberActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(GroupMemberActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<GroupMember>>> call, Throwable t) {
                Toast.makeText(GroupMemberActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }


    @Override
    protected void onResume() {
        super.onResume();
        loadData();
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        switch (id){
            case R.id.fab_group_add_member:
                Intent intent = new Intent(GroupMemberActivity.this,AddMemberActivity.class);
                intent.putExtra("groupId",groupId);
                startActivity(intent);
                break;
        }
    }
}

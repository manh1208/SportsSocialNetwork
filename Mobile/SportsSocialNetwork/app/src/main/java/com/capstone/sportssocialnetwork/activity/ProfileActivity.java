package com.capstone.sportssocialnetwork.activity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.Button;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.FeedAdapter;
import com.capstone.sportssocialnetwork.model.Feed;

import java.util.ArrayList;

public class ProfileActivity extends AppCompatActivity {
    private ListView lvTimeLine;
    private View header;
    private Button btnEditProfile;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        initView();
        prepareData();
        event();
    }

    private void prepareData() {


        lvTimeLine.addHeaderView(header);
        FeedAdapter adapter = new FeedAdapter(this, R.layout.item_feed, new ArrayList<Feed>());
        lvTimeLine.setAdapter(adapter);


    }

    private void initView() {
        lvTimeLine = (ListView) findViewById(R.id.lv_time_line);
        header = ((LayoutInflater) this
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                R.layout.item_profile_header, null, false);
        btnEditProfile = (Button) header.findViewById(R.id.btn_profile_detail);
    }

    private void event(){
        btnEditProfile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent=new Intent(getApplicationContext(),ProfileDetailActivity.class);
                startActivity(intent);
            }
        });
    }


}

package com.capstone.sportssocialnetwork.activity;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.PlaceManageAdapter;
import com.capstone.sportssocialnetwork.model.Place;

import java.util.ArrayList;
import java.util.List;

public class PlaceManageActivity extends AppCompatActivity {
    private ListView lvManagePlace;
    private PlaceManageAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_manage_place);
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
    }

    private void initView() {
        lvManagePlace = (ListView) findViewById(R.id.lv_manage_place);
    }

    private void prepareData() {
        adapter = new PlaceManageAdapter(this, R.layout.activity_manage_place, new ArrayList<Place>());
        lvManagePlace.setAdapter(adapter);
    }
}

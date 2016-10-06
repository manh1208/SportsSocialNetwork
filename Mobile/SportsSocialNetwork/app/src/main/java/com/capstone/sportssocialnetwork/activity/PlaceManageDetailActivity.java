package com.capstone.sportssocialnetwork.activity;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.support.v7.widget.ForwardingListener;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.FieldManageAdapter;
import com.capstone.sportssocialnetwork.adapter.PlaceFieldAdapter;
import com.capstone.sportssocialnetwork.model.Field;

import java.util.ArrayList;

public class PlaceManageDetailActivity extends AppCompatActivity {
    private ViewGroup placeDetailContainer;
    private ListView lvField;
    private FieldManageAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_place_manage_detail);
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

    private void initView(){
        placeDetailContainer = (ViewGroup) findViewById(R.id.place_detail_container);
        lvField = (ListView) findViewById(R.id.lv_field);
    }

    private void prepareData(){
        ArrayList<Field> fieldArrayList = new ArrayList<>();
        adapter = new FieldManageAdapter(this,R.layout.activity_place_manage_detail, fieldArrayList);
        lvField.setAdapter(adapter);
    }
}

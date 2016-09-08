package com.capstone.sportssocialnetwork.activity;

import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.View;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.NotificationAdapter;
import com.capstone.sportssocialnetwork.model.Notification;

import java.util.ArrayList;

public class NotificationActivity extends AppCompatActivity {
    private ListView lvNotification;
    private NotificationAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_notification);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Thông báo");
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

    private void prepareData() {
        adapter = new NotificationAdapter(this, R.layout.item_notification, new ArrayList<Notification>());
        lvNotification.setAdapter(adapter);
    }

    private void initView() {
        lvNotification = (ListView) findViewById(R.id.lv_notification);
    }


    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        return super.onPrepareOptionsMenu(menu);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_notification, menu);
        return super.onCreateOptionsMenu(menu);
    }
}

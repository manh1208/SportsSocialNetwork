package com.capstone.sportssocialnetwork.activity;

import android.content.Context;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.CommentAdapter;
import com.capstone.sportssocialnetwork.model.Comment;

import java.util.ArrayList;

public class PostDetailActivity extends AppCompatActivity {
    ListView lvComment;
    CommentAdapter adapter;
    View header;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_post_detail);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Chi tiết");
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

        adapter = new CommentAdapter(this,R.layout.item_comment,new ArrayList<Comment>());
        lvComment.addHeaderView(header);
        lvComment.setAdapter(adapter);
    }

    private void initView() {
        lvComment = (ListView) findViewById(R.id.lv_comment);
        header= ((LayoutInflater)
                getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                R.layout.item_feed, null, false);
    }

}

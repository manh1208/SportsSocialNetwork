package com.capstone.sportssocialnetwork.activity;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.ImageView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.PlacePageAdapter;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

public class ManagePlaceDetailActivity extends AppCompatActivity {
    private ViewPager viewPager;
    private TabLayout tabLayout;
    private FloatingActionButton fabBooking;
    private int placeId;
    private String placeName;
    private String placeAvatar;
    private ImageView ivCover;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_manage_place_detail);
        placeId = getIntent().getIntExtra("placeId",-1);
        placeName = getIntent().getStringExtra("placeName");
        placeAvatar = getIntent().getStringExtra("placeAvatar");
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        toolbar.setTitle(placeName);
        setSupportActionBar(toolbar);
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

//        getSupportActionBar().setTitle(placeName);
//        getSupportActionBar().setTitle("Chảo lửa");
        initView();
        prepareData();

    }

    private void prepareData() {
        PlacePageAdapter adapter = new PlacePageAdapter(getSupportFragmentManager(),placeId);
        viewPager.setAdapter(adapter);
        tabLayout.post(new Runnable() {
            @Override
            public void run() {
                tabLayout.setupWithViewPager(viewPager);
            }
        });
        Picasso.with(this).load(Uri.parse(DataUtils.URL+placeAvatar))
                .placeholder(R.drawable.img_default_avatar)
                .error(R.drawable.placeholder)
                .fit()
                .into(ivCover);

    }



    private void initView() {
        tabLayout = (TabLayout) findViewById(R.id.tabs_place_detail);
        viewPager = (ViewPager) findViewById(R.id.viewpager_place_detail);
        ivCover = (ImageView) findViewById(R.id.iv_detail_place_cover);
    }

    @Override
    protected void onResume() {
        super.onResume();
    }


}

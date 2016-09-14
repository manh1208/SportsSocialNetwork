package com.capstone.sportssocialnetwork.activity;

import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.content.ContextCompat;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.view.Window;
import android.view.WindowManager;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.ImageViewerAdapter;

public class ImageViewerActivity extends AppCompatActivity {
    private String[] mThumbIds;
    private ViewPager viewPager;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_image_viewer);
        setTitle(null);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        Window window = this.getWindow();
        window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS);
        window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
        window.setStatusBarColor(ContextCompat.getColor(this, R.color.black));
        int position = getIntent().getIntExtra("position", 0);
        mThumbIds = getIntent().getStringArrayExtra("listImage");
        viewPager = (ViewPager) findViewById(R.id.viewpager_default);
        viewPager.setAdapter(new ImageViewerAdapter(this, viewPager, mThumbIds));
        viewPager.setCurrentItem(position);

    }

}

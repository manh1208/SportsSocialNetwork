package com.capstone.sportssocialnetwork.activity;

import android.os.Bundle;
import android.os.PersistableBundle;
import android.support.annotation.IdRes;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.app.Fragment;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.fragment.BookingFragment;
import com.capstone.sportssocialnetwork.fragment.FeedFragment;
import com.capstone.sportssocialnetwork.fragment.ProfileFragment;
import com.roughike.bottombar.BottomBar;
import com.roughike.bottombar.OnMenuTabSelectedListener;

public class MainBottomBarActivity extends AppCompatActivity {
    private FrameLayout frame_main;
    private BottomBar bottomBar;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main_bottom_bar);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        getSupportActionBar().setTitle("Trang chủ");
        initView();
        prepareView();
        bottomBar = BottomBar.attach(this,savedInstanceState);
        bottomBar.setItemsFromMenu(R.menu.bottom_bar_menu, new OnMenuTabSelectedListener() {
            @Override
            public void onMenuItemSelected(@IdRes int menuItemId) {
                Fragment fragment = null;
                switch (menuItemId){
                    case R.id.nav_feed:
                        fragment = new FeedFragment();
                        getSupportActionBar().setTitle("Trang chủ");
                        break;
                    case R.id.nav_booking:
                        fragment = new BookingFragment();
                        getSupportActionBar().setTitle("Danh sách sân");
                        break;
                    case R.id.nav_profile:
                        fragment = new ProfileFragment();
                        getSupportActionBar().setTitle("Cá nhân");
                        break;
                }

                if (fragment!=null) {
                    getSupportFragmentManager()
                            .beginTransaction()
                            .replace(R.id.frame_menu, fragment)
                            .commit();
                }else{
                    getSupportFragmentManager()
                            .beginTransaction()
                            .replace(R.id.frame_menu, new FeedFragment())
                            .commit();
                }
            }
        });
    }

    private void prepareView() {
        getSupportFragmentManager()
                .beginTransaction()
                .replace(R.id.frame_menu, new FeedFragment())
                .commit();
    }

    @Override
    public void onSaveInstanceState(Bundle outState, PersistableBundle outPersistentState) {
        super.onSaveInstanceState(outState, outPersistentState);
        bottomBar.onSaveInstanceState(outState);
    }

    private void initView() {
        frame_main = (FrameLayout) findViewById(R.id.frame_menu);
    }


}

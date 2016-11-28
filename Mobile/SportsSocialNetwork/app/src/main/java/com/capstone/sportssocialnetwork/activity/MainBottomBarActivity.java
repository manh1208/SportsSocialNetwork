package com.capstone.sportssocialnetwork.activity;

import android.app.SearchManager;
import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.os.PersistableBundle;
import android.support.annotation.IdRes;
import android.support.v4.app.Fragment;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.SearchView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.FrameLayout;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.fragment.PlaceFragment;
import com.capstone.sportssocialnetwork.fragment.FeedFragment;
import com.capstone.sportssocialnetwork.fragment.ProfileFragment;
import com.capstone.sportssocialnetwork.fragment.SearchResultFragment;
import com.roughike.bottombar.BottomBar;
import com.roughike.bottombar.OnMenuTabSelectedListener;

public class MainBottomBarActivity extends AppCompatActivity {
    private FrameLayout frame_main;
    private BottomBar bottomBar;
    private SearchView searchView;
    private SearchView.OnQueryTextListener queryTextListener;
    private MenuItem searchItem;

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
                        fragment = new PlaceFragment();
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

//
//    @Override
//    public boolean onCreateOptionsMenu(Menu menu) {
////        getMenuInflater().inflate(R.menu.menu_main_bottom, menu);
//
////        searchItem = menu.findItem(R.id.menu_search);
////
////        SearchManager searchManager = (SearchManager) getSystemService(Context.SEARCH_SERVICE);
////        searchView = null;
////        if (searchItem != null) {
////            searchView = (SearchView) searchItem.getActionView();
////        }
////        if (searchView != null) {
////            searchView.setSearchableInfo(searchManager.getSearchableInfo(getComponentName()));
////
////            queryTextListener = new SearchView.OnQueryTextListener() {
////                @Override
////                public boolean onQueryTextChange(String newText) {
////                    Log.i("Main: onQueryTextChange", newText);
//////                    if (newText.length()<=0){
//////                        eventAdapter.setEventList(mEvents);
//////                        flag_loading =false;
//////                    }
////////                    doSearch(newText);
////                    return true;
////                }
////
////                @Override
////                public boolean onQueryTextSubmit(String query) {
////                    Log.i("Main onQueryTextSubmit", query);
//////                    doSearchAPI(query);
////                    getSupportFragmentManager()
////                            .beginTransaction()
////                            .replace(R.id.frame_menu, new SearchResultFragment())
////                            .commit();
////                    return true;
////                }
////            };
////            searchView.setOnQueryTextListener(queryTextListener);
////            searchView.onActionViewCollapsed();
////
////        }
//        return super.onCreateOptionsMenu(menu);
//    }

//    @Override
//    public boolean onOptionsItemSelected(MenuItem item) {
//        int id = item.getItemId();
//        //noinspection SimplifiableIfStatement
//        switch (id) {
//            case R.id.menu_search:
//                Intent intent =  new Intent(MainBottomBarActivity.this,SearchActivity.class);
//                startActivity(intent);
//                return true;
//        }
//        return super.onOptionsItemSelected(item);
//    }
}

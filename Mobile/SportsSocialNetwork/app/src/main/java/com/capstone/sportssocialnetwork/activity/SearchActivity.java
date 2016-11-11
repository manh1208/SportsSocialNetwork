package com.capstone.sportssocialnetwork.activity;

import android.app.SearchManager;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.SearchView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.PlacePageAdapter;
import com.capstone.sportssocialnetwork.adapter.SearchPageAdapter;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

public class SearchActivity extends AppCompatActivity {
    private MenuItem searchItem;
    private SearchView searchView;
    private SearchView.OnQueryTextListener queryTextListener;
    private ViewPager viewPager;
    private TabLayout tabLayout;
    private SearchPageAdapter adapter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_search);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setTitle("Tìm kiếm");
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        initView();
        prepareData();
    }

    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        searchItem = menu.findItem(R.id.menu_search_view);
        searchItem.expandActionView();
        SearchManager searchManager = (SearchManager) getSystemService(Context.SEARCH_SERVICE);
        searchView = null;
        if (searchItem != null) {
            searchView = (SearchView) searchItem.getActionView();
            searchView.setQueryHint("Nhập từ khóa");
        }
        if (searchView != null) {
            searchView.setSearchableInfo(searchManager.getSearchableInfo(getComponentName()));

            queryTextListener = new SearchView.OnQueryTextListener() {
                @Override
                public boolean onQueryTextChange(String newText) {
                    Log.i("Search:", newText);
//                    if (newText.length()<=0){
//                        eventAdapter.setEventList(mEvents);
//                        flag_loading =false;
//                    }
////                    doSearch(newText);
                    tabLayout.setVisibility(View.GONE);
                    viewPager.setVisibility(View.GONE);
                    return true;
                }

                @Override
                public boolean onQueryTextSubmit(String query) {
                    Log.i("Main onQueryTextSubmit", query);
//                    doSearchAPI(query);
                    tabLayout.setVisibility(View.VISIBLE);
                    viewPager.setVisibility(View.VISIBLE);
                    adapter.setQuery(query);
                    return true;
                }
            };
            searchView.setOnQueryTextListener(queryTextListener);

        }

        return super.onPrepareOptionsMenu(menu);
    }

    private void initView() {
        tabLayout = (TabLayout) findViewById(R.id.tabs_search);
        viewPager = (ViewPager) findViewById(R.id.viewpager_seach);
        tabLayout.setVisibility(View.GONE);
        viewPager.setVisibility(View.GONE);
    }

    private void prepareData() {
        adapter = new SearchPageAdapter(getSupportFragmentManager(),"");
        viewPager.setAdapter(adapter);
        tabLayout.post(new Runnable() {
            @Override
            public void run() {
                tabLayout.setupWithViewPager(viewPager);
            }
        });

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_search, menu);

//        searchView = (SearchView) searchItem.getActionView();
//        searchView.search
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public void onBackPressed() {
//        DataUtils.getINSTANCE(this).getPreferences().edit().clear().commit();
        Intent intent = new Intent(this, MainBottomBarActivity.class);
        intent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP);
        startActivity(intent);
        finish();
    }
}

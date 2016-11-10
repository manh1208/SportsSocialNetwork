package com.capstone.sportssocialnetwork.activity;

import android.os.Bundle;
import android.support.design.widget.TabLayout;
import android.support.v4.view.ViewPager;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.NewsPageAdapter;
import com.capstone.sportssocialnetwork.model.Category;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class NewsActivity extends AppCompatActivity {
    private ViewPager viewPager;
    private TabLayout tabLayout;
    private NewsPageAdapter adapter;
    private RestService service;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_news);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Tin tức thể thao");
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        initView();
        prepareData();
    }

    private void initView() {
        tabLayout = (TabLayout) findViewById(R.id.tabs_news);
        viewPager = (ViewPager) findViewById(R.id.viewpager_news);
        service = new RestService();
    }

    private void prepareData() {
        adapter = new NewsPageAdapter(getSupportFragmentManager());
        viewPager.setAdapter(adapter);
        tabLayout.post(new Runnable() {
            @Override
            public void run() {
                tabLayout.setupWithViewPager(viewPager);
            }
        });
    }

    private void loadCategory() {
        service.getNewsService().getAllCategory().enqueue(new Callback<ResponseModel<List<Category>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Category>>> call, Response<ResponseModel<List<Category>>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setCategories(response.body().getData());
                    }else{
                        Toast.makeText(NewsActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(NewsActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Category>>> call, Throwable t) {
                Toast.makeText(NewsActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    protected void onResume() {
        super.onResume();
        loadCategory();
    }
}

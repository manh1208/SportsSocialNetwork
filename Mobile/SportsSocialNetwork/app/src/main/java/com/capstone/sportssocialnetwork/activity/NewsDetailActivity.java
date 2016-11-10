package com.capstone.sportssocialnetwork.activity;

import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Html;
import android.view.View;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.News;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class NewsDetailActivity extends AppCompatActivity {
    private int newsId;
    private RestService service;
    private TextView txtTitle;
    private TextView txtAuthor;
    private TextView txtCategory;
    private TextView txtContent;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_news_detail);
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
        findView();
        service = new RestService();
        newsId = getIntent().getIntExtra("newsId", -1);
    }

    private void findView() {
        txtTitle = (TextView) findViewById(R.id.txt_news_detail_title);
        txtAuthor = (TextView) findViewById(R.id.txt_news_detail_author);
        txtCategory = (TextView) findViewById(R.id.txt_news_detail_category);
        txtContent = (TextView) findViewById(R.id.txt_news_detail_content);
    }

    private void updateUI(News news) {
        txtTitle.setText(news.getTitle());
        txtAuthor.setText(news.getAuthor()+"");
        txtCategory.setText(news.getCategoryName()+"");
        txtContent.setText(Html.fromHtml(news.getContent()));
    }

    private void loadNews() {
        service.getNewsService().getNewsDetail(newsId).enqueue(new Callback<ResponseModel<News>>() {
            @Override
            public void onResponse(Call<ResponseModel<News>> call, Response<ResponseModel<News>> response) {
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {

                        updateUI(response.body().getData());
                    } else {
                        Toast.makeText(NewsDetailActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(NewsDetailActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<News>> call, Throwable t) {
                Toast.makeText(NewsDetailActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    protected void onResume() {
        super.onResume();
        loadNews();
    }
}

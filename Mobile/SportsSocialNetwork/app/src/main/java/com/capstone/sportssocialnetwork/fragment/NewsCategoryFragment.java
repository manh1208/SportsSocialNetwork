package com.capstone.sportssocialnetwork.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.NewsActivity;
import com.capstone.sportssocialnetwork.adapter.NewsAdapter;
import com.capstone.sportssocialnetwork.model.News;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 11/10/16.
 */

public class NewsCategoryFragment extends Fragment {
    private ViewHolder viewHolder;
    private int categoryId;
    private RestService service;
    private NewsAdapter adapter;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_news_category,container,false);
        viewHolder = new ViewHolder(v);
        init();
        return v;
    }

    private void init() {
        categoryId = getArguments().getInt("categoryId",-1);
        service = new RestService();
        adapter = new NewsAdapter(getActivity(),R.layout.item_news,new ArrayList<News>());
        viewHolder.lvNews.setAdapter(adapter);
    }

    private void loadNews(){
        service.getNewsService().getNewsOfCategory(categoryId).enqueue(new Callback<ResponseModel<List<News>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<News>>> call, Response<ResponseModel<List<News>>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setNewses(response.body().getData());
                    }else{
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<News>>> call, Throwable t) {
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private final class ViewHolder{
        ListView lvNews;
        ViewHolder(View v){
            lvNews = (ListView) v.findViewById(R.id.lv_news);
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        loadNews();
    }
}

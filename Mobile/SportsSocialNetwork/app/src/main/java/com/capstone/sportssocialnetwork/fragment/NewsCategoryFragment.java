package com.capstone.sportssocialnetwork.fragment;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.NewsActivity;
import com.capstone.sportssocialnetwork.activity.NewsDetailActivity;
import com.capstone.sportssocialnetwork.adapter.NewsAdapter;
import com.capstone.sportssocialnetwork.model.Image;
import com.capstone.sportssocialnetwork.model.News;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

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
    private News hotNews;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_news_category,container,false);
        viewHolder = new ViewHolder(v);
        init();
        event();
        return v;
    }

    private void event() {
        viewHolder.lvNews.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Intent intent = new Intent(getActivity(), NewsDetailActivity.class);
                if (position==0) {
                    if (hotNews!=null) {
                        intent.putExtra("newsId", hotNews.getId());
                    }
                }else {
                    intent.putExtra("newsId", adapter.getItem(position-1).getId());
                }
                getActivity().startActivity(intent);
            }
        });
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
                        if (response.body().getData().size()>0){
                            List<News> newses = response.body().getData();
                            News news = newses.get(0);
                            newses.remove(0);
                            updateHeaderView(news);
                            adapter.setNewses(newses);
                        }else{
                            viewHolder.lvNews.removeHeaderView(viewHolder.header);
                        }

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

    private void updateHeaderView(News news){
        hotNews = news;
        viewHolder.txtTitle.setText(news.getTitle());
        Picasso.with(getActivity()).load(Uri.parse(DataUtils.URL + news.getImage()))
                .placeholder(R.drawable.placeholder)
                .error(R.drawable.ic_image_error)
                .noFade()
                .into(viewHolder.ivImage);
    }

    private final class ViewHolder{
        ListView lvNews;
        View header;
        ImageView ivImage;
        TextView txtTitle;

        ViewHolder(View v){
            lvNews = (ListView) v.findViewById(R.id.lv_news);
            header = getActivity().getLayoutInflater().inflate(R.layout.item_header_news,null,false);
            lvNews.addHeaderView(header);
            ivImage = (ImageView) header.findViewById(R.id.iv_news_image);
            txtTitle = (TextView) header.findViewById(R.id.txt_news_title);
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        loadNews();
    }
}

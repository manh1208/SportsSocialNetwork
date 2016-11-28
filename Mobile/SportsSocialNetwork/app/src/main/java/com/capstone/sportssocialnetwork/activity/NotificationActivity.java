package com.capstone.sportssocialnetwork.activity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.NotificationAdapter;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.Notification;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.ISocialNetworkService;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class NotificationActivity extends AppCompatActivity {
    private static final String TAG = "NotificationActivity";
    private SwipeRefreshLayout refreshLayout;
    private ListView lvNotification;
    private NotificationAdapter adapter;
    private RestService service;
    private String userId;
    private static final int MAX_TAKE = 20;
    private int skip;
    private int take;
    private boolean isFull;
    private boolean flag_loading;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_notification);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Thông báo");
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
        event();
    }

    private void event() {
        lvNotification.setOnScrollListener(new AbsListView.OnScrollListener() {
            @Override
            public void onScrollStateChanged(AbsListView view, int scrollState) {
            }

            @Override
            public void onScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
                if (firstVisibleItem + visibleItemCount >= totalItemCount-1 && totalItemCount >1) {
                    if (!flag_loading && !isFull) {
                        Log.i(TAG,"scroll");
                        loadData();
                    } else {
                        removeFooter();
                    }
                }
            }
        });

        lvNotification.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                final Notification notification = adapter.getItem(position);
                Call<ResponseModel<String>> call = service.getNotificationService().markAsRead(notification.getId());
                call.enqueue(new Callback<ResponseModel<String>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                        if (response.isSuccessful()){
                            if (response.body().isSucceed()){
                                notification.setRead(true);
                                adapter.notifyDataSetChanged();

                                if (notification.getPostId()!=null){
                                    Intent intent  = new Intent(NotificationActivity.this,PostDetailActivity.class);
                                    intent.putExtra("postId",notification.getPostId());
                                    startActivity(intent);
                                }
                                if (notification.getOrderId()!=null){
                                    Intent intent  = new Intent(NotificationActivity.this,MyOrderActivity.class);
                                    startActivity(intent);
                                }
                            }else{
                                Toast.makeText(NotificationActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(NotificationActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                        Toast.makeText(NotificationActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });


//                if (position>0) {
//                    Feed feed = adapter.getItem(position-1);
//                    Intent intent = new Intent(getActivity(), PostDetailActivity.class);
//                    intent.putExtra("postId", feed.getId());
//                    getActivity().startActivity(intent);
//                }
            }
        });

       refreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
//                Toast.makeText(mContext, "Refresh", Toast.LENGTH_SHORT).show();
                reloadAllFeed();
            }
        });
    }

    private void removeFooter() {
    }

    private void reloadAllFeed() {
        init();
//        if (viewHolder.lvFeed.getFooterViewsCount() <= 0)
//            viewHolder.lvFeed.addFooterView(viewHolder.footer);
//        adapter.notifyDataSetChanged();
        if (!flag_loading) {
            Log.i(TAG,"Resume");
            loadData();
        }
    }

    private void loadData() {
        flag_loading=true;
        Call<ResponseModel<List<Notification>>> call = service.getNotificationService().getNotifications(userId,skip,take);
        call.enqueue(new Callback<ResponseModel<List<Notification>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Notification>>> call, Response<ResponseModel<List<Notification>>> response) {
                if (refreshLayout.isRefreshing()) {
                    refreshLayout.setRefreshing(false);
                }
                flag_loading = false;
                if (response.isSuccessful()) {
                    ResponseModel<List<Notification>> responseModel = response.body();
                    if (responseModel.isSucceed()) {
                        if (responseModel.getData() != null && responseModel.getData().size() > 0) {
//                            Toast.makeText(getActivity(), "Load thành công", Toast.LENGTH_SHORT).show();
                            adapter.setAppendFeed(responseModel.getData());
                            if (adapter.getCount() < (skip + take)) {
                                isFull = true;
                                removeFooter();
                            }

                            skip = skip + take;
                        } else {
                            isFull = true;
                            removeFooter();
                        }
                    } else {
                        removeFooter();
                        Log.i(TAG, responseModel.getErrorsString());
                    }
                }

            }

            @Override
            public void onFailure(Call<ResponseModel<List<Notification>>> call, Throwable t) {
                if (refreshLayout.isRefreshing()) {
                    refreshLayout.setRefreshing(false);
                }
                flag_loading = false;
                Toast.makeText(NotificationActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void init() {
        skip = 0;
        take = MAX_TAKE;
        flag_loading = false;
        isFull = false;
        adapter.loadNew();
    }

    private void prepareData() {
        adapter = new NotificationAdapter(this, R.layout.item_notification, new ArrayList<Notification>());
        lvNotification.setAdapter(adapter);
    }

    private void initView() {
        service = new RestService();
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        lvNotification = (ListView) findViewById(R.id.lv_notification);
        refreshLayout = (SwipeRefreshLayout) findViewById(R.id.layout_refresh);
    }




    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        return super.onPrepareOptionsMenu(menu);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_notification, menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        switch (id){
            case R.id.menu_mark_all_read:
                service.getNotificationService().martAllAsRead(userId).enqueue(new Callback<ResponseModel<String>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                        if (response.body().isSucceed()){
                            reloadAllFeed();
                        }else{
                            Toast.makeText(NotificationActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                        Toast.makeText(NotificationActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });
                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    @Override
    protected void onResume() {
        super.onResume();
        reloadAllFeed();
    }
}

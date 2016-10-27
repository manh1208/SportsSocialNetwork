package com.capstone.sportssocialnetwork.fragment;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.PostActivity;
import com.capstone.sportssocialnetwork.activity.PostDetailActivity;
import com.capstone.sportssocialnetwork.adapter.PlaceDetailAdapter;
import com.capstone.sportssocialnetwork.adapter.ProfilePostAdapter;
import com.capstone.sportssocialnetwork.custom.DividerItemDecoration;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.ISocialNetworkService;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 10/3/16.
 */
public class ProfilePostFragment extends Fragment {
    private static final String TAG = "ProfileFragmentPost";
    private RestService service;
    private ViewHolder viewHolder;
    private ISocialNetworkService sSNService;
    private String userId;
    private static final int MAX_TAKE = 5;
    private int skip;
    private int take;
    private boolean isFull;
    private boolean flag_loading;
    private Context mContext;
    private ProfilePostAdapter adapter;
    private LinearLayoutManager mLayoutManager;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile_post, container, false);
        initView(v);
        prepareData();
        event();
        return v;
    }

    private void event() {
        viewHolder.btnPost.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(), PostActivity.class);
                startActivity(intent);
            }
        });

//        viewHolder.lvProfilePost.addOnScrollListener(new AbsListView.OnScrollListener() {
//            @Override
//            public void onScrollStateChanged(AbsListView view, int scrollState) {
//            }
//
//            @Override
//            public void onScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
//                if (firstVisibleItem + visibleItemCount >= totalItemCount-1 && totalItemCount >1) {
//                    if (!flag_loading && !isFull) {
//                        Log.i(TAG,"scroll");
//                        loadData();
//                    } else {
//                        removeFooter();
//                    }
//                }
//            }
//        });
        viewHolder.lvProfilePost.addOnScrollListener(new RecyclerView.OnScrollListener() {
            private boolean loading = true;
            int pastVisiblesItems, visibleItemCount, totalItemCount;
            @Override
            public void onScrollStateChanged(RecyclerView recyclerView, int newState) {
                super.onScrollStateChanged(recyclerView, newState);
            }

            @Override
            public void onScrolled(RecyclerView recyclerView, int dx, int dy) {
                super.onScrolled(recyclerView, dx, dy);
                if(dy > 0) //check for scroll down
                {
                    visibleItemCount = mLayoutManager.getChildCount();
                    totalItemCount = mLayoutManager.getItemCount();
                    pastVisiblesItems = mLayoutManager.findFirstVisibleItemPosition();

                        if ( (visibleItemCount + pastVisiblesItems) >= totalItemCount-1)
                        {

                            if (!flag_loading && !isFull) {
                                Log.i(TAG, "scroll");
                                loadData();
                            } else {
                                removeFooter();
                            }
                        }
                }
            }
        });

//        viewHolder.lvProfilePost.setOnItemClickListener(new AdapterView.OnItemClickListener() {
//            @Override
//            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
//                if (position>0) {
//                    Feed feed = adapter.getItem(position-1);
//                    Intent intent = new Intent(getActivity(), PostDetailActivity.class);
//                    intent.putExtra("postId", feed.getId());
//                    getActivity().startActivity(intent);
//                }
//            }
//        });

        viewHolder.layoutRefresh.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
//                Toast.makeText(mContext, "Refresh", Toast.LENGTH_SHORT).show();
                reloadAllFeed();
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

    private void loadData() {
        flag_loading = true;
        Call<ResponseModel<List<Feed>>> callback = service.getPostService().getUserPost(userId,userId, take, skip);
        callback.enqueue(new Callback<ResponseModel<List<Feed>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Feed>>> call, Response<ResponseModel<List<Feed>>> response) {
//                mFeeds = response.body().getData();
                if (viewHolder.layoutRefresh.isRefreshing()) {
                    viewHolder.layoutRefresh.setRefreshing(false);
                }
                flag_loading = false;
                if (response.isSuccessful()) {
                    ResponseModel<List<Feed>> responseModel = response.body();
                    if (responseModel.isSucceed()) {
                        if (responseModel.getData() != null && responseModel.getData().size() > 0) {
//                            Toast.makeText(getActivity(), "Load thành công", Toast.LENGTH_SHORT).show();
                            adapter.setAppendFeed(responseModel.getData());
                            if (adapter.getItemCount() < (skip + take)) {
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
            public void onFailure(Call<ResponseModel<List<Feed>>> call, Throwable t) {
                if (viewHolder.layoutRefresh.isRefreshing()) {
                    viewHolder.layoutRefresh.setRefreshing(false);
                }
                Toast.makeText(mContext, "Lỗi server", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void removeFooter() {
//        viewHolder.lvFeed.removeFooterView(viewHolder.footer);
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

    private final class ViewHolder {
        RecyclerView lvProfilePost;
        Button btnPost;
        View header;
        SwipeRefreshLayout layoutRefresh;
        View footer;

        ViewHolder(View v) {
            lvProfilePost = (RecyclerView) v.findViewById(R.id.lv_profile_post);
            header = ((LayoutInflater) getActivity()
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                    R.layout.item_header_feed, null, false);
            footer = ((LayoutInflater) getActivity()
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                    R.layout.item_load_more, null, false);

            btnPost = (Button) header.findViewById(R.id.btn_feed_post);
            layoutRefresh = (SwipeRefreshLayout) v.findViewById(R.id.layout_refresh);

        }

    }

    private void prepareData() {
        adapter = new ProfilePostAdapter(getActivity());
        mLayoutManager = new LinearLayoutManager(getActivity());
        viewHolder.lvProfilePost.addItemDecoration(new DividerItemDecoration(getActivity(),R.drawable.recycler_view_divider));
        viewHolder.lvProfilePost.setLayoutManager(mLayoutManager);
        viewHolder.lvProfilePost.setItemAnimator(new DefaultItemAnimator());
        viewHolder.lvProfilePost.setAdapter(adapter);
    }



    private void initView(View v) {
        userId = DataUtils.getINSTANCE(getActivity()).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        viewHolder = new ViewHolder(v);
        service = new RestService();
    }

    @Override
    public void onResume() {
        super.onResume();
        reloadAllFeed();
    }
}

package com.capstone.sportssocialnetwork.fragment;

import android.app.SearchManager;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.widget.SearchView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.NotificationActivity;
import com.capstone.sportssocialnetwork.activity.PostActivity;
import com.capstone.sportssocialnetwork.activity.PostDetailActivity;
import com.capstone.sportssocialnetwork.activity.SearchActivity;
import com.capstone.sportssocialnetwork.adapter.FeedAdapter;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.User;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.ISocialNetworkService;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/6/16.
 */
public class FeedFragment extends Fragment {
    private static final String TAG = "FeedFragment";
    private ViewHolder viewHolder;
    private FeedAdapter adapter;
    private SearchView searchView;
    private SearchView.OnQueryTextListener queryTextListener;
    private RestService service;
    private ISocialNetworkService sSNService;
    private String userId;
    private static final int MAX_TAKE = 5;
    private int skip;
    private int take;
    private boolean isFull;
    private boolean flag_loading;
    private Context mContext;


    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
        mContext = getActivity();
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_feed, container, false);
        initView(v);
        prepareData();
        event();
        getUser();
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

        viewHolder.lvFeed.setOnScrollListener(new AbsListView.OnScrollListener() {
            @Override
            public void onScrollStateChanged(AbsListView view, int scrollState) {
            }

            @Override
            public void onScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
                if (firstVisibleItem + visibleItemCount >= totalItemCount - 1 && totalItemCount > 1) {
                    if (!flag_loading && !isFull) {
                        Log.i(TAG, "scroll");
                        loadData();
                    } else {
                        removeFooter();
                    }
                }
            }
        });

        viewHolder.lvFeed.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                if (position > 0) {
                    Feed feed = adapter.getItem(position - 1);
                    Intent intent = new Intent(getActivity(), PostDetailActivity.class);
                    intent.putExtra("postId", feed.getId());
                    getActivity().startActivity(intent);
                }
            }
        });

        viewHolder.layoutRefresh.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
//                Toast.makeText(mContext, "Refresh", Toast.LENGTH_SHORT).show();
                reloadAllFeed();
            }
        });

    }

    private void prepareData() {
//        if (viewHolder.lvFeed.getFooterViewsCount() <= 0)
//            viewHolder.lvFeed.addFooterView(viewHolder.footer);
        viewHolder.lvFeed.addHeaderView(viewHolder.header);
        adapter = new FeedAdapter(getActivity(), R.layout.item_feed, new ArrayList<Feed>());
        viewHolder.lvFeed.setAdapter(adapter);


    }

    private void init() {
        skip = 0;
        take = MAX_TAKE;
        flag_loading = false;
        isFull = false;
        adapter.loadNew();
    }

    private void reloadAllFeed() {
        init();
//        if (viewHolder.lvFeed.getFooterViewsCount() <= 0)
//            viewHolder.lvFeed.addFooterView(viewHolder.footer);
//        adapter.notifyDataSetChanged();
        if (!flag_loading) {
            Log.i(TAG, "Resume");
            loadData();
        }
    }

    private void initView(View v) {
        viewHolder = new ViewHolder(v);
        service = new RestService();
        sSNService = service.getSocialNetworkService();
        userId = DataUtils.getINSTANCE(getActivity()).getPreferences()
                .getString(SharePreferentName.SHARE_USER_ID, "");

    }

    @Override
    public void onPrepareOptionsMenu(Menu menu) {
        super.onPrepareOptionsMenu(menu);
    }

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.menu_feed, menu);
//        MenuItem searchItem = menu.findItem(R.id.menu_search);
//        SearchManager searchManager = (SearchManager) getActivity().getSystemService(Context.SEARCH_SERVICE);
//        searchView = null;
//        if (searchItem != null) {
//            searchView = (SearchView) searchItem.getActionView();
//        }
//        if (searchView != null) {
//            searchView.setSearchableInfo(searchManager.getSearchableInfo(getActivity().getComponentName()));
//
//            queryTextListener = new SearchView.OnQueryTextListener() {
//                @Override
//                public boolean onQueryTextChange(String newText) {
//                    Log.i("onQueryTextChange", newText);
////                    if (newText.length()<=0){
////                        eventAdapter.setEventList(mEvents);
////                        flag_loading =false;
////                    }
//////                    doSearch(newText);
//                    return true;
//                }
//
//                @Override
//                public boolean onQueryTextSubmit(String query) {
//                    Log.i("onQueryTextSubmit", query);
////                    doSearchAPI(query);
//
//                    return true;
//                }
//            };
//            searchView.setOnQueryTextListener(queryTextListener);
//            searchView.onActionViewCollapsed();
//        }
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        //noinspection SimplifiableIfStatement
        switch (id) {
            case R.id.menu_notice:
                Intent intent = new Intent(getActivity(), NotificationActivity.class);
                startActivity(intent);
                return true;
            case R.id.menu_search:
                intent = new Intent(getActivity(), SearchActivity.class);
                startActivity(intent);
                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    private void loadData() {
        flag_loading = true;
        Call<ResponseModel<List<Feed>>> callback = service.getSocialNetworkService().getAllPost(userId, take, skip);
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
            public void onFailure(Call<ResponseModel<List<Feed>>> call, Throwable t) {
                if (viewHolder.layoutRefresh.isRefreshing()) {
                    viewHolder.layoutRefresh.setRefreshing(false);
                }
                Toast.makeText(mContext, "Lỗi server", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void getUser() {
        service.getAccountService().getUserProfile(userId, userId).enqueue(new Callback<ResponseModel<User>>() {
            @Override
            public void onResponse(Call<ResponseModel<User>> call, Response<ResponseModel<User>> response) {
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + response.body().getData().getAvatar()))
                                .placeholder(R.drawable.img_default_avatar)
                                .error(R.drawable.img_default_avatar_error)
                                .fit()
                                .into(viewHolder.ivPostAvatar);
                    } else {
                        Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<User>> call, Throwable t) {
                Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void removeFooter() {
//        viewHolder.lvFeed.removeFooterView(viewHolder.footer);
    }

    private final class ViewHolder {
        ListView lvFeed;
        LinearLayout btnPost;
        View header;
        SwipeRefreshLayout layoutRefresh;
        View footer;
        ImageView ivPostAvatar;

        ViewHolder(View v) {
            lvFeed = (ListView) v.findViewById(R.id.lv_list_feed);
            header = ((LayoutInflater) getActivity()
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                    R.layout.item_header_feed, null, false);
            footer = ((LayoutInflater) getActivity()
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                    R.layout.item_load_more, null, false);
            ivPostAvatar = (ImageView) header.findViewById(R.id.iv_post_avatar);
            btnPost = (LinearLayout) header.findViewById(R.id.btn_feed_post);
            layoutRefresh = (SwipeRefreshLayout) v.findViewById(R.id.layout_refresh);

        }

    }

    @Override
    public void onResume() {
        super.onResume();
        reloadAllFeed();
    }
}

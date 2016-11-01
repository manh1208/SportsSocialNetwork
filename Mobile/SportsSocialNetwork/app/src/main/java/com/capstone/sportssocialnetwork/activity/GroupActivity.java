package com.capstone.sportssocialnetwork.activity;

import android.app.SearchManager;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.app.ActionBar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.PopupMenu;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.SearchView;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.FeedAdapter;
import com.capstone.sportssocialnetwork.adapter.ProfilePostAdapter;
import com.capstone.sportssocialnetwork.custom.DividerItemDecoration;
import com.capstone.sportssocialnetwork.fragment.FeedFragment;
import com.capstone.sportssocialnetwork.fragment.ProfilePostFragment;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.Group;
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

public class GroupActivity extends AppCompatActivity implements View.OnClickListener{
    private static final String TAG = "Group Activity";
    private ViewHolder viewHolder;
    private ProfilePostAdapter adapter;
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
    private int groupId;
    private LinearLayoutManager mLayoutManager;
    private Toolbar toolbar;
    private ActionBar actionBar;
    private String groupName;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_group);
        toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        actionBar =getSupportActionBar();
        mContext = this;
        initView();
        prepareData();
        event();
        getUser();
        getGroupProfile();
    }


    private void event() {
        viewHolder.btnPost.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(GroupActivity.this, PostActivity.class);
                intent.putExtra("groupId",groupId+"");
                startActivity(intent);
            }
        });

        viewHolder.lvGroupPost.addOnScrollListener(new RecyclerView.OnScrollListener() {
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

        viewHolder.layoutRefresh.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
//                Toast.makeText(mContext, "Refresh", Toast.LENGTH_SHORT).show();
                reloadAllGroupPost();
            }
        });

    }

    private void prepareData() {
//        if (viewHolder.lvFeed.getFooterViewsCount() <= 0)
//            viewHolder.lvFeed.addFooterView(viewHolder.footer);
//        viewHolder.lvGroupPost.addHeaderView(viewHolder.header);
        adapter = new ProfilePostAdapter(this);
        mLayoutManager = new LinearLayoutManager(this);
        viewHolder.lvGroupPost.addItemDecoration(new DividerItemDecoration(this,R.drawable.recycler_view_divider));
        viewHolder.lvGroupPost.setLayoutManager(mLayoutManager);
        viewHolder.lvGroupPost.setItemAnimator(new DefaultItemAnimator());
        viewHolder.lvGroupPost.setAdapter(adapter);
        viewHolder.layoutJoin.setOnClickListener(this);
        viewHolder.layoutMember.setOnClickListener(this);
        viewHolder.layoutInfomation.setOnClickListener(this);

    }

    private void init() {
        skip = 0;
        take = MAX_TAKE;
        flag_loading = false;
        isFull = false;
        adapter.loadNew();
    }

    private void reloadAllGroupPost() {
        init();
//        if (viewHolder.lvFeed.getFooterViewsCount() <= 0)
//            viewHolder.lvFeed.addFooterView(viewHolder.footer);
//        adapter.notifyDataSetChanged();
        if (!flag_loading) {
            Log.i(TAG,"Resume");
            loadData();
        }
    }

    private void initView() {
        viewHolder = new ViewHolder();
        service = new RestService();
        sSNService = service.getSocialNetworkService();
        userId = DataUtils.getINSTANCE(this).getPreferences()
                .getString(SharePreferentName.SHARE_USER_ID, "");
        groupId= getIntent().getIntExtra("groupId",-1);
        groupName = getIntent().getStringExtra("groupName");
        getSupportActionBar().setTitle(groupName);
    }


    private void loadData() {
        flag_loading = true;
        Call<ResponseModel<List<Feed>>> callback = service.getGroupService().getGroupPost(groupId,userId, skip, take);
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

    private void getUser(){
        service.getAccountService().getUserProfile(userId).enqueue(new Callback<ResponseModel<User>>() {
            @Override
            public void onResponse(Call<ResponseModel<User>> call, Response<ResponseModel<User>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + response.body().getData().getAvatar()))
                                .placeholder(R.drawable.img_default_avatar)
                                .error(R.drawable.img_default_avatar_error)
                                .fit()
                                .into(viewHolder.ivPostAvatar);
                    }else{
                        Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<User>> call, Throwable t) {
                Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void getGroupProfile(){
        service.getGroupService().getGroupDetail(groupId,userId).enqueue(new Callback<ResponseModel<Group>>() {
            @Override
            public void onResponse(Call<ResponseModel<Group>> call, Response<ResponseModel<Group>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        actionBar.setTitle(response.body().getData().getName());
                        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + response.body().getData().getCoverImage()))
                                .placeholder(R.drawable.img_default_avatar)
                                .error(R.drawable.img_default_avatar_error)
                                .fit()
                                .into(viewHolder.ivGroupCover);
                    }else{
                        Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<Group>> call, Throwable t) {
                Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void removeFooter() {
//        viewHolder.lvFeed.removeFooterView(viewHolder.footer);
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        switch (id){
            case R.id.layout_group_join:
                showPopupMenu(v);
                break;
            case R.id.layout_group_member:
                Intent intent = new Intent(GroupActivity.this,GroupMemberActivity.class);
                intent.putExtra("groupId",groupId);
                intent.putExtra("groupName",groupName);
                startActivity(intent);
                break;
            case R.id.layout_group_information:
                intent = new Intent(GroupActivity.this,GroupInformationActivity.class);
                intent.putExtra("groupId",groupId);
                intent.putExtra("groupName",groupName);
                startActivity(intent);
                break;
        }

    }

    public void showPopupMenu(View v) {
        PopupMenu popupMenu = new PopupMenu(mContext, v);
        final MenuInflater inflater = popupMenu.getMenuInflater();
        inflater.inflate(R.menu.menu_group_popup, popupMenu.getMenu());
        popupMenu.show();
        popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
            @Override
            public boolean onMenuItemClick(MenuItem item) {
                int id = item.getItemId();
                switch (id) {
                    case R.id.menu_group_leave_group:
                        Toast.makeText(mContext, "Rời khỏi nhóm", Toast.LENGTH_SHORT).show();
                        return true;

                }
                return false;
            }
        });
    }

    private final class ViewHolder {
        RecyclerView lvGroupPost;
        LinearLayout btnPost;
        SwipeRefreshLayout layoutRefresh;
        View footer;
        ImageView ivPostAvatar;
        ImageView ivGroupCover;
        LinearLayout layoutJoin;
        LinearLayout layoutMember;
        LinearLayout layoutInfomation;

        ViewHolder() {
            lvGroupPost = (RecyclerView) findViewById(R.id.lv_group_post);
            footer = ((LayoutInflater)
                    getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                    R.layout.item_load_more, null, false);
            ivPostAvatar = (ImageView)findViewById(R.id.iv_group_post_avatar);
            btnPost = (LinearLayout) findViewById(R.id.btn_group_post);
            layoutRefresh = (SwipeRefreshLayout) findViewById(R.id.layout_refresh);
            ivGroupCover = (ImageView) findViewById(R.id.iv_group_cover);
            layoutJoin = (LinearLayout) findViewById(R.id.layout_group_join);
            layoutMember = (LinearLayout) findViewById(R.id.layout_group_member);
            layoutInfomation = (LinearLayout) findViewById(R.id.layout_group_information);

        }

    }

    @Override
    public void onResume() {
        super.onResume();
        reloadAllGroupPost();
    }

}

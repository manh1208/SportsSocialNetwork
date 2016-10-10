package com.capstone.sportssocialnetwork.fragment;

import android.app.SearchManager;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.SearchView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.NotificationActivity;
import com.capstone.sportssocialnetwork.activity.PostActivity;
import com.capstone.sportssocialnetwork.adapter.FeedAdapter;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.ResponseModel;
import com.capstone.sportssocialnetwork.service.ISocialNetworkService;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/6/16.
 */
public class FeedFragment extends Fragment {
    private ListView lvFeed;
    private FeedAdapter adapter;
    private Button btnPost;
    private View header;
    private SearchView searchView;
    private SearchView.OnQueryTextListener queryTextListener;
    private RestService service;
    private ISocialNetworkService sSNService;
    private String userId;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_feed, container, false);
        initView(v);
        prepareData();
        event();
        reloadData();
        return v;
    }



    private void event() {
        btnPost.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(), PostActivity.class);
                startActivity(intent);
            }
        });

    }

    private void prepareData() {


        lvFeed.addHeaderView(header);
        adapter = new FeedAdapter(getActivity(), R.layout.item_feed, new ArrayList<Feed>());
        lvFeed.setAdapter(adapter);


    }

    private void initView(View v) {
        service = new RestService();
        sSNService = service.getSocialNetworkService();
        userId = DataUtils.getINSTANCE(getActivity()).getPreferences()
                .getString(SharePreferentName.SHARE_USER_ID,"");
        lvFeed = (ListView) v.findViewById(R.id.lv_list_feed);
        header = ((LayoutInflater) getActivity()
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                R.layout.item_header_feed, null, false);
        btnPost = (Button) header.findViewById(R.id.btn_feed_post);
    }

    @Override
    public void onPrepareOptionsMenu(Menu menu) {
        super.onPrepareOptionsMenu(menu);
    }

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.menu_feed, menu);
        MenuItem searchItem = menu.findItem(R.id.menu_search);
        SearchManager searchManager = (SearchManager) getActivity().getSystemService(Context.SEARCH_SERVICE);
        searchView=null;
        if (searchItem != null) {
            searchView = (SearchView) searchItem.getActionView();
        }
        if (searchView != null) {
            searchView.setSearchableInfo(searchManager.getSearchableInfo(getActivity().getComponentName()));

            queryTextListener = new SearchView.OnQueryTextListener() {
                @Override
                public boolean onQueryTextChange(String newText) {
                    Log.i("onQueryTextChange", newText);
//                    if (newText.length()<=0){
//                        eventAdapter.setEventList(mEvents);
//                        flag_loading =false;
//                    }
////                    doSearch(newText);
                    return true;
                }

                @Override
                public boolean onQueryTextSubmit(String query) {
                    Log.i("onQueryTextSubmit", query);
//                    doSearchAPI(query);
                    return true;
                }
            };
            searchView.setOnQueryTextListener(queryTextListener);
            searchView.onActionViewCollapsed();
        }
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
        }
        return super.onOptionsItemSelected(item);
    }

    private void reloadData() {
        Call<ResponseModel<List<Feed>>> callback = sSNService.getAllPost(userId);
        callback.enqueue(new Callback<ResponseModel<List<Feed>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Feed>>> call, Response<ResponseModel<List<Feed>>> response) {
                if (response.isSuccessful()){
                    ResponseModel<List<Feed>> responseModel = response.body();
                    if (responseModel.isSucceed()){
                        adapter.setFeeds(responseModel.getData());
                    }else{

                    }
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Feed>>> call, Throwable t) {

            }
        });
    }
}

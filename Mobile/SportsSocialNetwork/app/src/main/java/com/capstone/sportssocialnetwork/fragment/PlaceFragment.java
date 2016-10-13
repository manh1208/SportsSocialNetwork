package com.capstone.sportssocialnetwork.fragment;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.app.AlertDialog;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AbsListView;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.PlaceDetailActivity;
import com.capstone.sportssocialnetwork.adapter.PlaceAdapter;
import com.capstone.sportssocialnetwork.model.Place;
import com.capstone.sportssocialnetwork.model.response.PlaceResponseModel;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.ISocialNetworkService;
import com.capstone.sportssocialnetwork.service.RestService;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/6/16.
 */
public class PlaceFragment extends Fragment {

    private static final String TAG = "PlaceFragment";
    private PlaceAdapter adapter;
    private RestService service;
    private ISocialNetworkService sSNService;
    private String userId;
    private static final int MAX_TAKE = 5;
    private int skip;
    private int take;
    private boolean isFull;
    private boolean flag_loading;
    private ViewHolder viewHolder;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_place, container, false);
        initView(v);
        prepareData();
        event();
        return v;
    }

    private void event() {
        viewHolder.lvPlace.setOnScrollListener(new AbsListView.OnScrollListener() {
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
//                        removeFooter();
                    }
                }
            }
        });

        viewHolder.refreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
                reloadAllPlaces();
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

    private void reloadAllPlaces() {
        init();
        if (!flag_loading) {
            Log.i(TAG, "Resume");
            loadData();
        }
    }

    private void loadData() {
        flag_loading = true;
        Call<ResponseModel<List<PlaceResponseModel>>> callback = service.getPlaceService().getAllPlace(skip, take);
        callback.enqueue(new Callback<ResponseModel<List<PlaceResponseModel>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<PlaceResponseModel>>> call, Response<ResponseModel<List<PlaceResponseModel>>> response) {
                if (viewHolder.refreshLayout.isRefreshing()) {
                    viewHolder.refreshLayout.setRefreshing(false);
                }
                flag_loading = false;
                if (response.isSuccessful()) {
                    ResponseModel<List<PlaceResponseModel>> responseModel = response.body();
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
            public void onFailure(Call<ResponseModel<List<PlaceResponseModel>>> call, Throwable t) {
                if (viewHolder.refreshLayout.isRefreshing()) {
                    viewHolder.refreshLayout.setRefreshing(false);
                }
                Toast.makeText(getActivity(), t.getMessage(), Toast.LENGTH_SHORT).show();
            }
        });

    }

    private void removeFooter() {
    }

    private void prepareData() {

        adapter = new PlaceAdapter(getActivity(), R.layout.item_feed, new ArrayList<PlaceResponseModel>());
        viewHolder.lvPlace.setAdapter(adapter);
        viewHolder.lvPlace.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Intent intent = new Intent(getActivity(), PlaceDetailActivity.class);
                intent.putExtra("placeId",adapter.getItem(position).getId());
                intent.putExtra("placeName",adapter.getItem(position).getName());
                startActivity(intent);
            }
        });
    }

    private void initView(View v) {
        viewHolder = new ViewHolder(v);
        service = new RestService();
    }

    @Override
    public void onPrepareOptionsMenu(Menu menu) {
        super.onPrepareOptionsMenu(menu);
    }

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.menu_booking, menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        switch (id) {
            case R.id.menu_filter:
                AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
                View v= getActivity().getLayoutInflater().inflate(R.layout.dialog_place_filter,null,false);
                builder.setView(v)
                        .setPositiveButton("Lọc", new DialogInterface.OnClickListener() {
                            @Override
                            public void onClick(DialogInterface dialog, int which) {

                            }
                        })
                        .setNegativeButton("Tìm sân quanh đây",null)
                        .create().show();
                break;
        }
        return super.onOptionsItemSelected(item);
    }

    private final class ViewHolder {
        ListView lvPlace;
        SwipeRefreshLayout refreshLayout;

        ViewHolder(View v) {
            lvPlace = (ListView) v.findViewById(R.id.lv_list_place);
            refreshLayout = (SwipeRefreshLayout) v.findViewById(R.id.layout_refresh);
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        reloadAllPlaces();
    }
}

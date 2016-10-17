package com.capstone.sportssocialnetwork.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.ManageOrderAdapter;
import com.capstone.sportssocialnetwork.model.Order;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 10/14/16.
 */
public class ManageOrderFragment extends Fragment {
    private ViewHolder viewHolder;
    private RestService service;
    private ManageOrderAdapter adapter;
    private String userId;
    private boolean isLoading;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_manage_order, container, false);
        init(v);
        prepareData();
        event();

        return v;
    }

    private void event() {
        viewHolder.refreshLayout.setOnRefreshListener(new SwipeRefreshLayout.OnRefreshListener() {
            @Override
            public void onRefresh() {
//                Toast.makeText(mContext, "Refresh", Toast.LENGTH_SHORT).show();
                if (!isLoading) {
                    loadData();
                }
            }
        });
    }

    private void prepareData() {
        viewHolder.lvOrder.setAdapter(adapter);
    }

    private void init(View v) {
        userId = DataUtils.getINSTANCE(getActivity()).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
        viewHolder = new ViewHolder(v);
        service = new RestService();
        adapter = new ManageOrderAdapter(getActivity(), R.layout.item_manage_order, new ArrayList<Order>());
        isLoading = false;
    }

    private final class ViewHolder {
        SwipeRefreshLayout refreshLayout;
        ListView lvOrder;

        ViewHolder(View v) {
            refreshLayout = (SwipeRefreshLayout) v.findViewById(R.id.layout_refresh);
            lvOrder = (ListView) v.findViewById(R.id.lv_manage_order);
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        loadData();
    }

    private void loadData() {
        isLoading = true;
        Call<ResponseModel<List<Order>>> call = service.getOrderService().getPlaceOwnerOrder(userId);
        call.enqueue(new Callback<ResponseModel<List<Order>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Order>>> call, Response<ResponseModel<List<Order>>> response) {
                isLoading = false;
                if (viewHolder.refreshLayout.isRefreshing()) {
                    viewHolder.refreshLayout.setRefreshing(false);
                }
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        adapter.setOrders(response.body().getData());
                    } else {
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Order>>> call, Throwable t) {
                if (viewHolder.refreshLayout.isRefreshing()) {
                    viewHolder.refreshLayout.setRefreshing(false);
                }
                Toast.makeText(getActivity(), "Loi ket noi server", Toast.LENGTH_SHORT).show();
            }
        });
    }
}

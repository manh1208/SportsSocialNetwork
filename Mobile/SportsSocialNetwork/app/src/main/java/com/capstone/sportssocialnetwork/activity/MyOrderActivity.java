package com.capstone.sportssocialnetwork.activity;

import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.enumerable.OrderStatusEnum;
import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.MyOrderAdapter;
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

public class MyOrderActivity extends AppCompatActivity {
    private ListView lvMyOrder;
    private MyOrderAdapter adapter;
    private RestService service;
    private String userId;
    private Spinner spFilter;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_my_order);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Đơn đặt sân của tôi");
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        initView();
        createSpinner();
    }

    private void initView() {
        lvMyOrder = (ListView) findViewById(R.id.lv_my_order);
        spFilter = (Spinner) findViewById(R.id.spinner_manage_order);
        adapter = new MyOrderAdapter(this, R.layout.item_order_history, new ArrayList<Order>());
        lvMyOrder.setAdapter(adapter);
        service = new RestService();
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
        lvMyOrder.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View v, int position, long id) {
                Order order = adapter.getItem(position);
                adapter.showDialog(order);
            }
        });
    }

    private void createSpinner() {
        List<String> sports = new ArrayList<String>();
        sports.add("Tất cả");
        for (int i = 0; i < OrderStatusEnum.values().length; i++) {
            sports.add(OrderStatusEnum.values()[i].toString());
        }

        ArrayAdapter<String> arrayAdapter = new ArrayAdapter(this, R.layout.item_spinner, sports);
        arrayAdapter.setDropDownViewResource(R.layout.item_spinner);
        spFilter.setAdapter(arrayAdapter);
        spFilter.setSelection(0);
        spFilter.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                adapter.filter(position);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
    }


    @Override
    protected void onResume() {
        super.onResume();
        loadData();
    }

    private void loadData() {
        Call<ResponseModel<List<Order>>> call = service.getOrderService().getMyOrder(userId);
        call.enqueue(new Callback<ResponseModel<List<Order>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Order>>> call, Response<ResponseModel<List<Order>>> response) {
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        adapter.setOrders(response.body().getData());
                    } else {
                        Toast.makeText(MyOrderActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(MyOrderActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Order>>> call, Throwable t) {
                Toast.makeText(MyOrderActivity.this, "Loi ket noi voi server", Toast.LENGTH_SHORT).show();
            }
        });
    }
}

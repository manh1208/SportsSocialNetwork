package com.capstone.sportssocialnetwork.activity;

import android.net.Uri;
import android.os.Bundle;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.MyOrderAdapter;
import com.capstone.sportssocialnetwork.model.Order;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.squareup.picasso.Picasso;

import java.text.ParseException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class MyOrderActivity extends AppCompatActivity {
    private ListView lvMyOrder;
    private MyOrderAdapter adapter;
    private RestService service;
    private String userId;

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
    }

    private void initView() {
        lvMyOrder= (ListView) findViewById(R.id.lv_my_order);
        adapter = new MyOrderAdapter(this,R.layout.item_order_history,new ArrayList<Order>());
        lvMyOrder.setAdapter(adapter);
        service  = new RestService();
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        lvMyOrder.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View v, int position, long id) {
                Order order = adapter.getItem(position);
                adapter.showDialog(order);
//                AlertDialog.Builder buider=  new AlertDialog.Builder(MyOrderActivity.this);
//                View view = LayoutInflater.from(MyOrderActivity.this).inflate(R.layout.dialog_my_order_detail,null,false);
//                ImageView ivQR = (ImageView) view.findViewById(R.id.iv_qr_code);
//
//                Picasso.with(MyOrderActivity.this).load(Uri.parse(DataUtils.URL+order.getqRCodeUrl()))
//                        .placeholder(R.drawable.image_logo)
//                        .error(R.drawable.img_default_avatar)
//                        .into(ivQR);
//                TextView useDate = (TextView) view.findViewById(R.id.txt_order_detail_use_date);
//                try {
//                    Date date = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
//                    useDate.setText( Utilities.getDateTimeString(date,"dd/MM/yyyy"));
//                } catch (ParseException e) {
//                    Toast.makeText(MyOrderActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
//                }
//
//                TextView place = (TextView) view.findViewById(R.id.txt_order_detail_place);
//                place.setText(order.getPlaceName());
//                TextView field = (TextView) view.findViewById(R.id.txt_order_detail_field);
//                field.setText(order.getFieldName());
//                TextView startTime = (TextView) view.findViewById(R.id.txt_order_detail_start_time);
//                try {
//                    Date date = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
//                    startTime.setText( Utilities.getDateTimeString(date,"hh:mm a"));
//                } catch (ParseException e) {
//                    Toast.makeText(MyOrderActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
//                }
//
//
//                TextView endTime = (TextView) view.findViewById(R.id.txt_order_detail_end_time);
//                try {
//                    Date date = Utilities.getDateTime(order.getEndTime(),"MM/dd/yyyy hh:mm:ss a");
//                    endTime.setText( Utilities.getDateTimeString(date,"hh:mm a"));
//                } catch (ParseException e) {
//                    Toast.makeText(MyOrderActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
//                }
//
//                TextView payment = (TextView) view.findViewById(R.id.txt_order_detail_payment);
//                payment.setText(order.getPaidTypeString());
//                TextView status = (TextView) view.findViewById(R.id.txt_order_detail_order_status);
//                status.setText(order.getStatus());
//                buider.setView(view)
//                        .setNegativeButton("OK",null).create().show();
            }
        });
    }


    @Override
    protected void onResume() {
        super.onResume();
        loadData();
    }

    private void loadData() {
        Call<ResponseModel<List<Order>>> call =  service.getOrderService().getMyOrder(userId);
        call.enqueue(new Callback<ResponseModel<List<Order>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Order>>> call, Response<ResponseModel<List<Order>>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setOrders(response.body().getData());
                    }else{
                        Toast.makeText(MyOrderActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
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

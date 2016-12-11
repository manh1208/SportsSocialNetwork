package com.capstone.sportssocialnetwork.fragment;

import android.content.Context;
import android.content.DialogInterface;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.enumerable.OrderStatusEnum;
import com.capstone.sportssocialnetwork.enumerable.PaidTypeEnum;
import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.ManageOrderAdapter;
import com.capstone.sportssocialnetwork.model.Order;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.text.ParseException;
import java.util.ArrayList;
import java.util.Date;
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
    private Context mContext;

    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);

    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_manage_order, container, false);
        init(v);
        createSpinner();
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
        viewHolder.spFilter.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                adapter.filter(position);
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
    }

    private void prepareData() {
        viewHolder.lvOrder.setAdapter(adapter);
    }

    private void init(View v) {
        mContext = getActivity();
        userId = DataUtils.getINSTANCE(getActivity()).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
        viewHolder = new ViewHolder(v);
        service = new RestService();
        adapter = new ManageOrderAdapter(getActivity(), R.layout.item_order_manage, new ArrayList<Order>());
        isLoading = false;
        viewHolder.lvOrder.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Order order = adapter.getItem(position);
               showDialog(order);
            }
        });
    }

    private void createSpinner() {
        List<String> sports = new ArrayList<String>();
        sports.add("Tất cả");
        for (int i = 0; i < OrderStatusEnum.values().length; i++) {
            sports.add(OrderStatusEnum.values()[i].toString());
        }

        ArrayAdapter<String> arrayAdapter = new ArrayAdapter(getActivity(), R.layout.item_spinner, sports);
        arrayAdapter.setDropDownViewResource(R.layout.item_spinner);
        viewHolder.spFilter.setAdapter(arrayAdapter);
        viewHolder.spFilter.setSelection(0);
    }

    private final class ViewHolder {
        Spinner spFilter;
        SwipeRefreshLayout refreshLayout;
        ListView lvOrder;

        ViewHolder(View v) {
            refreshLayout = (SwipeRefreshLayout) v.findViewById(R.id.layout_refresh);
            lvOrder = (ListView) v.findViewById(R.id.lv_manage_order);
            spFilter = (Spinner) v.findViewById(R.id.spinner_manage_order);
        }
    }

    @Override
    public void onResume() {
        super.onResume();
        loadData();
    }

    public void showDialog(final Order order) {
        AlertDialog.Builder buider = new AlertDialog.Builder(mContext);
        View view = LayoutInflater.from(mContext).inflate(R.layout.dialog_manage_order_detail, null, false);
        final AlertDialog dialog =  buider.setView(view)
                .setNegativeButton("OK", null).create();


        TextView name = (TextView) view.findViewById(R.id.txt_order_detail_fullname);
        name.setText(order.getFullName());
//                Toast.makeText(mContext, (DataUtils.URL+order.getqRCodeUrl()), Toast.LENGTH_SHORT).show();

        TextView createDate = (TextView) view.findViewById(R.id.txt_order_detail_create_time);
        createDate.setText(order.getCreateDate());
        TextView useDate = (TextView) view.findViewById(R.id.txt_order_detail_use_date);
        try {
            Date date = Utilities.getDateTime(order.getStartTime(), DataUtils.FORMAT_DATE_TIME);
            useDate.setText(Utilities.getDateTimeString(date, DataUtils.FORMAT_DATE));
        } catch (ParseException e) {
            Toast.makeText(mContext, R.string.parse_exception, Toast.LENGTH_SHORT).show();
        }

        TextView place = (TextView) view.findViewById(R.id.txt_order_detail_place);
        place.setText(order.getPlaceName());
        TextView field = (TextView) view.findViewById(R.id.txt_order_detail_field);
        field.setText(order.getFieldName());
        TextView startTime = (TextView) view.findViewById(R.id.txt_order_detail_start_time);
        try {
            Date date = Utilities.getDateTime(order.getStartTime(), DataUtils.FORMAT_DATE_TIME);
            startTime.setText(Utilities.getDateTimeString(date, DataUtils.FORMAT_TIME));
        } catch (ParseException e) {
            Toast.makeText(mContext, R.string.parse_exception, Toast.LENGTH_SHORT).show();
        }


        TextView endTime = (TextView) view.findViewById(R.id.txt_order_detail_end_time);
        try {
            Date date = Utilities.getDateTime(order.getEndTime(), DataUtils.FORMAT_DATE_TIME);
            endTime.setText(Utilities.getDateTimeString(date, DataUtils.FORMAT_TIME));
        } catch (ParseException e) {
            Toast.makeText(mContext, R.string.parse_exception, Toast.LENGTH_SHORT).show();
        }

        TextView price = (TextView) view.findViewById(R.id.txt_order_detail_price);
        price.setText(order.getPrice().longValue()+"");

        TextView payment = (TextView) view.findViewById(R.id.txt_order_detail_payment);
        payment.setText(PaidTypeEnum.fromInteger(order.getPaidType()).toString());
        TextView status = (TextView) view.findViewById(R.id.txt_order_detail_order_status);
        status.setText(OrderStatusEnum.fromInteger(order.getStatus()).toString());
        Button btnApprove = (Button) view.findViewById(R.id.btn_maange_order_approve);
        Button btnUnapprove = (Button) view.findViewById(R.id.btn_manage_order_unapprove);
        Button btnPayment = (Button) view.findViewById(R.id.btn_manage_order_payment);
        if (order.getStatus()==OrderStatusEnum.Pending.getValue()){
            btnApprove.setVisibility(View.VISIBLE);
            btnUnapprove.setVisibility(View.VISIBLE);
        }else{
            btnApprove.setVisibility(View.GONE);
            btnUnapprove.setVisibility(View.GONE);
        }
        if (order.getStatus()==OrderStatusEnum.Approved.getValue()){
            btnUnapprove.setVisibility(View.VISIBLE);
            btnApprove.setVisibility(View.GONE);
        }
        if (order.getStatus()==OrderStatusEnum.Unapproved.getValue()){
            btnUnapprove.setVisibility(View.GONE);
            btnApprove.setVisibility(View.VISIBLE);
        }

        if (order.getStatus()!=OrderStatusEnum.Cancel.getValue() && order.getStatus()!=OrderStatusEnum.Unapproved.getValue()){
            if (order.getPaidType()== PaidTypeEnum.ChosePayByCash.getValue()
                    || order.getPaidType() == PaidTypeEnum.ChosePayOnline.getValue()){
                btnPayment.setVisibility((View.VISIBLE));
            }else{
                btnPayment.setVisibility(View.GONE);
            }
        }else{
            btnPayment.setVisibility(View.GONE);
        }
        btnApprove.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Call<ResponseModel<Order>> call = service.getOrderService().changeStatusOrder(order.getId(),OrderStatusEnum.Approved.getValue());
                call.enqueue(new Callback<ResponseModel<Order>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<Order>> call, Response<ResponseModel<Order>> response) {
                        dialog.dismiss();
                        if (response.isSuccessful()){
                            if (response.body().isSucceed()){
                                Toast.makeText(mContext, "Approved", Toast.LENGTH_SHORT).show();

                            }else{
                                Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<Order>> call, Throwable t) {
                        Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });

            }
        });

        btnUnapprove.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Call<ResponseModel<Order>> call = service.getOrderService().changeStatusOrder(order.getId(),OrderStatusEnum.Unapproved.getValue());
                call.enqueue(new Callback<ResponseModel<Order>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<Order>> call, Response<ResponseModel<Order>> response) {
                        dialog.dismiss();
                        if (response.isSuccessful()){
                            if (response.body().isSucceed()){
                                Toast.makeText(mContext, "UnApproved", Toast.LENGTH_SHORT).show();
                            }else{
                                Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<Order>> call, Throwable t) {
                        Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });
            }
        });

        btnPayment.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Call<ResponseModel<Order>> call = service.getOrderService().confirmPayment(order.getId());
                call.enqueue(new Callback<ResponseModel<Order>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<Order>> call, Response<ResponseModel<Order>> response) {
                        dialog.dismiss();
                        if (response.isSuccessful()){
                            if (response.body().isSucceed()){
                                Toast.makeText(mContext, "Đã thanh toán.", Toast.LENGTH_SHORT).show();
                            }else{
                                Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<Order>> call, Throwable t) {
                        Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });
            }
        });
        dialog.show();
        dialog.setOnDismissListener(new DialogInterface.OnDismissListener() {
            @Override
            public void onDismiss(DialogInterface dialog) {
                loadData();
            }
        });

    }


    private void loadData() {
        viewHolder.spFilter.setSelection(0);
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
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }
}

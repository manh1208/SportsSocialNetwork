package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.net.Uri;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.Enumerable.OrderStatusEnum;
import com.capstone.sportssocialnetwork.Enumerable.PaidTypeEnum;
import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Order;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.squareup.picasso.Picasso;

import java.text.ParseException;
import java.util.Date;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 10/17/16.
 */
public class ManageOrderAdapter extends ArrayAdapter<Order> {
    private Context mContext;
    private List<Order> orders;
    private RestService service;

    public ManageOrderAdapter(Context context, int resource, List<Order> objects) {
        super(context, resource, objects);
        mContext = context;
        orders = objects;
        service = new RestService();
    }

    @Override
    public int getCount() {
        return orders.size();
    }

    @Override
    public Order getItem(int position) {
        return orders.get(position);
    }

    public void setOrders(List<Order> orders) {
        this.orders = orders;
        notifyDataSetChanged();
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView==null){
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_order_manage,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        Order order = getItem(position);

        viewHolder.txtFullName.setText(order.getFullName());

        try {
            Date date  = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
            String useDate = Utilities.getDateTimeString(date,"dd/MM/yyyy");
            String startTime = Utilities.getDateTimeString(date,"hh:mm a");
            date = Utilities.getDateTime(order.getEndTime(),"MM/dd/yyyy hh:mm:ss a");
            String endTime = Utilities.getDateTimeString(date,"hh:mm a");
            viewHolder.txtTime.setText(useDate +" : " +startTime +" - " +endTime);
        } catch (ParseException e) {
            viewHolder.txtTime.setText(order.getStartTime()+" - " + order.getEndTime());
            e.printStackTrace();
        }

        viewHolder.txtStatus.setText("" + OrderStatusEnum.fromInteger(order.getStatus()).toString());
        if (order.getStatus()==OrderStatusEnum.Pending.getValue()){
            viewHolder.txtStatus.setBackgroundResource(R.drawable.order_status_waiting);
        }else if (order.getStatus()==OrderStatusEnum.Cancel.getValue() || order.getStatus() == OrderStatusEnum.Unapproved.getValue()){
            viewHolder.txtStatus.setBackgroundResource(R.drawable.order_status_cancel);
        }else{
            viewHolder.txtStatus.setBackgroundResource(R.drawable.order_status_approve);
        }

        return convertView;
    }

    public void showDialog(final Order order) {
        AlertDialog.Builder buider = new AlertDialog.Builder(mContext);
        View view = LayoutInflater.from(mContext).inflate(R.layout.dialog_manage_order_detail, null, false);
        TextView name = (TextView) view.findViewById(R.id.txt_order_detail_fullname);
        name.setText(order.getFullName());
//                Toast.makeText(mContext, (DataUtils.URL+order.getqRCodeUrl()), Toast.LENGTH_SHORT).show();

        TextView useDate = (TextView) view.findViewById(R.id.txt_order_detail_use_date);
        try {
            Date date = Utilities.getDateTime(order.getStartTime(), "MM/dd/yyyy hh:mm:ss a");
            useDate.setText(Utilities.getDateTimeString(date, "dd/MM/yyyy"));
        } catch (ParseException e) {
            Toast.makeText(mContext, "Lỗi parse", Toast.LENGTH_SHORT).show();
        }

        TextView place = (TextView) view.findViewById(R.id.txt_order_detail_place);
        place.setText(order.getPlaceName());
        TextView field = (TextView) view.findViewById(R.id.txt_order_detail_field);
        field.setText(order.getFieldName());
        TextView startTime = (TextView) view.findViewById(R.id.txt_order_detail_start_time);
        try {
            Date date = Utilities.getDateTime(order.getStartTime(), "MM/dd/yyyy hh:mm:ss a");
            startTime.setText(Utilities.getDateTimeString(date, "hh:mm a"));
        } catch (ParseException e) {
            Toast.makeText(mContext, "Lỗi parse", Toast.LENGTH_SHORT).show();
        }


        TextView endTime = (TextView) view.findViewById(R.id.txt_order_detail_end_time);
        try {
            Date date = Utilities.getDateTime(order.getEndTime(), "MM/dd/yyyy hh:mm:ss a");
            endTime.setText(Utilities.getDateTimeString(date, "hh:mm a"));
        } catch (ParseException e) {
            Toast.makeText(mContext, "Lỗi parse", Toast.LENGTH_SHORT).show();
        }

        TextView price = (TextView) view.findViewById(R.id.txt_order_detail_price);
        price.setText(order.getPrice().longValue()+"");

        TextView payment = (TextView) view.findViewById(R.id.txt_order_detail_payment);
        payment.setText(PaidTypeEnum.fromInteger(order.getPaidType()).toString());
        TextView status = (TextView) view.findViewById(R.id.txt_order_detail_order_status);
        status.setText(OrderStatusEnum.fromInteger(order.getStatus()).toString());
        Button btnApprove = (Button) view.findViewById(R.id.btn_maange_order_approve);
        Button btnUnapprove = (Button) view.findViewById(R.id.btn_manage_order_unapprove);
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
        btnApprove.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Call<ResponseModel<Order>> call = service.getOrderService().changeStatusOrder(order.getId(),OrderStatusEnum.Approved.getValue());
                call.enqueue(new Callback<ResponseModel<Order>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<Order>> call, Response<ResponseModel<Order>> response) {
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
                        Toast.makeText(mContext, "Loi ket noi voi server", Toast.LENGTH_SHORT).show();
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
                        Toast.makeText(mContext, "Loi ket noi voi server", Toast.LENGTH_SHORT).show();
                    }
                });
            }
        });
        buider.setView(view)
                .setNegativeButton("OK", null).create().show();
    }

    private final class ViewHolder{
        TextView txtFullName;
        TextView txtTime;
        TextView txtStatus;
        ViewHolder(View v){
            txtFullName = (TextView) v.findViewById(R.id.txt_manage_order_fullname);
            txtTime = (TextView) v.findViewById(R.id.txt_manage_order_time);
            txtStatus = (TextView) v.findViewById(R.id.txt_manage_order_status);
        }
    }
}

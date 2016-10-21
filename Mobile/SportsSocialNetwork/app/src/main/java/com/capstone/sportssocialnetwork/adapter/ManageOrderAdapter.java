package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.net.Uri;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Filter;
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
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;

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
    private final Object mLock = new Object();
    private ArrayList<Order> mOriginalItems;

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
        mOriginalItems = null;
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
            Date date  = Utilities.getDateTime(order.getStartTime(),DataUtils.FORMAT_DATE_TIME);
            String useDate = Utilities.getDateTimeString(date,DataUtils.FORMAT_DATE);
            String startTime = Utilities.getDateTimeString(date,DataUtils.FORMAT_TIME);
            date = Utilities.getDateTime(order.getEndTime(),DataUtils.FORMAT_DATE_TIME);
            String endTime = Utilities.getDateTimeString(date,DataUtils.FORMAT_TIME);
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

    public void filter(int orderStatus) {
       if (mOriginalItems==null){
           mOriginalItems = new ArrayList<>(orders);
       }

       if (orderStatus<=0){
           orders = mOriginalItems;
           notifyDataSetChanged();
       }else{
           List<Order> newlist = new ArrayList<>();
           for (int i = 0; i < mOriginalItems.size(); i++) {
               Order value = mOriginalItems.get(i);
               if (value.getStatus()==orderStatus){
                   newlist.add(value);
               }
           }
           orders = newlist;
           notifyDataSetChanged();
       }

    }


}

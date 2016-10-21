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
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.squareup.picasso.Picasso;

import java.text.ParseException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

/**
 * Created by ManhNV on 10/18/16.
 */
public class MyOrderAdapter extends ArrayAdapter<Order> {
    private Context mContext;
    private List<Order> orders;
    private ArrayList<Order> mOriginalItems;

    public MyOrderAdapter(Context context, int resource, List<Order> objects) {
        super(context, resource, objects);
        mContext = context;
        orders = objects;
    }

    @Override
    public int getCount() {
        return orders.size();
    }

    @Override
    public Order getItem(int position) {
        return orders.get(position);
    }


    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_order_history, parent, false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }

        final Order order = getItem(position);

        viewHolder.txtPlace.setText(order.getPlaceName());

        try {
            Date date = Utilities.getDateTime(order.getStartTime(), DataUtils.FORMAT_DATE_TIME);
            String useDate = Utilities.getDateTimeString(date, DataUtils.FORMAT_DATE);
            String startTime = Utilities.getDateTimeString(date, DataUtils.FORMAT_TIME);
            date = Utilities.getDateTime(order.getEndTime(), DataUtils.FORMAT_DATE_TIME);
            String endTime = Utilities.getDateTimeString(date, DataUtils.FORMAT_TIME);
            viewHolder.txtTime.setText(useDate + " : " + startTime + " - " + endTime);
        } catch (ParseException e) {
            viewHolder.txtTime.setText(order.getStartTime() + " - " + order.getEndTime());
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

    public void showDialog(Order order) {
        AlertDialog.Builder buider = new AlertDialog.Builder(mContext);
        View view = LayoutInflater.from(mContext).inflate(R.layout.dialog_my_order_detail, null, false);


        ImageView ivQR = (ImageView) view.findViewById(R.id.iv_qr_code);

        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + order.getqRCodeUrl()))
                .placeholder(R.drawable.image_logo)
                .error(R.drawable.img_default_avatar)
                .into(ivQR);
//                Toast.makeText(mContext, (DataUtils.URL+order.getqRCodeUrl()), Toast.LENGTH_SHORT).show();


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
        buider.setView(view)
                .setNegativeButton("OK", null).create().show();
    }

    public void setOrders(List<Order> orders) {
        this.orders = orders;
        mOriginalItems = null;
        notifyDataSetChanged();
    }

    public void filter(int position) {
        if (mOriginalItems==null){
            mOriginalItems = new ArrayList<>(orders);
        }

        if (position<=0){
            orders = mOriginalItems;
            notifyDataSetChanged();
        }else{
            List<Order> newlist = new ArrayList<>();
            for (int i = 0; i < mOriginalItems.size(); i++) {
                Order value = mOriginalItems.get(i);
                if (value.getStatus()==position){
                    newlist.add(value);
                }
            }
            orders = newlist;
            notifyDataSetChanged();
        }
    }

    private final class ViewHolder {
        TextView txtPlace;
        TextView txtTime;
        TextView txtStatus;

        ViewHolder(View v) {
            txtPlace = (TextView) v.findViewById(R.id.txt_my_order_place);
            txtTime = (TextView) v.findViewById(R.id.txt_my_order_time);
            txtStatus = (TextView) v.findViewById(R.id.txt_my_order_status);
        }
    }
}

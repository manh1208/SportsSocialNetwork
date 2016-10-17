package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Order;

import java.util.List;

/**
 * Created by ManhNV on 10/17/16.
 */
public class ManageOrderAdapter extends ArrayAdapter<Order> {
    private Context mContext;
    private List<Order> orders;

    public ManageOrderAdapter(Context context, int resource, List<Order> objects) {
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

    public void setOrders(List<Order> orders) {
        this.orders = orders;
        notifyDataSetChanged();
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView==null){
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_manage_order,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        Order order = getItem(position);



        return convertView;
    }

    private final class ViewHolder{
        TextView txtFullName;
        TextView txtTime;
        Button btnDetail;
        ViewHolder(View v){
            txtFullName = (TextView) v.findViewById(R.id.txt_manage_order_fullname);
            txtTime = (TextView) v.findViewById(R.id.txt_manage_order_time);
            btnDetail = (Button) v.findViewById(R.id.btn_manage_order_detail);
        }
    }
}

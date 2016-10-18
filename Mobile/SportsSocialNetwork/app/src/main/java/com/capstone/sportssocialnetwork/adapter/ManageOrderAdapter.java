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
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.text.ParseException;
import java.util.Date;
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

        viewHolder.txtStatus.setText("Trạng thái: " + order.getStatus());

        return convertView;
    }

    private final class ViewHolder{
        TextView txtFullName;
        TextView txtTime;
        TextView txtStatus;
        Button btnDetail;
        ViewHolder(View v){
            txtFullName = (TextView) v.findViewById(R.id.txt_manage_order_fullname);
            txtTime = (TextView) v.findViewById(R.id.txt_manage_order_time);
            txtStatus = (TextView) v.findViewById(R.id.txt_manage_order_status);
            btnDetail = (Button) v.findViewById(R.id.btn_manage_order_detail);
        }
    }
}

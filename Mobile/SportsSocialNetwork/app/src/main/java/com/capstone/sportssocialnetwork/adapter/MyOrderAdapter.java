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

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Order;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.squareup.picasso.Picasso;

import java.text.ParseException;
import java.util.Date;
import java.util.List;

/**
 * Created by ManhNV on 10/18/16.
 */
public class MyOrderAdapter extends ArrayAdapter<Order> {
    private Context mContext;
    private List<Order> orders;

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
        if (convertView ==null){
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_my_order,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        final Order order = getItem(position);

        viewHolder.txtPlace.setText(order.getPlaceName());

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





        viewHolder.btnDetail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                AlertDialog.Builder buider=  new AlertDialog.Builder(mContext);
                View view = LayoutInflater.from(mContext).inflate(R.layout.dialog_my_order_detail,null,false);


                ImageView ivQR = (ImageView) view.findViewById(R.id.iv_qr_code);

                Picasso.with(mContext).load(Uri.parse(DataUtils.URL+order.getqRCodeUrl()))
                        .placeholder(R.drawable.image_logo)
                        .error(R.drawable.img_default_avatar)
                        .into(ivQR);
//                Toast.makeText(mContext, (DataUtils.URL+order.getqRCodeUrl()), Toast.LENGTH_SHORT).show();

                TextView useDate = (TextView) view.findViewById(R.id.txt_order_detail_use_date);
                try {
                    Date date = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
                    useDate.setText( Utilities.getDateTimeString(date,"dd/MM/yyyy"));
                } catch (ParseException e) {
                    Toast.makeText(mContext, "Lỗi parse", Toast.LENGTH_SHORT).show();
                }

                TextView place = (TextView) view.findViewById(R.id.txt_order_detail_place);
                place.setText(order.getPlaceName());
                TextView field = (TextView) view.findViewById(R.id.txt_order_detail_field);
                field.setText(order.getFieldName());
                TextView startTime = (TextView) view.findViewById(R.id.txt_order_detail_start_time);
                try {
                    Date date = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
                    startTime.setText( Utilities.getDateTimeString(date,"hh:mm a"));
                } catch (ParseException e) {
                    Toast.makeText(mContext, "Lỗi parse", Toast.LENGTH_SHORT).show();
                }


                TextView endTime = (TextView) view.findViewById(R.id.txt_order_detail_end_time);
                try {
                    Date date = Utilities.getDateTime(order.getEndTime(),"MM/dd/yyyy hh:mm:ss a");
                    endTime.setText( Utilities.getDateTimeString(date,"hh:mm a"));
                } catch (ParseException e) {
                    Toast.makeText(mContext, "Lỗi parse", Toast.LENGTH_SHORT).show();
                }

                TextView payment = (TextView) view.findViewById(R.id.txt_order_detail_payment);
                payment.setText(order.getPaidType());
                TextView status = (TextView) view.findViewById(R.id.txt_order_detail_order_status);
                status.setText(order.getStatus());
                buider.setView(view)
                        .setNegativeButton("OK",null).create().show();
            }
        });
        return convertView;
    }

    public void setOrders(List<Order> orders) {
        this.orders = orders;
        notifyDataSetChanged();
    }

    private final class ViewHolder{
        TextView txtPlace;
        TextView txtTime;
        TextView txtStatus;
        Button btnDetail;
        ViewHolder(View v){
            txtPlace = (TextView) v.findViewById(R.id.txt_my_order_place);
            txtTime = (TextView) v.findViewById(R.id.txt_my_order_time);
            txtStatus = (TextView) v.findViewById(R.id.txt_my_order_status);
            btnDetail = (Button) v.findViewById(R.id.btn_my_order_detail);
        }
    }
}

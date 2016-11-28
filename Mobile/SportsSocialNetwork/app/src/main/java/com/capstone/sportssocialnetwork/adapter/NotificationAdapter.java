package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Notification;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.text.ParseException;
import java.util.List;

/**
 * Created by ManhNV on 9/8/16.
 */
public class NotificationAdapter extends ArrayAdapter<Notification> {
    private Context mContext;
    private List<Notification> notifications;

    public NotificationAdapter(Context context, int resource, List<Notification> objects) {
        super(context, resource, objects);
        mContext = context;
        notifications = objects;
    }




    @Override
    public int getCount() {
        return notifications.size();
    }

    @Override
    public Notification getItem(int position) {
        return notifications.get(position);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_notification, parent, false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        Notification notification = getItem(position);
        if (!notification.isRead()){
            viewHolder.layout.setBackgroundResource(R.color.notification);
        }else{
            viewHolder.layout.setBackgroundResource(R.color.white);
        }
        viewHolder.txtContent.setText(notification.getMessage());
        try {
            viewHolder.txtTime.setText(Utilities.getTimeAgo(notification.getCreateDate()));
        } catch (ParseException e) {
            e.printStackTrace();
        }
        return convertView;
    }

    public void loadNew() {
        notifications.clear();
        notifyDataSetChanged();
    }

    public void setAppendFeed(List<Notification> data) {
        notifications.addAll(data);
        notifyDataSetChanged();
    }

    private final class ViewHolder{
        LinearLayout layout;
        TextView txtContent;
        TextView txtTime;
        ImageView ivType;
        RoundedImageView ivAvatar;
        ViewHolder(View v){
            layout = (LinearLayout) v.findViewById(R.id.layout_notification);
            txtContent = (TextView) v.findViewById(R.id.txt_notification_content);
            txtTime = (TextView) v.findViewById(R.id.txt_notification_time);
            ivType = (ImageView) v.findViewById(R.id.iv_notification_type);
            ivAvatar = (RoundedImageView) v.findViewById(R.id.iv_notification_avatar);
        }


    }
}

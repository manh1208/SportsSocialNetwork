package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Notification;

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
        return 10;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_notification, parent, false);
        }
        return convertView;
    }
}

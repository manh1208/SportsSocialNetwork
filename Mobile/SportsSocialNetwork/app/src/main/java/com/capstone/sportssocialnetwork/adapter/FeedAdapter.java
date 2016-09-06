package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Feed;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by ManhNV on 9/6/16.
 */
public class FeedAdapter extends ArrayAdapter<Feed> {
    private Context mContext;
    private List<Feed> feeds;

    public FeedAdapter(Context context, int resource, List<Feed> objects) {
        super(context, resource, objects);
        mContext = context;
        feeds = objects;
    }

    @Override
    public int getCount() {
        return 5;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        if (convertView ==null)
        {
         convertView = LayoutInflater.from(mContext).inflate(R.layout.item_feed,parent,false);
        }else{

        }
        return convertView;
    }
}

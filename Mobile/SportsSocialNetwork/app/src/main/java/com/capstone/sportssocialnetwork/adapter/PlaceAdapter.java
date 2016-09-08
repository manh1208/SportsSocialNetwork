package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Place;

import java.util.List;

/**
 * Created by ManhNV on 9/6/16.
 */
public class PlaceAdapter extends ArrayAdapter<Place> {
    private Context mContext;
    private List<Place> places;

    public PlaceAdapter(Context context, int resource, List<Place> objects) {
        super(context, resource, objects);
        mContext = context;
        places = objects;
    }

    @Override
    public int getCount() {
        return 5;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        if (convertView ==null)
        {
         convertView = LayoutInflater.from(mContext).inflate(R.layout.item_place,parent,false);
        }else{

        }
        return convertView;
    }
}

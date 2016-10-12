package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.BookingActivity;
import com.capstone.sportssocialnetwork.activity.PlaceManageDetailActivity;
import com.capstone.sportssocialnetwork.model.Place;

import java.util.List;

/**
 * Created by MyPC on 02/10/2016.
 */
public class PlaceManageAdapter extends ArrayAdapter<Place> {
    private Context mContext;
    private List<Place> places;

    public PlaceManageAdapter(Context context, int resource, List<Place> objects) {
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
        ViewHolder viewHolder;
        if (convertView ==null)
        {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_place_manage,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }
        //Sửa
        viewHolder.btnPlaceDetail.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(mContext, PlaceManageDetailActivity.class);
                mContext.startActivity(intent);
            }
        });
        viewHolder.btnDeletePlace.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

            }
        });
        return convertView;
    }

    private class ViewHolder{
        Button btnPlaceDetail,btnDeletePlace;
        public ViewHolder(View v){
//            btnPlaceDetail = (Button) v.findViewById(R.id.btn_place_detail);
//            btnDeletePlace = (Button) v.findViewById(R.id.btn_delete_place);
        }
    }
}

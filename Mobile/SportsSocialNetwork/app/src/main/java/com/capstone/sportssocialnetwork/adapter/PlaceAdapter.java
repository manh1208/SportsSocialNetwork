package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.BookingActivity;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.Order;
import com.capstone.sportssocialnetwork.model.Place;
import com.capstone.sportssocialnetwork.model.response.PlaceResponseModel;

import java.util.ArrayList;
import java.util.List;
import java.util.Locale;

/**
 * Created by ManhNV on 9/6/16.
 */
public class PlaceAdapter extends ArrayAdapter<PlaceResponseModel> {
    private Context mContext;
    private List<PlaceResponseModel> places;
    private ArrayList<PlaceResponseModel> mOriginalItems;

    public PlaceAdapter(Context context, int resource, List<PlaceResponseModel> objects) {
        super(context, resource, objects);
        mContext = context;
        places = objects;
    }

    @Override
    public int getCount() {
        return places.size();
    }


    @Override
    public PlaceResponseModel getItem(int position) {
        return places.get(position);
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView ==null)
        {
         convertView = LayoutInflater.from(mContext).inflate(R.layout.item_place,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        final PlaceResponseModel place=  getItem(position);
        viewHolder.txtName.setText(place.getName());
        viewHolder.txtAddress.setText(place.getAddressString());
        viewHolder.btnBook.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                Intent intent = new Intent(mContext, BookingActivity.class);
                intent.putExtra("placeId",place.getId());
                intent.putExtra("placeName",place.getName());
                mContext.startActivity(intent);
            }
        });
        return convertView;
    }

    public void loadNew() {
        places.clear();
        mOriginalItems = null;
        notifyDataSetChanged();
    }

    public void setAppendFeed(List<PlaceResponseModel> data) {
        places.addAll(data);
        notifyDataSetChanged();
    }
    
    public void filter(String query){

        if (mOriginalItems==null){
            mOriginalItems = new ArrayList<>(places);
        }

        if (query == null || query.length() == 0) {
            places = mOriginalItems;
            mOriginalItems = null;
            notifyDataSetChanged();
        } else {
            String constraintString = query.toString().toLowerCase(
                    Locale.getDefault());
            List<PlaceResponseModel> newlist = new ArrayList<>();
            for (int i = 0; i < mOriginalItems.size(); i++) {
                final PlaceResponseModel value = mOriginalItems.get(i);
                final String valueText = value.getName().toString().toLowerCase(
                        Locale.getDefault());

                // First match against the whole, non-splitted value
                if (valueText.contains(constraintString)) {
                    newlist.add(value);
                }
            }
            places = newlist;
            notifyDataSetChanged();
        }
    }



    private class ViewHolder{
        ImageView ivAvatar;
        TextView txtName;
        TextView txtAddress;
        Button btnBook;
        public ViewHolder(View v){
            ivAvatar = (ImageView) v.findViewById(R.id.iv_place_list_image);
            txtName = (TextView) v.findViewById(R.id.txt_place_list_name);
            txtAddress = (TextView) v.findViewById(R.id.txt_place_list_address);
            btnBook = (Button) v.findViewById(R.id.btn_booking_now);
        }
    }
}

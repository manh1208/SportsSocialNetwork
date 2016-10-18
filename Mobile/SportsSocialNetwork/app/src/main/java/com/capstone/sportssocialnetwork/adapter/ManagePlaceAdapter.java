package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Place;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

import java.util.List;

/**
 * Created by ManhNV on 10/19/16.
 */
public class ManagePlaceAdapter extends ArrayAdapter<Place> {
    private Context mContext;
    private List<Place> places;

    public ManagePlaceAdapter(Context context, int resource, List<Place> objects) {
        super(context, resource, objects);
        this.mContext = context;
        this.places = objects;
    }


    @Override
    public int getCount() {
        return places.size();
    }

    @Override
    public Place getItem(int position) {
        return places.get(position);
    }

    public void setPlaces(List<Place> places) {
        this.places = places;
        notifyDataSetChanged();
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView==null){
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_place_manage,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        Place place = getItem(position);
        Picasso.with(mContext).load(Uri.parse(DataUtils.URL+place.getAvatar()))
                .placeholder(R.drawable.image_logo)
                .error(R.drawable.img_default_avatar)
                .into(viewHolder.ivImage);
        viewHolder.txtName.setText(place.getName());
        viewHolder.txtAddress.setText(place.getAddressString());
        return convertView;
    }

    private final class ViewHolder{
        ImageView ivImage;
        TextView txtName;
        TextView txtAddress;
        ViewHolder(View v){
            ivImage = (ImageView) v.findViewById(R.id.iv_manage_place_image);
            txtName = (TextView) v.findViewById(R.id.txt_manage_place_name);
            txtAddress = (TextView) v.findViewById(R.id.txt_manage_place_address);
        }
    }
}

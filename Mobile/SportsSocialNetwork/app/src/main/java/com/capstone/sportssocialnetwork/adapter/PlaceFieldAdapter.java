package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.net.Uri;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Field;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

import java.util.List;

/**
 * Created by ManhNV on 9/8/16.
 */
public class PlaceFieldAdapter extends RecyclerView.Adapter<PlaceFieldAdapter.PlaceFieldViewHolder>   {

    private Context mContext;
    private List<Field> fields;

    public PlaceFieldAdapter(Context mContext,List<Field> fields) {
        this.mContext = mContext;
        this.fields = fields;
//        this.event = event;
//        restService = new RestService();
//        userId = DataUtils.getINSTANCE(mContext).getmPreferences().getString(QuickSharePreferences.SHARE_USERID, "");
    }


    @Override
    public PlaceFieldViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(mContext)
                .inflate(R.layout.item_place_field,parent,false);
        return new PlaceFieldViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(PlaceFieldViewHolder holder, int position) {
        Field field = fields.get(position);
        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + field.getAvatar()))
                .placeholder(R.drawable.placeholder)
                .error(R.drawable.ic_image_error)
                .fit()
                .into(holder.ivImage);
        holder.txtName.setText(field.getName());
    }

    public void setFields(List<Field> fields) {
        this.fields = fields;
        notifyDataSetChanged();
    }

    @Override
    public int getItemCount() {
        return fields.size();
    }

    public class PlaceFieldViewHolder extends RecyclerView.ViewHolder {
        ImageView ivImage;
        TextView txtName;
        public PlaceFieldViewHolder(View itemView) {
            super(itemView);
            ivImage = (ImageView) itemView.findViewById(R.id.iv_place_field_image);
            txtName = (TextView) itemView.findViewById(R.id.txt_place_field_name);
        }
    }
}

package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.capstone.sportssocialnetwork.R;

/**
 * Created by ManhNV on 9/8/16.
 */
public class PlaceImageAdapter extends RecyclerView.Adapter<PlaceImageAdapter.PlaceImageViewHolder>   {

    private Context mContext;

    public PlaceImageAdapter(Context mContext) {
        this.mContext = mContext;
//        this.event = event;
//        restService = new RestService();
//        userId = DataUtils.getINSTANCE(mContext).getmPreferences().getString(QuickSharePreferences.SHARE_USERID, "");
    }


    @Override
    public PlaceImageViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(mContext)
                .inflate(R.layout.item_place_image,parent,false);
        return new PlaceImageViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(PlaceImageViewHolder holder, int position) {

    }

    @Override
    public int getItemCount() {
        return 1;
    }

    public class PlaceImageViewHolder extends RecyclerView.ViewHolder {

        public PlaceImageViewHolder(View itemView) {
            super(itemView);
        }
    }
}

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
public class PlaceFieldAdapter extends RecyclerView.Adapter<PlaceFieldAdapter.PlaceFieldViewHolder>   {

    private Context mContext;

    public PlaceFieldAdapter(Context mContext) {
        this.mContext = mContext;
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

    }

    @Override
    public int getItemCount() {
        return 1;
    }

    public class PlaceFieldViewHolder extends RecyclerView.ViewHolder {

        public PlaceFieldViewHolder(View itemView) {
            super(itemView);
        }
    }
}

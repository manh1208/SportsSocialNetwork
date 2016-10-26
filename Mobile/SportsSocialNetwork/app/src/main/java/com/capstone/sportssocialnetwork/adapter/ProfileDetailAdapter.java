package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.User;

/**
 * Created by ManhNV on 9/8/16.
 */
public class ProfileDetailAdapter extends RecyclerView.Adapter<ProfileDetailAdapter.ContentViewHolder> {

    private Context mContext;
    private User user;

    public ProfileDetailAdapter(Context mContext,User user) {
        this.mContext = mContext;
        this.user = user;
//        this.event = event;
//        restService = new RestService();
//        userId = DataUtils.getINSTANCE(mContext).getmPreferences().getString(QuickSharePreferences.SHARE_USERID, "");
    }

    public void setUser(User user) {
        this.user = user;
        notifyDataSetChanged();
    }

    @Override
    public ContentViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(mContext)
                .inflate(R.layout.item_profile_detail, parent, false);
        return new ContentViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(ContentViewHolder holder, int position) {
        holder.txtEmail.setText(user.getEmail());
        holder.txtPhoneNumber.setText(user.getPhoneNumber());
        holder.txtAddress.setText(user.getAddressString());
        holder.txtSport.setText(user.getListSport());
        holder.txtFollowed.setText(user.getFollowedCount()+"");
        holder.txtFollowing.setText(user.getFollowCount()+"");
        holder.txtPostCount.setText(user.getPostCount()+"");

    }

    @Override
    public int getItemCount() {
        return 1;
    }

    public class ContentViewHolder extends RecyclerView.ViewHolder {
        TextView txtEmail;
        TextView txtPhoneNumber;
        TextView txtAddress;
        TextView txtSport;
        TextView txtFollowing;
        TextView txtFollowed;
        TextView txtPostCount;

        public ContentViewHolder(View itemView) {
            super(itemView);
            txtEmail = (TextView) itemView.findViewById(R.id.txt_profile_email);
            txtPhoneNumber = (TextView) itemView.findViewById(R.id.txt_profile_phone_number);
            txtAddress = (TextView) itemView.findViewById(R.id.txt_profile_address);
            txtSport = (TextView) itemView.findViewById(R.id.txt_profile_sport);
            txtFollowing = (TextView) itemView.findViewById(R.id.txt_profile_following);
            txtFollowed = (TextView) itemView.findViewById(R.id.txt_profile_followed);
            txtPostCount = (TextView) itemView.findViewById(R.id.txt_profile_post_count);

        }
    }
}

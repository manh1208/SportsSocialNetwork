package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.User;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/8/16.
 */
public class ProfileDetailAdapter extends RecyclerView.Adapter<ProfileDetailAdapter.ContentViewHolder> {

    private Context mContext;
    private User user;
    private String userId;
    private RestService service;

    public ProfileDetailAdapter(Context mContext,User user) {
        this.mContext = mContext;
        this.user = user;
        userId = DataUtils.getINSTANCE(mContext).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
//        this.event = event;
        service = new RestService();
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
    public void onBindViewHolder(final ContentViewHolder holder, int position) {
        holder.txtEmail.setText(user.getEmail());
        holder.txtPhoneNumber.setText(user.getPhoneNumber());
        holder.txtAddress.setText(user.getAddressString());
        holder.txtSport.setText(user.getListSport());
        holder.txtFollowed.setText(user.getFollowedCount()+"");
        holder.txtFollowing.setText(user.getFollowCount()+"");
        holder.txtPostCount.setText(user.getPostCount()+"");

        if (user.getId()!=null && user.getId().equals(userId)){
            holder.btnFollow.setText("Chỉnh sửa");
        }else{
            if (!user.isFollowed()) {
                holder.btnFollow.setText("Theo dõi");
            }else{
                holder.btnFollow.setText("Bỏ theo dõi");
            }
            holder.btnFollow.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    service.getAccountService()
                            .followUser(user.getId(), userId)
                            .enqueue(new Callback<ResponseModel<String>>() {
                                @Override
                                public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                                    if (response.isSuccessful()) {
                                        if (response.body().isSucceed()) {
                                            user.setFollowed(!user.isFollowed());
                                            notifyDataSetChanged();
                                        } else {
                                            Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                                        }
                                    } else {
                                        Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                                    }
                                }

                                @Override
                                public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                                    Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
                                }
                            });
                }
            });
        }

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
        Button btnFollow;

        public ContentViewHolder(View itemView) {
            super(itemView);
            txtEmail = (TextView) itemView.findViewById(R.id.txt_profile_email);
            txtPhoneNumber = (TextView) itemView.findViewById(R.id.txt_profile_phone_number);
            txtAddress = (TextView) itemView.findViewById(R.id.txt_profile_address);
            txtSport = (TextView) itemView.findViewById(R.id.txt_profile_sport);
            txtFollowing = (TextView) itemView.findViewById(R.id.txt_profile_following);
            txtFollowed = (TextView) itemView.findViewById(R.id.txt_profile_followed);
            txtPostCount = (TextView) itemView.findViewById(R.id.txt_profile_post_count);
            btnFollow = (Button) itemView.findViewById(R.id.btn_profile_follow);
        }
    }
}

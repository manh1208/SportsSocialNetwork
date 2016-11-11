package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v7.widget.PopupMenu;
import android.view.LayoutInflater;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.model.User;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.squareup.picasso.Picasso;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 11/11/16.
 */

public class PeopleSearchResultAdapter extends ArrayAdapter<User> implements View.OnClickListener {
    private Context mContext;
    private List<User> users;
    private boolean followed;
    private RestService service;
    private String userId;

    public PeopleSearchResultAdapter(Context context, int resource, List<User> objects) {
        super(context, resource, objects);
        this.mContext = context;
        this.users = objects;
//        this.followed = followed;
//        service = new RestService();
//        userId = DataUtils.getINSTANCE(mContext).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
    }

    @Nullable
    @Override
    public User getItem(int position) {
        return users.get(position);
    }

    @Override
    public int getCount() {
        return users.size();
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_friend, parent, false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }

        User user = getItem(position);
        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + user.getAvatar()))
                .placeholder(R.drawable.img_default_avatar)
                .error(R.drawable.img_default_avatar_error)
                .fit()
                .into(viewHolder.ivAvatar);

        viewHolder.txtName.setText(user.getFullName());
        viewHolder.txtHobby.setText(user.getListSport());
//        viewHolder.btnDropdown.setTag(position);
//        viewHolder.btnDropdown.setOnClickListener(this);
        viewHolder.btnDropdown.setVisibility(View.GONE);
        viewHolder.ivAvatar.setTag(position);
        viewHolder.ivAvatar.setOnClickListener(this);
        viewHolder.txtName.setTag(position);
        viewHolder.txtName.setOnClickListener(this);

        return convertView;
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        int position = (int) v.getTag();
        switch (id) {
            case R.id.iv_friend_avatar:
            case R.id.txt_friend_name:
                User user = getItem(position);
                Intent intent = new Intent(mContext, ProfileActivity.class);
                intent.putExtra("userId",user.getId());
                mContext.startActivity(intent);
                break;
        }
    }

    public void setUsers(List<User> users) {
        this.users = users;
        notifyDataSetChanged();
    }

    private final class ViewHolder {
        RoundedImageView ivAvatar;
        TextView txtName;
        TextView txtHobby;
        ImageButton btnDropdown;

        ViewHolder(View v) {
            ivAvatar = (RoundedImageView) v.findViewById(R.id.iv_friend_avatar);
            txtName = (TextView) v.findViewById(R.id.txt_friend_name);
            txtHobby = (TextView) v.findViewById(R.id.txt_friend_hobbies);
            btnDropdown = (ImageButton) v.findViewById(R.id.btn_friend_menu_down);
        }
    }
}

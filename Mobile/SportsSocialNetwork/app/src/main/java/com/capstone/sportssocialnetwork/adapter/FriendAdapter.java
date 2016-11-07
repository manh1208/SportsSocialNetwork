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
import com.capstone.sportssocialnetwork.activity.GroupMemberActivity;
import com.capstone.sportssocialnetwork.activity.MainBottomBarActivity;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.GroupMember;
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
 * Created by ManhNV on 11/5/16.
 */

public class FriendAdapter extends ArrayAdapter<User> implements View.OnClickListener {
    private Context mContext;
    private List<User> users;
    private boolean followed;
    private RestService service;
    private String userId;

    public FriendAdapter(Context context, int resource, List<User> objects, boolean followed) {
        super(context, resource, objects);
        this.mContext = context;
        this.users = objects;
        this.followed = followed;
        service = new RestService();
        userId = DataUtils.getINSTANCE(mContext).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
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
        viewHolder.btnDropdown.setTag(position);
        viewHolder.btnDropdown.setOnClickListener(this);
        if (followed) {
            if (user.isFollowed()) {
                viewHolder.btnDropdown.setVisibility(View.GONE);
            } else {
                viewHolder.btnDropdown.setVisibility(View.VISIBLE);
            }
        }

        return convertView;
    }

    public void showPopupMenu(View v, int position) {
        PopupMenu popupMenu = new PopupMenu(mContext, v);
        final MenuInflater inflater = popupMenu.getMenuInflater();
        inflater.inflate(R.menu.menu_friend, popupMenu.getMenu());
        popupMenu.show();
        MenuItem item = popupMenu.getMenu().getItem(0);
        if (followed) {
            item.setTitle("Theo dõi");
        } else {
            item.setTitle("Bỏ theo dõi");
        }
        final User user = getItem(position);
        popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
            @Override
            public boolean onMenuItemClick(MenuItem item) {
                int id = item.getItemId();
                switch (id) {
                    case R.id.menu_follow:
                        service.getAccountService()
                                .followUser(user.getId(), userId)
                                .enqueue(new Callback<ResponseModel<String>>() {
                                    @Override
                                    public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                                        if (response.isSuccessful()) {
                                            if (response.body().isSucceed()) {
                                                if (!followed) {
                                                    users.remove(user);
                                                    notifyDataSetChanged();
                                                }
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
                        return true;

                }
                return false;
            }
        });
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        int position = (int) v.getTag();
        switch (id) {
            case R.id.btn_friend_menu_down:
                showPopupMenu(v, position);
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

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
import com.capstone.sportssocialnetwork.activity.GroupInformationActivity;
import com.capstone.sportssocialnetwork.activity.GroupMemberActivity;
import com.capstone.sportssocialnetwork.activity.MainBottomBarActivity;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.GroupMember;
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
 * Created by ManhNV on 11/1/16.
 */

public class GroupMemberAdapter extends ArrayAdapter<GroupMember> implements View.OnClickListener{
    private Context mContext;
    private List<GroupMember> groupMembers;
    private boolean isAdmin;
    private RestService service;
    private String userId;

    public GroupMemberAdapter(Context context, int resource, List<GroupMember> objects,boolean isAdmin) {
        super(context, resource, objects);
        mContext = context;
        groupMembers = objects;
        this.isAdmin = isAdmin;
        service = new RestService();
        userId = DataUtils.getINSTANCE(mContext).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
    }

    @Override
    public int getCount() {
        return groupMembers.size();
    }

    @Nullable
    @Override
    public GroupMember getItem(int position) {
        return groupMembers.get(position);
    }

    public void setGroupMembers(List<GroupMember> groupMembers) {
        this.groupMembers = groupMembers;
        notifyDataSetChanged();
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView ==null){
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_group_member,parent,false);
            viewHolder =  new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }
        GroupMember groupMember = getItem(position);

        if (groupMember.getUser()!=null){
            Picasso.with(mContext).load(Uri.parse(DataUtils.URL + groupMember.getUser().getAvatar()))
                    .placeholder(R.drawable.img_default_avatar)
                    .error(R.drawable.img_default_avatar_error)
                    .fit()
                    .into(viewHolder.ivAvatar);
            viewHolder.txtName.setText(groupMember.getUser().getFullName());
        }else{
            Picasso.with(mContext).load(Uri.parse(DataUtils.URL +"/SSNImages/UserImages/img_default_avatar.png"))
                    .placeholder(R.drawable.img_default_avatar)
                    .error(R.drawable.img_default_avatar_error)
                    .fit()
                    .into(viewHolder.ivAvatar);
            viewHolder.txtName.setText(groupMember.getUserId());
        }
        if (!isAdmin){
            viewHolder.btnDropdown.setVisibility(View.GONE);

        }else{
            viewHolder.btnDropdown.setVisibility(View.VISIBLE);
        }
        viewHolder.btnDropdown.setTag(position);
        viewHolder.btnDropdown.setOnClickListener(this);
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
        switch (id){
            case R.id.btn_group_member_menu_down:
                showPopupMenu(v,position);
                break;
            case R.id.iv_group_member_avatar:
            case R.id.txt_group_member_name:
                GroupMember groupMember= getItem(position);
                Intent intent = new Intent(mContext, ProfileActivity.class);
                intent.putExtra("userId",groupMember.getUser().getId());
                mContext.startActivity(intent);
                break;
        }
    }

    public void showPopupMenu(View v, final int position) {
        PopupMenu popupMenu = new PopupMenu(mContext, v);
        final MenuInflater inflater = popupMenu.getMenuInflater();
        inflater.inflate(R.menu.menu_group_member_popup, popupMenu.getMenu());
        popupMenu.show();
        popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
            @Override
            public boolean onMenuItemClick(MenuItem item) {
                int id = item.getItemId();
                switch (id) {
                    case R.id.menu_group_kick_member:
                        final String deleteUserId = getItem(position).getUser().getId();
                        service.getGroupService()
                                .kickFromGroup(getItem(position).getId())
                                .enqueue(new Callback<ResponseModel<GroupMember>>() {
                                    @Override
                                    public void onResponse(Call<ResponseModel<GroupMember>> call, Response<ResponseModel<GroupMember>> response) {
                                        if (response.isSuccessful()) {
                                            if (response.body().isSucceed()) {
                                                groupMembers.remove(position);
                                                notifyDataSetChanged();
                                                if (getCount()<=0 || userId.equals(deleteUserId)){
                                                    Intent intent = new Intent(mContext,MainBottomBarActivity.class);
                                                    intent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP);
                                                    mContext.startActivity(intent);
                                                    ((GroupMemberActivity)mContext).finish();
                                                }
                                            } else {
                                                Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                                            }
                                        } else {
                                            Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                                        }
                                    }

                                    @Override
                                    public void onFailure(Call<ResponseModel<GroupMember>> call, Throwable t) {
                                        Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
                                    }
                                });
                        return true;

                }
                return false;
            }
        });
    }

    private final class ViewHolder{
        RoundedImageView ivAvatar;
        TextView txtName;
        ImageButton btnDropdown;
        ViewHolder(View v){
            ivAvatar = (RoundedImageView) v.findViewById(R.id.iv_group_member_avatar);
            txtName = (TextView) v.findViewById(R.id.txt_group_member_name);
            btnDropdown = (ImageButton) v.findViewById(R.id.btn_group_member_menu_down);
        }
    }
}

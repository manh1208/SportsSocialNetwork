package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.net.Uri;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.GroupMember;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

import java.util.List;

/**
 * Created by ManhNV on 11/1/16.
 */

public class GroupMemberAdapter extends ArrayAdapter<GroupMember> {
    private Context mContext;
    private List<GroupMember> groupMembers;

    public GroupMemberAdapter(Context context, int resource, List<GroupMember> objects) {
        super(context, resource, objects);
        mContext = context;
        groupMembers = objects;
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
        return convertView;
    }

    private final class ViewHolder{
        RoundedImageView ivAvatar;
        TextView txtName;
        ViewHolder(View v){
            ivAvatar = (RoundedImageView) v.findViewById(R.id.iv_group_member_avatar);
            txtName = (TextView) v.findViewById(R.id.txt_group_member_name);
        }
    }
}

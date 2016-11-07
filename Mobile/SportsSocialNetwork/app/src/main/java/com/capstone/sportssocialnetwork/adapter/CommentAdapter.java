package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.ImageViewerActivity;
import com.capstone.sportssocialnetwork.activity.PostDetailActivity;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.custom.CustomImage;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Comment;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.squareup.picasso.Picasso;

import org.w3c.dom.Text;

import java.text.ParseException;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/6/16.
 */
public class CommentAdapter extends ArrayAdapter<Comment> implements View.OnClickListener {
    private Context mContext;
    private List<Comment> comments;

    public CommentAdapter(Context context, int resource, List<Comment> objects) {
        super(context, resource, objects);
        mContext = context;
        comments = objects;
    }

    @Override
    public Comment getItem(int position) {
        return comments.get(position);
    }

    @Override
    public int getCount() {
        return comments.size();
    }


    public void setAppendFeed(List<Comment> data) {
        comments.addAll(data);
        notifyDataSetChanged();
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_comment, parent, false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }
        Comment comment = getItem(position);
        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + comment.getUser().getAvatar()))
                .placeholder(R.drawable.img_default_avatar)
                .error(R.drawable.img_default_avatar_error)
                .fit()
                .into(viewHolder.ivAvatar);
        viewHolder.txtName.setText(comment.getUser().getFullName());
        viewHolder.txtComment.setText(comment.getComment());
        if (comment.getImage() == null || comment.getImage().equals("")) {
            viewHolder.ivImage.setVisibility(View.GONE);
        } else {
            viewHolder.ivImage.setVisibility(View.VISIBLE);
            Picasso.with(mContext).load(Uri.parse(DataUtils.URL + comment.getImage()))
                    .placeholder(R.drawable.placeholder)
                    .error(R.drawable.img_default_avatar_error)
                    .fit()
                    .into(viewHolder.ivImage);
        }
        try {
            viewHolder.txtTime.setText(Utilities.getTimeAgo(comment.getCreateDate()));
        } catch (ParseException e) {
            Toast.makeText(mContext, R.string.parse_exception, Toast.LENGTH_SHORT).show();
        }
//        viewHolder.btnComment.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                Intent intent = new Intent(mContext, PostDetailActivity.class);
//                mContext.startActivity(intent);
//            }
//        });
        viewHolder.ivAvatar.setTag(position);
        viewHolder.ivAvatar.setOnClickListener(this);
        viewHolder.txtName.setTag(position);
        viewHolder.txtName.setOnClickListener(this);

        return convertView;
    }

    public void addNew() {
        comments.clear();
        notifyDataSetChanged();
    }

    @Override
    public void onClick(final View v) {
        int id = v.getId();
        final int position = (int) v.getTag();
        switch (id) {
            case R.id.iv_comment_avatar:
            case R.id.txt_comment_name:
                Comment comment = getItem(position);
                Intent intent = new Intent(mContext, ProfileActivity.class);
                intent.putExtra("userId", comment.getUserId());
                mContext.startActivity(intent);
                break;
        }
    }

    private final class ViewHolder {
        RoundedImageView ivAvatar;
        TextView txtName;
        TextView txtComment;
        TextView txtTime;
        CustomImage ivImage;

        public ViewHolder(View v) {
            ivAvatar = (RoundedImageView) v.findViewById(R.id.iv_comment_avatar);
            txtName = (TextView) v.findViewById(R.id.txt_comment_name);
            txtComment = (TextView) v.findViewById(R.id.txt_comment_content);
            txtTime = (TextView) v.findViewById(R.id.txt_comment_time);
            ivImage = (CustomImage) v.findViewById(R.id.iv_comment_image);
        }

    }
}

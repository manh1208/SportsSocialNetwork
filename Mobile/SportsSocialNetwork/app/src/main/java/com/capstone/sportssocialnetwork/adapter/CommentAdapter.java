package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.PostDetailActivity;
import com.capstone.sportssocialnetwork.custom.CustomImage;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Comment;
import com.capstone.sportssocialnetwork.model.Feed;

import org.w3c.dom.Text;

import java.util.List;

/**
 * Created by ManhNV on 9/6/16.
 */
public class CommentAdapter extends ArrayAdapter<Comment> {
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
        viewHolder.txtName.setText(comment.getUser().getFullName());
        viewHolder.txtComment.setText(comment.getComment());
        if (comment.getImage()==null || comment.getImage().equals("")){
            viewHolder.ivImage.setVisibility(View.GONE);
        }else{
            viewHolder.imageView.setVisibility(View.VISIBLE);
        }
//        viewHolder.btnComment.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                Intent intent = new Intent(mContext, PostDetailActivity.class);
//                mContext.startActivity(intent);
//            }
//        });
        return convertView;
    }

    public void addNew() {
        comments.clear();
        notifyDataSetChanged();
    }

    private final class ViewHolder {
        RoundedImageView imageView;
        TextView txtName;
        TextView txtComment;
        TextView txtTime;
        CustomImage ivImage;

        public ViewHolder(View v) {
            imageView = (RoundedImageView) v.findViewById(R.id.iv_comment_avatar);
            txtName = (TextView) v.findViewById(R.id.txt_comment_name);
            txtComment = (TextView) v.findViewById(R.id.txt_comment_content);
            txtTime = (TextView) v.findViewById(R.id.txt_comment_time);
            ivImage = (CustomImage) v.findViewById(R.id.iv_comment_image);
        }

    }
}

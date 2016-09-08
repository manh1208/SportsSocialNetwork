package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.PostDetailActivity;
import com.capstone.sportssocialnetwork.model.Comment;
import com.capstone.sportssocialnetwork.model.Feed;

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
    public int getCount() {
        return 5;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
//        ViewHolder viewHolder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_comment, parent, false);
//            viewHolder = new ViewHolder(convertView);
//            convertView.setTag(viewHolder);
        } else {
//            viewHolder = (ViewHolder) convertView.getTag();
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

    private final class ViewHolder {
        Button btnComment;
        Button btnLike;

        public ViewHolder(View v) {
            btnComment = (Button) v.findViewById(R.id.btn_feed_comment);
            btnLike = (Button) v.findViewById(R.id.btn_feed_like);
        }

    }
}

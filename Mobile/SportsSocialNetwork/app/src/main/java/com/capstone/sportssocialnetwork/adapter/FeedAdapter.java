package com.capstone.sportssocialnetwork.adapter;

import android.annotation.TargetApi;
import android.content.Context;
import android.content.Intent;
import android.content.res.ColorStateList;
import android.graphics.Color;
import android.graphics.PorterDuff;
import android.graphics.drawable.Drawable;
import android.os.Build;
import android.support.v7.widget.AppCompatDrawableManager;
import android.support.v7.widget.PopupMenu;
import android.view.LayoutInflater;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.PostDetailActivity;
import com.capstone.sportssocialnetwork.custom.CustomImage;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Feed;

import org.w3c.dom.Text;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by ManhNV on 9/6/16.
 */
public class FeedAdapter extends ArrayAdapter<Feed> implements View.OnClickListener {
    private Context mContext;
    private List<Feed> feeds;

    public FeedAdapter(Context context, int resource, List<Feed> objects) {
        super(context, resource, objects);
        mContext = context;
        feeds = objects;
    }

    @Override
    public int getCount() {
        return feeds.size();
    }

    @Override
    public Feed getItem(int position) {

        return feeds.get(position);

    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_feed, parent, false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }

        Feed feed = getItem(position);
        viewHolder.txtNumOfLike.setText(feed.getLikeCount() + "");
        viewHolder.txtNumOfComment.setText(feed.getCommentCount() + " bình luận");
        if (feed.isLiked()) {
            setLiked(viewHolder.btnLike);
        } else {
            setUnLiked(viewHolder.btnLike);
        }
        viewHolder.txtName.setText(feed.getUser().getFullName());
        viewHolder.txtContent.setText(feed.getPostContent());
        if (feed.getImage() != null) {
            viewHolder.ivImage.setVisibility(View.VISIBLE);
        } else {
            viewHolder.ivImage.setVisibility(View.GONE);
        }
        viewHolder.btnMenuPopUp.setTag(position);
        viewHolder.btnMenuPopUp.setOnClickListener(this);
        viewHolder.btnComment.setTag(position);
        viewHolder.btnComment.setOnClickListener(this);
        viewHolder.btnLike.setTag(position);
        viewHolder.btnLike.setOnClickListener(this);
        return convertView;
    }

    @TargetApi(Build.VERSION_CODES.LOLLIPOP)
    private void setLiked(Button btnLike) {

        Drawable[] drawables = btnLike.getCompoundDrawables();
        drawables[0].setTint(mContext.getResources().getColor(R.color.colorPrimary));
        btnLike.setTextColor(mContext.getResources().getColor(R.color.colorPrimary));
        btnLike.setCompoundDrawablesRelativeWithIntrinsicBounds(drawables[0], drawables[1], drawables[2], drawables[3]);

    }

    @TargetApi(Build.VERSION_CODES.LOLLIPOP)
    private void setUnLiked(Button btnLike) {
        Drawable[] drawables = btnLike.getCompoundDrawables();
        drawables[0].setTint(Color.BLACK);
        btnLike.setTextColor(Color.BLACK);
        btnLike.setCompoundDrawablesRelativeWithIntrinsicBounds(drawables[0], drawables[1], drawables[2], drawables[3]);
    }

    public void setFeeds(List<Feed> feeds) {
        this.feeds = feeds;
//        notifyDataSetChanged();
    }

    public void showPopupMenu(View v) {
        PopupMenu popupMenu = new PopupMenu(mContext, v);
        final MenuInflater inflater = popupMenu.getMenuInflater();
        inflater.inflate(R.menu.menu_feed_popup, popupMenu.getMenu());
        popupMenu.show();
        popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
            @Override
            public boolean onMenuItemClick(MenuItem item) {
                int id = item.getItemId();
                switch (id) {
                    case R.id.menu_feed_edit:

                        return true;
                    case R.id.menu_feed_delete:

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
            case R.id.btn_feed_like:
                setLiked((Button) v);
                break;
            case R.id.btn_feed_comment:
                Intent intent = new Intent(mContext, PostDetailActivity.class);
                mContext.startActivity(intent);
                break;

            case R.id.btn_feed_menu_down:
                showPopupMenu(v);
                break;

            case R.id.txt_feed_name:

                break;
        }
    }

    public void setAppendFeed(List<Feed> data) {
        feeds.addAll(data);
//        notifyDataSetChanged();
    }

    public void loadNew() {
        feeds = new ArrayList<>();
//        notifyDataSetChanged();
    }


    private final class ViewHolder {
        RoundedImageView ivAvatar;
        TextView txtName;
        TextView txtTime;
        ImageView btnMenuPopUp;
        TextView txtContent;
        CustomImage ivImage;
        TextView txtNumOfLike;
        TextView txtNumOfComment;
        Button btnComment;
        Button btnLike;

        public ViewHolder(View v) {
            ivAvatar = (RoundedImageView) v.findViewById(R.id.iv_feed_avatar);
            txtName = (TextView) v.findViewById(R.id.txt_feed_name);
            txtTime = (TextView) v.findViewById(R.id.txt_feed_time);
            btnMenuPopUp = (ImageView) v.findViewById(R.id.btn_feed_menu_down);
            txtContent = (TextView) v.findViewById(R.id.txt_feed_post_content);
            ivImage = (CustomImage) v.findViewById(R.id.iv_feed_post_image);
            txtNumOfLike = (TextView) v.findViewById(R.id.txt_feed_num_of_like);
            txtNumOfComment = (TextView) v.findViewById(R.id.txt_feed_num_of_comment);
            btnComment = (Button) v.findViewById(R.id.btn_feed_comment);
            btnLike = (Button) v.findViewById(R.id.btn_feed_like);

        }

    }
}

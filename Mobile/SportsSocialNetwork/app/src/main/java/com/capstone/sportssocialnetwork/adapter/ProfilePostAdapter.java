package com.capstone.sportssocialnetwork.adapter;

import android.annotation.TargetApi;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.net.Uri;
import android.os.Build;
import android.support.v7.widget.PopupMenu;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.PostDetailActivity;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.custom.CustomImage;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.squareup.picasso.Picasso;

import java.text.ParseException;
import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/8/16.
 */
public class ProfilePostAdapter extends RecyclerView.Adapter<ProfilePostAdapter.ContentViewHolder> implements View.OnClickListener {

    private Context mContext;
    private List<Feed> feeds;
    private RestService service;
    private String userId;

    public ProfilePostAdapter(Context mContext) {
        this.mContext = mContext;
        feeds = new ArrayList<>();
        service=  new RestService();
//        this.event = event;
//        restService = new RestService();
        userId = DataUtils.getINSTANCE(mContext).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
    }

    public void setAppendFeed(List<Feed> data) {
        feeds.addAll(data);
        notifyDataSetChanged();
    }

    public void loadNew() {
        feeds.clear();
        notifyDataSetChanged();
    }


    @Override
    public ContentViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(mContext)
                .inflate(R.layout.item_feed,parent,false);
        return new ContentViewHolder(itemView);
    }

    @Override
    public void onBindViewHolder(ContentViewHolder viewHolder, final int position) {

        Feed feed = feeds.get(position);
        viewHolder.txtNumOfLike.setText(feed.getLikeCount() + "");
        viewHolder.txtNumOfComment.setText(feed.getCommentCount() + " bình luận");
        if (feed.isLiked()) {
            setLiked(viewHolder.btnLike);
        } else {
            setUnLiked(viewHolder.btnLike);
        }


        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + feed.getUser().getAvatar()))
                .placeholder(R.drawable.img_default_avatar)
                .error(R.drawable.img_default_avatar_error)
                .fit()
                .into(viewHolder.ivAvatar);
        try {
            viewHolder.txtTime.setText(Utilities.getTimeAgo(feed.getCreateDateString()));
        } catch (ParseException e) {
            Toast.makeText(mContext, R.string.parse_exception, Toast.LENGTH_SHORT).show();
        }
        viewHolder.txtName.setText(feed.getUser().getFullName());
        viewHolder.txtContent.setText(feed.getPostContent());
        if (feed.getPostImages().size()>0) {
            viewHolder.ivImage.setVisibility(View.VISIBLE);
//            viewHolder.ivImage.setImageURI(Uri.parse(DataUtils.URL+feed.getPostImages().get(0).getImage()));
//
            Picasso.with(mContext).load(Uri.parse(DataUtils.URL + feed.getPostImages().get(0).getImage()))
                    .placeholder(R.drawable.placeholder)
                    .error(R.drawable.placeholder)
                    .fit()
                    .into(viewHolder.ivImage);
        } else {
            viewHolder.ivImage.setVisibility(View.GONE);
        }
        if (userId.equals(feed.getUser().getId())){
            viewHolder.btnMenuPopUp.setVisibility(View.VISIBLE);
        }else{
            viewHolder.btnMenuPopUp.setVisibility(View.GONE);
        }
        viewHolder.btnMenuPopUp.setTag(position);
        viewHolder.btnMenuPopUp.setOnClickListener(this);
        viewHolder.btnComment.setTag(position);
        viewHolder.btnComment.setOnClickListener(this);
        viewHolder.btnLike.setTag(position);
        viewHolder.btnLike.setOnClickListener(this);

        viewHolder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Feed feed = feeds.get(position);
                    Intent intent = new Intent(mContext, PostDetailActivity.class);
                    intent.putExtra("postId", feed.getId());
                    mContext.startActivity(intent);
            }
        });
        viewHolder.ivAvatar.setTag(position);
        viewHolder.ivAvatar.setOnClickListener(this);
        viewHolder.txtName.setTag(position);
        viewHolder.txtName.setOnClickListener(this);
    }

    private void setLiked(Button btnLike) {

        Drawable[] drawables = btnLike.getCompoundDrawables();
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            drawables[0].setTint(mContext.getResources().getColor(R.color.colorPrimary));
        }
        btnLike.setTextColor(mContext.getResources().getColor(R.color.colorPrimary));
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1) {
            btnLike.setCompoundDrawablesRelativeWithIntrinsicBounds(drawables[0], drawables[1], drawables[2], drawables[3]);
        }

    }


    private void setUnLiked(Button btnLike) {
        Drawable[] drawables = btnLike.getCompoundDrawables();
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.LOLLIPOP) {
            drawables[0].setTint(Color.BLACK);
        }
        btnLike.setTextColor(Color.BLACK);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.JELLY_BEAN_MR1) {
            btnLike.setCompoundDrawablesRelativeWithIntrinsicBounds(drawables[0], drawables[1], drawables[2], drawables[3]);
        }
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
    public void onClick(final View v) {
        int id = v.getId();
        final int position = (int) v.getTag();
        switch (id) {
            case R.id.btn_feed_like:
                v.setEnabled(false);
                Call<ResponseModel<String>> callLike = service.getPostService().likePost(getItem(position).getId(),userId);
                callLike.enqueue(new Callback<ResponseModel<String>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                        v.setEnabled(true);
                        if(response.isSuccessful()){
                            if (response.body().isSucceed()){
                                Feed feed = getItem(position);
                                feed.setLiked(!feed.isLiked());
                                if (feed.isLiked()){
                                    feed.setLikeCount(feed.getLikeCount()+1);
                                }else{
                                    feed.setLikeCount(feed.getLikeCount()-1);
                                }
                                notifyDataSetChanged();
                            }else{
                                Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(mContext, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                        v.setEnabled(true);
                        Toast.makeText(mContext, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });
                break;
            case R.id.btn_feed_comment:
                Intent intent = new Intent(mContext, PostDetailActivity.class);
                intent.putExtra("postId",getItem(position).getId());
                mContext.startActivity(intent);
                break;

            case R.id.btn_feed_menu_down:
                showPopupMenu(v);
                break;

            case R.id.iv_feed_avatar:
            case R.id.txt_feed_name:
                Feed feed = getItem(position);
                intent = new Intent(mContext, ProfileActivity.class);
                intent.putExtra("userId",feed.getUserId());
                mContext.startActivity(intent);
                break;
        }
    }

    private Feed getItem(int position) {
        return feeds.get(position);
    }

    @Override
    public int getItemCount() {
        return feeds.size();
    }

    public class ContentViewHolder extends RecyclerView.ViewHolder {
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
        public ContentViewHolder(View v) {
            super(v);
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

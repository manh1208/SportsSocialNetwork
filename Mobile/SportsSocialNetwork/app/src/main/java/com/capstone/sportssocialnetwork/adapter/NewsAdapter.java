package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.net.Uri;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.News;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

import java.util.List;

/**
 * Created by ManhNV on 11/10/16.
 */

public class NewsAdapter extends ArrayAdapter<News> {
    private Context mContext;
    private List<News> newses;

    public NewsAdapter(Context context, int resource, List<News> objects) {
        super(context, resource, objects);
        mContext = context;
        newses = objects;
    }

    @Override
    public int getCount() {
        return newses.size();
    }

    @Nullable
    @Override
    public News getItem(int position) {
        return newses.get(position);
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_news, parent, false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }

        News news = getItem(position);
        viewHolder.txtTitle.setText(news.getTitle());
        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + news.getImage()))
                .placeholder(R.drawable.placeholder)
                .error(R.drawable.ic_image_error)
                .noFade()
                .into(viewHolder.ivImage);

        return convertView;
    }

    public void setNewses(List<News> newses) {
        this.newses = newses;
        notifyDataSetChanged();
    }

    private final class ViewHolder {
        ImageView ivImage;
        TextView txtTitle;

        ViewHolder(View v) {
            ivImage = (ImageView) v.findViewById(R.id.iv_news_image);
            txtTitle = (TextView) v.findViewById(R.id.txt_news_title);
        }
    }
}

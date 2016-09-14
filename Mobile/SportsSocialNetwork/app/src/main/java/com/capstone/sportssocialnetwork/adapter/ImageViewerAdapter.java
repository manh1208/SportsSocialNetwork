package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.net.Uri;
import android.support.annotation.NonNull;
import android.support.v4.view.ViewPager;
import android.view.Gravity;
import android.view.ViewGroup;

import com.alexvasilkov.gestures.Settings;
import com.alexvasilkov.gestures.commons.RecyclePagerAdapter;
import com.alexvasilkov.gestures.views.GestureImageView;
import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.utils.GestureSettingsSetupListener;
import com.squareup.picasso.Picasso;

/**
 * Created by ManhNV on 9/14/16.
 */
public class ImageViewerAdapter extends RecyclePagerAdapter<ImageViewerAdapter.ViewHolder> {
    private String[] images;
    private final ViewPager viewPager;
//    private final GestureSettingsSetupListener setupListener;
    private Context context;

    public ImageViewerAdapter(Context context,ViewPager pager, String[] images
                              ) {
        this.context = context;
        this.viewPager = pager;
        this.images = images;
//        this.setupListener = listener;
    }

    @Override
    public ViewHolder onCreateViewHolder(@NonNull ViewGroup container) {
        ViewHolder holder = new ViewHolder(container);
        holder.image.getController().enableScrollInViewPager(viewPager);
        return holder;
    }

    @Override
    public void onBindViewHolder(@NonNull ViewHolder holder, int position) {
//        if (setupListener != null) {
//            setupListener.onSetupGestureView(holder.image);
//        }
//        GlideHelper.loadResource(paintings[position].getImageId(), holder.image);
        Picasso.with(context).load(Uri.parse(images[position]))
                .placeholder(R.drawable.img_default_avatar)
                .error(R.drawable.placeholder)
                .into(holder.image);
    }

    @Override
    public int getCount() {
        return images.length;
    }

    public static GestureImageView getImage(RecyclePagerAdapter.ViewHolder holder) {
        return ((ViewHolder) holder).image;
    }

    static class ViewHolder extends RecyclePagerAdapter.ViewHolder {
        final GestureImageView image;

        ViewHolder(ViewGroup container) {
            super(new GestureImageView(container.getContext()));
            image = (GestureImageView) itemView;
            image.getController().getSettings()
                    .setMaxZoom(5f)
                    .setPanEnabled(true)
                    .setZoomEnabled(true)
                    .setDoubleTapEnabled(true)
                    .setOverscrollDistance(container.getContext(), 32f, 32f)
                    .setOverzoomFactor(Settings.OVERZOOM_FACTOR)
                    .setRotationEnabled(false)
                    .setRestrictRotation(false)
                    .setFillViewport(true)
                    .setFitMethod(Settings.Fit.INSIDE)
                    .setGravity(Gravity.CENTER);
        }
    }
}

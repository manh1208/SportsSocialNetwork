package com.capstone.sportssocialnetwork.adapter;

import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.ImageView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.ImageViewerActivity;
import com.capstone.sportssocialnetwork.activity.MainActivity;
import com.squareup.picasso.Picasso;
//import com.stfalcon.frescoimageviewer.ImageViewer;

/**
 * Created by ManhNV on 9/11/16.
 */
public class ImageAdapter extends BaseAdapter {
    private Context mContext;


    public ImageAdapter(Context c) {
        mContext = c;
    }

    public int getCount() {
        return mThumbIds.length;
    }

    public Object getItem(int position) {
        return null;
    }

    public long getItemId(int position) {
        return 0;
    }

    // create a new ImageView for each item referenced by the Adapter
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            // if it's not recycled, initialize some attributes
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_place_image,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }

//        viewHolder.imageView.setImageResource(mThumbIds[position]);

        Picasso.with(mContext).load(Uri.parse(mThumbIds[position]))
                .placeholder(R.drawable.img_default_avatar)
                .error(R.drawable.placeholder)
                .into(viewHolder.imageView);
        viewHolder.imageView.setTag(position);
        viewHolder.imageView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                int position = (int) view.getTag();
                Intent intent =  new Intent(mContext, ImageViewerActivity.class);
                intent.putExtra("position",position);
                intent.putExtra("listImage",mThumbIds);
                mContext.startActivity(intent);
//                Intent intent = new Intent(Intent.ACTION_VIEW);
//                Uri uri = Uri.parse(ContentResolver.SCHEME_ANDROID_RESOURCE +
//                        "://"+ mContext.getResources().getResourcePackageName(R.drawable.placeholder)+"/"+
//                                mContext.getResources().getResourceTypeName(R.drawable.placeholder) + "/"+
//                                mContext.getResources().getResourceEntryName(R.drawable.placeholder));
//                intent.setDataAndType(Uri.parse("http://e1.365dm.com/16/06/16-9/20/arsenal-fixtures-graphic-badge-sanchez_3484220.jpg"), "image/jpg");
//                mContext.startActivity(intent);
//                String videoUrl = "http://s71.stream.nixcdn.com/tvc/apple_cider_elder_flower_2207.mp4";
//                Intent i = new Intent(Intent.ACTION_VIEW);
//                i.setDataAndType(Uri.parse(videoUrl),"video/mp4");
//                mContext.startActivity(i);


            }
        });
        return convertView;
    }

    private class ViewHolder{
        ImageView imageView;
        ViewHolder (View view){
            imageView = (ImageView) view.findViewById(R.id.iv_place_image);
        }
    }


    // references to our images
    private String[] mThumbIds = {
            "http://talksport.com/sites/default/files/styles/large/public/field/image/201506/468580396.jpg",
            "http://www.101greatgoals.com/wp-content/uploads/2015/06/alexis-sanchez-thumbs-up.jpg",
            "http://cdn.unilad.co.uk/wp-content/uploads/2016/02/Sanchez-2.jpg",
            "http://aaj.tv/wp-content/uploads/2016/01/ramsey-ozil_2754407b.jpg",
            "http://www4.pictures.zimbio.com/gi/Alexis+Sanchez+Arsenal+v+Everton+Premier+League+VwmzJaR49HFl.jpg"
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder,
//            R.drawable.placeholder, R.drawable.placeholder
    };
}

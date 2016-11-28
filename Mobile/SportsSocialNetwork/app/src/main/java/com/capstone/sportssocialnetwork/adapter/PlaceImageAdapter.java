package com.capstone.sportssocialnetwork.adapter;

import android.content.ContentResolver;
import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.BaseAdapter;
import android.widget.ImageView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.ImageViewerActivity;
import com.capstone.sportssocialnetwork.activity.MainActivity;
import com.capstone.sportssocialnetwork.model.PlaceImage;
import com.capstone.sportssocialnetwork.model.PostImage;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.squareup.picasso.Picasso;

import java.util.List;
//import com.stfalcon.frescoimageviewer.ImageViewer;

/**
 * Created by ManhNV on 9/11/16.
 */
public class PlaceImageAdapter extends ArrayAdapter<PlaceImage> {
    private Context mContext;
    private List<PlaceImage> images;

    public PlaceImageAdapter(Context context, int resource, List<PlaceImage> objects) {
        super(context, resource, objects);
        mContext = context;
        images = objects;
    }

    public void setImages(List<PlaceImage> images) {
        this.images = images;
        notifyDataSetChanged();
    }

    public int getCount() {
        return images.size();
    }

    public PlaceImage getItem(int position) {
        return images.get(position);
    }

    public long getItemId(int position) {
        return images.get(position).getId();
    }

    // create a new ImageView for each item referenced by the Adapter
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            // if it's not recycled, initialize some attributes
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_place_image, parent, false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }

//        viewHolder.imageView.setImageResour   ce(mThumbIds[position]);
        PlaceImage image = getItem(position);
        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + image.getImage()))
                .placeholder(R.drawable.placeholder)
                .error(R.drawable.ic_image_error)
                .fit()
                .into(viewHolder.imageView);
        viewHolder.imageView.setTag(position);
        viewHolder.imageView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                String[] imageArray = new String[images.size()];
                int i = 0;
                for (PlaceImage image : images
                        ) {
                    imageArray[i++] = image.getImage();
                }
                int position = (int) view.getTag();
                Intent intent = new Intent(mContext, ImageViewerActivity.class);
                intent.putExtra("position", position);
                intent.putExtra("listImage", imageArray);
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

    private class ViewHolder {
        ImageView imageView;

        ViewHolder(View view) {
            imageView = (ImageView) view.findViewById(R.id.iv_place_image);
        }
    }


//    // references to our images
//    private String[] mThumbIds = {
//            "http://talksport.com/sites/default/files/styles/large/public/field/image/201506/468580396.jpg",
//            "http://www.101greatgoals.com/wp-content/uploads/2015/06/alexis-sanchez-thumbs-up.jpg",
//            "http://cdn.unilad.co.uk/wp-content/uploads/2016/02/Sanchez-2.jpg",
//            "http://aaj.tv/wp-content/uploads/2016/01/ramsey-ozil_2754407b.jpg",
//            "http://www4.pictures.zimbio.com/gi/Alexis+Sanchez+Arsenal+v+Everton+Premier+League+VwmzJaR49HFl.jpg"
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder,
////            R.drawable.placeholder, R.drawable.placeholder
//    };
}

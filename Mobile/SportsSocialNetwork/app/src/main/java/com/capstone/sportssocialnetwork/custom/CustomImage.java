package com.capstone.sportssocialnetwork.custom;

import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.drawable.BitmapDrawable;
import android.graphics.drawable.Drawable;
import android.util.AttributeSet;
import android.widget.ImageView;

/**
 * Created by ManhNV on 6/26/2016.
 */
public class CustomImage extends ImageView {
    private double mHeightRatio;
    private Context mContext;
    public CustomImage(Context context) {
        super(context);
        mContext = context;
    }

    public CustomImage(Context context, AttributeSet attrs) {
        super(context, attrs);
        mContext = context;
    }

    public CustomImage(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
        mContext = context;
    }

    public void setHeightRatio(double ratio) {
        if (ratio != mHeightRatio) {
            mHeightRatio = ratio;
            requestLayout();
        }
    }

    @Override
    protected void onMeasure(int widthMeasureSpec, int heightMeasureSpec) {


        Drawable drawable = getDrawable();
//        Uri uri = Uri.parse("android.resource://"+mContext.getPackageName()+drawable.toString());
//        File file = DataUtils.getImageFileFromUri(mContext,uri,DataUtils.getFilePath("test.jpg"),2000, Bitmap.CompressFormat.JPEG,50);
//        try {
//            ExifInterface exifInterface = new ExifInterface(file.getAbsolutePath());
//            int orientation = exifInterface.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_NORMAL);
//            Log.
//        } catch (IOException e) {
//            e.printStackTrace();
//        }

        if (drawable != null)
        {
            //get imageview width
            int width =  MeasureSpec.getSize(widthMeasureSpec);

            int diw = drawable.getIntrinsicWidth();
            int dih = drawable.getIntrinsicHeight();
            float ratio = (float)dih/diw; //get image aspect ratio

            int height = (int) (width * ratio);

            //don't let height exceed width
//            if (height > width){
//                height = width;
//            }


            setMeasuredDimension(width, height);
        }
        else
            super.onMeasure(widthMeasureSpec, heightMeasureSpec);

    }

    private double getRatioHeight(){
        BitmapFactory.Options dimensions = new BitmapFactory.Options();
        dimensions.inJustDecodeBounds = true;
        Bitmap mBitmapB = ((BitmapDrawable)this.getDrawable()).getBitmap();
        double height = dimensions.outHeight;
        double width =  dimensions.outWidth;
        double ratio = height/width;
        return ratio;//>0?ratio:height/width;
    }


}

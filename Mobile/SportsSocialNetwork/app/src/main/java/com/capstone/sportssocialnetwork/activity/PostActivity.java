package com.capstone.sportssocialnetwork.activity;

import android.Manifest;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.media.ExifInterface;
import android.os.Build;
import android.os.Bundle;
import android.provider.MediaStore;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ScrollView;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.CustomImage;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.io.File;
import java.io.IOException;

import okhttp3.MediaType;
import okhttp3.RequestBody;

public class PostActivity extends AppCompatActivity implements View.OnClickListener {
    private static final int SELECT_PICTURE = 2016;
    private ImageView ivAvatar;
    private TextView txtName;
    private TextView txtContent;
    private CustomImage ivContentImage;
    private ImageButton btnCamera;
    private RequestBody requestFile;
//    private TypedFile imageFile;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_post);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Đăng bài viết");
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        requestPermissionsImage();
        init();

    }

    private void requestPermissionsImage() {
//        String[] perms = {"android.permission.INTERNET","android.permission.WRITE_EXTERNAL_STORAGE","android.permission.READ_EXTERNAL_STORAGE"};
        int permsRequestCode = 200;
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            if (ActivityCompat.checkSelfPermission(this, Manifest.permission.WRITE_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
                String[] perms = {"android.permission.WRITE_EXTERNAL_STORAGE"};
                requestPermissions(perms, permsRequestCode);
            }
            if (ActivityCompat.checkSelfPermission(this, Manifest.permission.READ_EXTERNAL_STORAGE) != PackageManager.PERMISSION_GRANTED) {
                String[] perms = {"android.permission.READ_EXTERNAL_STORAGE"};
                requestPermissions(perms, permsRequestCode);
            }
        }
    }

    private void init(){
        ivAvatar = (ImageView) findViewById(R.id.iv_new_post_avatar);
        txtName = (TextView) findViewById(R.id.txt_new_post_name);
        ivContentImage = (CustomImage) findViewById(R.id.iv_new_post_image);
        txtContent = (TextView) findViewById(R.id.txt_new_post_content);
        btnCamera = (ImageButton) findViewById(R.id.btn_new_post_camera);
        btnCamera.setOnClickListener(this);
    }


    @Override
    public void onClick(View v) {
        int id = v.getId();
        switch (id) {
            case R.id.btn_new_post_camera:
                Intent pickIntent = new Intent();
                pickIntent.setType("image/*");
                pickIntent.setAction(Intent.ACTION_GET_CONTENT);
                Intent takePhotoIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
                String pickTitle = "Select or take a new Picture"; // Or get from strings.xml
                Intent chooserIntent = Intent.createChooser(pickIntent, pickTitle);
                chooserIntent.putExtra
                        (
                                Intent.EXTRA_INITIAL_INTENTS,
                                new Intent[]{takePhotoIntent}
                        );

                startActivityForResult(chooserIntent, SELECT_PICTURE);
                break;
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == SELECT_PICTURE && resultCode == Activity.RESULT_OK) {
            String filename = "Image" + System.currentTimeMillis() % 10000 + ".jpg";
            File f = Utilities.getImageFileFromUri(this, data.getData(), Utilities.getPicturePath(filename), 2000, Bitmap.CompressFormat.JPEG, 50);
//            ExifInterface exifInterface = null;
//            try {
//                exifInterface = new ExifInterface(f.getAbsolutePath());
//            } catch (IOException e) {
//                e.printStackTrace();
//            }
//            int orientation = exifInterface.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_NORMAL);
//            Bitmap bm;
//            try {
//                 bm = BitmapFactory.decodeStream(
//                        getContentResolver().openInputStream(data.getData()));
//                Log.d("haha","haha");
//            } catch (FileNotFoundException e) {
//                e.printStackTrace();
//            }
            try {
                int str = new ExifInterface(data.getData().getPath()).getAttributeInt("Orientation", 1000);
                Log.d("haha", "haha");
            } catch (IOException e) {
                e.printStackTrace();
            }
//            imageFile = new TypedFile("multipart/form-data", f);

            requestFile =
                    RequestBody.create(MediaType.parse("multipart/form-data"), f);
            ivContentImage.setImageURI(data.getData());
            ivContentImage.setVisibility(View.VISIBLE);
        }
    }
}

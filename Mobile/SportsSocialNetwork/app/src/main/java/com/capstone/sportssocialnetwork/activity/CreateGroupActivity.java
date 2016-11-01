package com.capstone.sportssocialnetwork.activity;

import android.Manifest;
import android.app.Activity;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.os.Build;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.app.ActivityCompat;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.PopupMenu;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.widget.ImageView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.io.File;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;

public class CreateGroupActivity extends AppCompatActivity implements View.OnClickListener {
    private static final int SELECT_PICTURE = 1900;
    private static final int TAKE_PICTURE = 1991;
    private RoundedImageView ivCamera;
    private RequestBody requestFile;
    private MultipartBody.Part body;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_group);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Tạo nhóm");
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        init();
    }

    private void init() {
        ivCamera = (RoundedImageView) findViewById(R.id.btn_camera);
        ivCamera.setOnClickListener(this);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_create_group,menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()){
            case R.id.menu_create_group:

                break;
        }
        return super.onOptionsItemSelected(item);
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
            if (ActivityCompat.checkSelfPermission(this, Manifest.permission.CAMERA) != PackageManager.PERMISSION_GRANTED) {
                String[] perms = {"android.permission.CAMERA"};
                requestPermissions(perms, permsRequestCode);
            }
        }
    }

    public void showPopupMenu(View v) {
        PopupMenu popupMenu = new PopupMenu(this, v);
        final MenuInflater inflater = popupMenu.getMenuInflater();
        inflater.inflate(R.menu.menu_image_popup, popupMenu.getMenu());
        popupMenu.show();
        if (body==null){
            popupMenu.getMenu().getItem(0).setVisible(false);
        }else{
            popupMenu.getMenu().getItem(0).setVisible(true);
        }
        popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
            @Override
            public boolean onMenuItemClick(MenuItem item) {
                int id = item.getItemId();
                requestPermissionsImage();
                switch (id) {
                    case R.id.menu_image_change:
                        startIntent();
                        return true;
                    case R.id.menu_image_delete:
                        requestFile = null;
                        body = null;
                        ivCamera.setImageResource(R.drawable.ic_camera_rounded);
                        return true;

                }
                return false;
            }
        });
    }

    private void startIntent(){
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
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == SELECT_PICTURE && data!=null && resultCode == Activity.RESULT_OK) {
            String filename = "Image" + System.currentTimeMillis() % 100000 + ".jpg";
            File f = Utilities.getImageFileFromUri(this, data.getData(), Utilities.getPicturePath(filename), DataUtils.MAX_SIZE_IMAGE, Bitmap.CompressFormat.JPEG, 50);

            requestFile =
                    RequestBody.create(MediaType.parse("multipart/form-data"), f);
            body =
                    MultipartBody.Part.createFormData("uploadimage", f.getName(), requestFile);

            ivCamera.setImageURI(data.getData());
        }

    }

    @Override
    public void onClick(View v) {
        int id=  v.getId();
        switch (id){
            case R.id.btn_camera:
                if (body!=null) {
                    showPopupMenu(v);
                }else{
                    startIntent();
                }
                break;
        }
    }
}

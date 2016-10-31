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
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.animation.AnimationUtils;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ScrollView;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.CustomImage;
import com.capstone.sportssocialnetwork.model.Post;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.io.File;
import java.io.IOException;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;
import retrofit2.http.POST;

public class PostActivity extends AppCompatActivity implements View.OnClickListener {
    private static final int SELECT_PICTURE = 2016;
    private ImageView ivAvatar;
    private TextView txtName;
    private TextView txtContent;
    private CustomImage ivContentImage;
    private ImageButton btnCamera;
    private RequestBody requestFile;
//    private TypedFile imageFile;
    private RestService service;
    private String userId;
    private MultipartBody.Part body;
    private String groupId;


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
        service = new RestService();
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        groupId = getIntent().getStringExtra("groupId");
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
            String filename = "Image" + System.currentTimeMillis() % 100000 + ".jpg";
            File f = Utilities.getImageFileFromUri(this, data.getData(), Utilities.getPicturePath(filename), DataUtils.MAX_SIZE_IMAGE, Bitmap.CompressFormat.JPEG, 50);
//            Toast.makeText(PostActivity.this, Utilities.getOrientation(this,data.getData())+"", Toast.LENGTH_SHORT).show();
//            ExifInterface exifInterface = null;
//            try {
//                exifInterface = new ExifInterface(f.getAbsolutePath());
//            } catch (IOException e) {
//                e.printStackTrace();
//            }
//            int orientation = exifInterface.getAttributeInt(ExifInterface.TAG_ORIENTATION, ExifInterface.ORIENTATION_NORMAL);
//            Toast.makeText(PostActivity.this, Utilities.exifToDegrees(orientation)+"", Toast.LENGTH_SHORT).show();
////            Bitmap bm;
////            try {
////                 bm = BitmapFactory.decodeStream(
////                        getContentResolver().openInputStream(data.getData()));
////                Log.d("haha","haha");
////            } catch (FileNotFoundException e) {
////                e.printStackTrace();
////            }
//
//            try {
//                int str = new ExifInterface(data.getData().getPath()).getAttributeInt("Orientation", 1000);
//                Log.d("haha", "haha");
//            } catch (IOException e) {
//                e.printStackTrace();
//            }
////            imageFile = new TypedFile("multipart/form-data", f);

            requestFile =
                    RequestBody.create(MediaType.parse("multipart/form-data"), f);
            body =
                    MultipartBody.Part.createFormData("uploadimage", f.getName(), requestFile);

            ivContentImage.setImageURI(data.getData());
            ivContentImage.setVisibility(View.VISIBLE);
            txtContent.setError(null);
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_new_post, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        switch (id){
            case R.id.menu_create_post:
                if (txtContent.getText().toString().equals("") && body==null){
                    txtContent.setError("Vui lòng điền nội dung");
                    txtContent.startAnimation(AnimationUtils.loadAnimation(this,R.anim.shake));
                    txtContent.requestFocus();
                    return true;
                }
                RequestBody userBody =
                        RequestBody.create(
                                MediaType.parse("multipart/form-data"), userId);
                RequestBody contentBody =
                        RequestBody.create(
                                MediaType.parse("multipart/form-data"), txtContent.getText().toString());
                RequestBody groupBody =
                        RequestBody.create(
                                MediaType.parse("multipart/form-data"), groupId+"");

                Call<ResponseModel<Post>> call =  service.getPostService()
                     .createPost(userBody,contentBody,body,groupBody);
                
                call.enqueue(new Callback<ResponseModel<Post>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<Post>> call, Response<ResponseModel<Post>> response) {
                        if (response.isSuccessful()){
                            if (response.body().isSucceed()){
                            onBackPressed();
//                                Toast.makeText(PostActivity.this, response.body().getMessage(), Toast.LENGTH_SHORT).show();
                            }else{
                                Toast.makeText(PostActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(PostActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<Post>> call, Throwable t) {
                        Toast.makeText(PostActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });
                return true;
        }
        return super.onOptionsItemSelected(item);
    }
}

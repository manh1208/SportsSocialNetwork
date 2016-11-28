package com.capstone.sportssocialnetwork.activity;

import android.Manifest;
import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
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
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.FieldType;
import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.model.Sport;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.io.File;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CreateGroupActivity extends AppCompatActivity implements View.OnClickListener {
    private static final int SELECT_PICTURE = 1900;
    private static final int TAKE_PICTURE = 1991;
    private RoundedImageView ivCamera;
    private RequestBody requestFile;
    private MultipartBody.Part body;
    private Spinner spGroupSport;
    private ArrayAdapter<String> sportAdapter;
    private ProgressDialog progressDialog;
    private RestService service;
    private HashMap<String,Sport> sportHash;
    private TextView txtGroupName;
    private TextView txtGroupDescription;
    private String userId;

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
        createSportSpinner();
    }

    private void init() {
        ivCamera = (RoundedImageView) findViewById(R.id.btn_camera);
        ivCamera.setOnClickListener(this);
        spGroupSport = (Spinner) findViewById(R.id.sp_group_sport);
        txtGroupName  = (TextView) findViewById(R.id.txt_create_group_name);
        txtGroupDescription  = (TextView) findViewById(R.id.txt_create_group_description);
        service = new RestService();
        sportHash = new HashMap<>();
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
    }

    private void createSportSpinner() {
        sportAdapter = new ArrayAdapter(this, R.layout.item_spinner, new ArrayList());
        sportAdapter.setDropDownViewResource(R.layout.item_spinner);
        spGroupSport.setAdapter(sportAdapter);
        spGroupSport.setSelection(0);
    }

    private void loadSport() {
        progressDialog = new ProgressDialog(this);
        progressDialog.setMessage("Đang tải loại sân...");
        progressDialog.show();
        Call<ResponseModel<List<Sport>>> call = service.getSocialNetworkService().getAllSport();
        call.enqueue(new Callback<ResponseModel<List<Sport>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Sport>>> call, Response<ResponseModel<List<Sport>>> response) {
                progressDialog.dismiss();
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        sportHash.clear();
                        for (Sport item : response.body().getData()
                                ) {
                            sportHash.put(item.getName(), item);
                        }
                        sportAdapter.clear();
                        sportAdapter.addAll(sportHash.keySet());
                        if (sportHash.size() > 0) {
                            spGroupSport.setSelection(0);
                        }
                    } else {
                        Toast.makeText(CreateGroupActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {

                    Toast.makeText(CreateGroupActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Sport>>> call, Throwable t) {
                progressDialog.dismiss();
                Toast.makeText(CreateGroupActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });

    }

    @Override
    protected void onResume() {
        super.onResume();
        loadSport();
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
                View focus = null;
                String name = txtGroupName.getText().toString();
                String description = txtGroupDescription.getText().toString();
                int sportId = sportHash.get((String) spGroupSport.getSelectedItem()).getId();
                if (name.equals("")){
                    txtGroupName.setError("Vui lòng nhập tên nhóm");
                    if (focus==null){
                        focus = txtGroupName;
                    }
                }
                if (description.equals("")){
                    txtGroupDescription.setError("Vui lòng nhập tên nhóm");
                    if (focus==null){
                        focus = txtGroupDescription;
                    }
                }

                if (focus!=null){
                    focus.requestFocus();
                }else{
                    RequestBody nameBody =
                            RequestBody.create(
                                    MediaType.parse("multipart/form-data"), name);
                    RequestBody descriptionBody =
                            RequestBody.create(
                                    MediaType.parse("multipart/form-data"), description);
                    RequestBody userIdBody =
                            RequestBody.create(
                                    MediaType.parse("multipart/form-data"), userId);
                    service.getGroupService()
                            .createGroup(nameBody,descriptionBody,sportId,userIdBody,body)
                    .enqueue(new Callback<ResponseModel<Group>>() {
                        @Override
                        public void onResponse(Call<ResponseModel<Group>> call, Response<ResponseModel<Group>> response) {
                            if (response.isSuccessful()) {
                                if (response.body().isSucceed()) {
                                    Intent intent = new Intent(CreateGroupActivity.this,MainBottomBarActivity.class);
                                    intent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP);
                                    startActivity(intent);
                                    finish();
                                } else {
                                    Toast.makeText(CreateGroupActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                                }
                            } else {
                                Toast.makeText(CreateGroupActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                            }
                        }

                        @Override
                        public void onFailure(Call<ResponseModel<Group>> call, Throwable t) {
                            Toast.makeText(CreateGroupActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
                        }
                    });
                    
                }
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
            Bitmap bitmap = BitmapFactory.decodeFile(f.getAbsolutePath());
            ivCamera.setImageBitmap(bitmap);
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

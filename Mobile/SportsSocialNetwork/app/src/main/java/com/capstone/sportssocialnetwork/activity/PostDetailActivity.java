package com.capstone.sportssocialnetwork.activity;

import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.media.ExifInterface;
import android.os.Build;
import android.os.Bundle;
import android.provider.MediaStore;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.PopupMenu;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.AbsListView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.CommentAdapter;
import com.capstone.sportssocialnetwork.custom.CustomImage;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Comment;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.response.PostDetailResponseModel;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import okhttp3.MediaType;
import okhttp3.MultipartBody;
import okhttp3.RequestBody;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class PostDetailActivity extends AppCompatActivity implements View.OnClickListener{
    private static final int SELECT_PICTURE = 1994;
    private static final String TAG = "PostDetailActivity";
    private ListView lvComment;
    private CommentAdapter adapter;
    private View header;
    private ViewHolder headerViewHolder;
    private RestService service;
    private int postId;
    private String userId;
    private static final int MAX_TAKE = 5;
    private int skip;
    private int take;
    private boolean isFull;
    private boolean flag_loading;
    private EditText txtComment;
    private ImageButton btnCamera;
    private ImageButton btnSent;
    private ImageView ivCommentImage;
    private ImageView ivDeleteImage;
    private RequestBody requestFile;
    private MultipartBody.Part body;
    private boolean isLiked;
    private int likeCount;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_post_detail);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Chi tiết");
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        initView();
        prepareData();
        event();
    }

    private void event() {
        txtComment.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {
                Log.e(TAG,s.toString());
                if (s.length()>0){
                    btnSent.setVisibility(View.VISIBLE);
                }else{
                    btnSent.setVisibility(View.GONE);
                }
            }

            @Override
            public void afterTextChanged(Editable s) {

            }
        });
        lvComment.setOnScrollListener(new AbsListView.OnScrollListener() {
            @Override
            public void onScrollStateChanged(AbsListView view, int scrollState) {
            }

            @Override
            public void onScroll(AbsListView view, int firstVisibleItem, int visibleItemCount, int totalItemCount) {
                if (firstVisibleItem + visibleItemCount >= totalItemCount-1 && totalItemCount >1) {
                    if (!flag_loading && !isFull) {
                        loadData();
                    }
                }
            }
        });
    }

    private void loadData() {
        flag_loading = true;
        Call<ResponseModel<List<Comment>>> call =  service.getPostService()
                .getComment(postId,skip,take);
        call.enqueue(new Callback<ResponseModel<List<Comment>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Comment>>> call, Response<ResponseModel<List<Comment>>> response) {
                flag_loading =false;
                if(response.isSuccessful())
                {
                    if (response.body().isSucceed()){
                        if (response.body().getData()!=null &&
                                response.body().getData().size()>0) {
                            adapter.setAppendFeed(response.body().getData());
                            if (adapter.getCount() < (skip + take)) {
                                isFull = true;
                            }

                            skip = skip + take;
                        }else{
                            isFull = true;
                        }
                    }else{
                        Toast.makeText(PostDetailActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(PostDetailActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Comment>>> call, Throwable t) {
                flag_loading=false;
                Toast.makeText(PostDetailActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void prepareData() {

        adapter = new CommentAdapter(this,R.layout.item_comment,new ArrayList<Comment>());
        lvComment.addHeaderView(header);
        lvComment.setAdapter(adapter);
        init();
    }

    private void initView() {
        postId = getIntent().getIntExtra("postId",-1);
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        lvComment = (ListView) findViewById(R.id.lv_comment);
        txtComment = (EditText) findViewById(R.id.txt_post_comment);
        btnCamera = (ImageButton) findViewById(R.id.btn_post_comment_image);
        btnSent = (ImageButton) findViewById(R.id.btn_post_comment_write);
        ivCommentImage = (ImageView) findViewById(R.id.iv_write_comment_image);
        ivDeleteImage = (ImageView) findViewById(R.id.iv_write_comment_delete);
        btnCamera.setOnClickListener(this);
        ivDeleteImage.setOnClickListener(this);
        btnSent.setOnClickListener(this);
        header= ((LayoutInflater)
                getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                R.layout.item_feed, null, false);
        headerViewHolder = new ViewHolder(header);
        service = new RestService();
        ivCommentImage.setVisibility(View.GONE);
        ivDeleteImage.setVisibility(View.GONE);
        btnSent.setVisibility(View.GONE);

    }

    private void init(){
        skip = 0;
        take = MAX_TAKE;
        flag_loading = false;
        isFull = false;
        txtComment.setText("");
        txtComment.clearFocus();
        InputMethodManager imm = (InputMethodManager) getSystemService(Context.INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(txtComment.getWindowToken(), InputMethodManager.HIDE_NOT_ALWAYS);
        adapter.addNew();
    }

    private void loadNewData(){
        init();
        flag_loading = true;
        Call<ResponseModel<PostDetailResponseModel>> call =  service.getPostService()
                                                            .getPostDetail(postId,userId,skip,take);
        call.enqueue(new Callback<ResponseModel<PostDetailResponseModel>>() {
            @Override
            public void onResponse(Call<ResponseModel<PostDetailResponseModel>> call, Response<ResponseModel<PostDetailResponseModel>> response) {
                flag_loading =false;
                if(response.isSuccessful())
                {
                    if (response.body().isSucceed()){
                        updateHeaderUI(response.body().getData().getPost());
                        if (response.body().getData().getComments()!=null &&
                                response.body().getData().getComments().size()>0) {
                            adapter.setAppendFeed(response.body().getData().getComments());
                            if (adapter.getCount() < (skip + take)) {
                                isFull = true;
                            }

                            skip = skip + take;
                        }else{
                            isFull = true;
                        }
                    }else{
                        Toast.makeText(PostDetailActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(PostDetailActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<PostDetailResponseModel>> call, Throwable t) {
                flag_loading=false;
                Toast.makeText(PostDetailActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
        
    }

    @Override
    protected void onResume() {
        super.onResume();
        loadNewData();
    }

    private void updateHeaderUI(Feed feed) {
        headerViewHolder.txtNumOfLike.setText(feed.getLikeCount() + "");
        headerViewHolder.txtNumOfComment.setText(feed.getCommentCount() + " bình luận");
        if (feed.isLiked()) {
            setLiked(headerViewHolder.btnLike);
            isLiked=true;
        } else {
            setUnLiked(headerViewHolder.btnLike);
            isLiked=false;
        }
        likeCount = feed.getLikeCount();
        headerViewHolder.txtName.setText(feed.getUser().getFullName());
        headerViewHolder.txtContent.setText(feed.getPostContent());
        if (feed.getImage() != null) {
            headerViewHolder.ivImage.setVisibility(View.VISIBLE);
        } else {
            headerViewHolder.ivImage.setVisibility(View.GONE);
        }
        if (userId.equals(feed.getUser().getId())){
            headerViewHolder.btnMenuPopUp.setVisibility(View.VISIBLE);
        }else{
            headerViewHolder.btnMenuPopUp.setVisibility(View.GONE);
        }
        headerViewHolder.btnMenuPopUp.setTag(feed);
        headerViewHolder.btnMenuPopUp.setOnClickListener(this);
        headerViewHolder.btnComment.setTag(feed);
        headerViewHolder.btnComment.setOnClickListener(this);
        headerViewHolder.btnLike.setTag(feed);
        headerViewHolder.btnLike.setOnClickListener(this);

    }

    @Override
    public void onClick(final View v) {
        int id = v.getId();
        Feed feed = (Feed) v.getTag();
        switch (id) {
            case R.id.btn_feed_like:
                headerViewHolder.btnLike.setEnabled(false);
                Call<ResponseModel<String>> callLike = service.getPostService().likePost(postId,userId);
                callLike.enqueue(new Callback<ResponseModel<String>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                        headerViewHolder.btnLike.setEnabled(true);
                        if(response.isSuccessful()){
                            if (response.body().isSucceed()){
                                isLiked = !isLiked;
                                if (isLiked) {
                                    setLiked((Button) v);
                                    likeCount++;
                                    headerViewHolder.txtNumOfLike.setText(likeCount+"");
                                }else{
                                    setUnLiked((Button)v);
                                    likeCount--;
                                    headerViewHolder.txtNumOfLike.setText(likeCount+"");
                                }

                            }else{
                                Toast.makeText(PostDetailActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(PostDetailActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                        headerViewHolder.btnLike.setEnabled(true);
                        Toast.makeText(PostDetailActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });

                break;
            case R.id.btn_feed_comment:
                txtComment.requestFocus();
                break;

            case R.id.btn_feed_menu_down:
                showPopupMenu(v);
                break;

            case R.id.txt_feed_name:

                break;
            case R.id.btn_post_comment_image:
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
            case R.id.iv_write_comment_delete:
                body = null;
                ivCommentImage.setVisibility(View.GONE);
                ivDeleteImage.setVisibility(View.GONE);
                break;
            case R.id.btn_post_comment_write:
                RequestBody postId =
                        RequestBody.create(
                                MediaType.parse("multipart/form-data"), String.valueOf(this.postId));
                RequestBody userBody =
                        RequestBody.create(
                                MediaType.parse("multipart/form-data"), this.userId);
                RequestBody content =
                        RequestBody.create(
                                MediaType.parse("multipart/form-data"), txtComment.getText().toString());
                Call<ResponseModel<Comment>> call =  service.getPostService().commentPost(postId,userBody,content,body);
                call.enqueue(new Callback<ResponseModel<Comment>>() {
                    @Override
                    public void onResponse(Call<ResponseModel<Comment>> call, Response<ResponseModel<Comment>> response) {
                        if (response.isSuccessful()){
                            if (response.body().isSucceed()){
                                loadNewData();
                            }else{
                                Toast.makeText(PostDetailActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                            }
                        }else{
                            Toast.makeText(PostDetailActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                        }
                    }

                    @Override
                    public void onFailure(Call<ResponseModel<Comment>> call, Throwable t) {
                        Toast.makeText(PostDetailActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
                    }
                });
                break;
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == SELECT_PICTURE && resultCode == Activity.RESULT_OK) {
            String filename = "Image" + System.currentTimeMillis() % 10000 + ".jpg";
            File f = Utilities.getImageFileFromUri(this, data.getData(), Utilities.getPicturePath(filename), 2000, Bitmap.CompressFormat.JPEG, 50);

            try {
                int str = new ExifInterface(data.getData().getPath()).getAttributeInt("Orientation", 1000);
                Log.d("haha", "haha");
            } catch (IOException e) {
                e.printStackTrace();
            }
//            imageFile = new TypedFile("multipart/form-data", f);

            requestFile =
                    RequestBody.create(MediaType.parse("multipart/form-data"), f);
            body =
                    MultipartBody.Part.createFormData("image", f.getName(), requestFile);

            ivCommentImage.setImageURI(data.getData());
            ivCommentImage.setVisibility(View.VISIBLE);
            ivDeleteImage.setVisibility(View.VISIBLE);
        }
    }

    public void showPopupMenu(View v) {
        PopupMenu popupMenu = new PopupMenu(this, v);
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

    @TargetApi(Build.VERSION_CODES.LOLLIPOP)
    private void setLiked(Button btnLike) {

        Drawable[] drawables = btnLike.getCompoundDrawables();
        drawables[0].setTint(getResources().getColor(R.color.colorPrimary));
        btnLike.setTextColor(getResources().getColor(R.color.colorPrimary));
        btnLike.setCompoundDrawablesRelativeWithIntrinsicBounds(drawables[0], drawables[1], drawables[2], drawables[3]);

    }

    @TargetApi(Build.VERSION_CODES.LOLLIPOP)
    private void setUnLiked(Button btnLike) {
        Drawable[] drawables = btnLike.getCompoundDrawables();
        drawables[0].setTint(Color.BLACK);
        btnLike.setTextColor(Color.BLACK);
        btnLike.setCompoundDrawablesRelativeWithIntrinsicBounds(drawables[0], drawables[1], drawables[2], drawables[3]);
    }

    private final class ViewHolder {
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

        public ViewHolder(View v) {
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

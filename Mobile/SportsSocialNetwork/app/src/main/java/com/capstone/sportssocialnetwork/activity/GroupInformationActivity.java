package com.capstone.sportssocialnetwork.activity;

import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.squareup.picasso.Picasso;

import org.w3c.dom.Text;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

import static com.capstone.sportssocialnetwork.R.id.txt_group_name;

public class GroupInformationActivity extends AppCompatActivity implements View.OnClickListener {
    private int groupId;
    private String groupName;
    private RoundedImageView ivGroupAvatar;
    private TextView txtGroupName;
    private TextView txtGroupType;
    private TextView txtGroupDescription;
    private Group group;
    private RestService service;
    private String userId;
    private Button btnChangeCover;
    private Button btnChangeAvatar;
    private Button btnGroupMember;
    private Button btnLeaveGroup;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_group_information);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
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
        groupId = getIntent().getIntExtra("groupId", -1);
        groupName = getIntent().getStringExtra("groupName");
        getSupportActionBar().setTitle("Thông tin nhóm");
        ivGroupAvatar = (RoundedImageView) findViewById(R.id.iv_group_avatar);
        txtGroupName = (TextView) findViewById(txt_group_name);
        txtGroupType = (TextView) findViewById(R.id.txt_group_type);
        txtGroupDescription = (TextView) findViewById(R.id.txt_group_description);
        group = new Group();
        service = new RestService();
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
        btnChangeCover = (Button) findViewById(R.id.btn_group_chane_avatar);
        btnChangeAvatar = (Button) findViewById(R.id.btn_group_change_cover);
        btnGroupMember = (Button) findViewById(R.id.btn_group_member);
        btnLeaveGroup = (Button) findViewById(R.id.btn_group_leave_group);
        btnChangeCover.setOnClickListener(this);
        btnChangeAvatar.setOnClickListener(this);
        btnGroupMember.setOnClickListener(this);
        btnLeaveGroup.setOnClickListener(this);
    }

    private void updateUI() {
        txtGroupName.setText(groupName);
        txtGroupDescription.setText(group.getDescription());
        txtGroupType.setText(group.getSportId() + "");
        if (group.isAdmin()){
            btnChangeAvatar.setVisibility(View.VISIBLE);
            btnChangeCover.setVisibility(View.VISIBLE);
        }else{
            btnChangeAvatar.setVisibility(View.GONE);
            btnChangeCover.setVisibility(View.GONE);
        }
    }

    private void getGroupProfile() {
        service.getGroupService().getGroupDetail(groupId, userId).enqueue(new Callback<ResponseModel<Group>>() {
            @Override
            public void onResponse(Call<ResponseModel<Group>> call, Response<ResponseModel<Group>> response) {
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        group = response.body().getData();
                        updateUI();
                    } else {
                        Toast.makeText(GroupInformationActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(GroupInformationActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<Group>> call, Throwable t) {
                Toast.makeText(GroupInformationActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    protected void onResume() {
        super.onResume();
        getGroupProfile();
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        switch (id) {
            case R.id.btn_group_change_cover:

                break;
            case R.id.btn_group_chane_avatar:

                break;
            case R.id.btn_group_member:
                Intent intent =  new Intent(GroupInformationActivity.this, GroupMemberActivity.class);
                intent.putExtra("groupId",groupId);
                intent.putExtra("groupName",groupName);
                startActivity(intent);
                break;
            case R.id.btn_group_leave_group:

                break;
        }
    }
}

package com.capstone.sportssocialnetwork.activity;

import android.content.Intent;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.enumerable.RoleEnum;
import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.model.User;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.google.firebase.iid.FirebaseInstanceId;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class LoginActivity extends AppCompatActivity implements View.OnClickListener {
    private TextView txtUsername;
    private TextView txtPassword;
    private Button btnLogin;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        initView();
        if (DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "").length() > 0) {
            sendRegistrationToServer();
            if (DataUtils.getINSTANCE(this).getPreferences().getInt(SharePreferentName.SHARE_USER_ROLE,-1)==RoleEnum.MEMBER.getValue()){
                Intent intent = new Intent(LoginActivity.this, MainBottomBarActivity.class);
                startActivity(intent);
                finish();
            }else  if (DataUtils.getINSTANCE(this).getPreferences().getInt(SharePreferentName.SHARE_USER_ROLE,-1)==RoleEnum.PLACEOWNER.getValue()){
                Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                startActivity(intent);
                finish();
            }

        }

    }

    private void initView() {
        txtUsername = (TextView) findViewById(R.id.txt_login_username);
        txtPassword = (TextView) findViewById(R.id.txt_login_password);
        btnLogin = (Button) findViewById(R.id.btn_login);
        btnLogin.setOnClickListener(this);
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        switch (id) {
            case R.id.btn_login:
                final String username = txtUsername.getText().toString();
                String password = txtPassword.getText().toString();
                View focus = null;
                if (username.equals("")) {
                    Animation shake = AnimationUtils.loadAnimation(
                            LoginActivity.this, R.anim.shake);
                    txtUsername.startAnimation(shake);
                    txtUsername.setError("Xin nhập tên tài khoản");
                    focus = txtUsername;
                }

                if (password.equals("")) {
                    Animation shake = AnimationUtils.loadAnimation(
                            LoginActivity.this, R.anim.shake);
                    txtPassword.startAnimation(shake);
                    txtPassword.setError("Xin nhập tên mật khẩu");
                    if (focus == null) {
                        focus = txtUsername;
                    }
                }
                if (focus != null) {
                    focus.requestFocus();
                } else {
                    RestService restService = new RestService();
                    Call<ResponseModel<User>> call = restService.getAccountService().login(username, password);
                    call.enqueue(new Callback<ResponseModel<User>>() {
                        @Override
                        public void onResponse(Call<ResponseModel<User>> call, Response<ResponseModel<User>> response) {
                            if (response.isSuccessful()) {
                                if (response.body().isSucceed()) {
                                    User user = response.body().getData();
                                    if (user.getRole().getId().equalsIgnoreCase(RoleEnum.MEMBER.getValue() + ""))
                                    {

                                        DataUtils.getINSTANCE(LoginActivity.this)
                                                .getPreferences()
                                                .edit()
                                                .putString(SharePreferentName.SHARE_USER_ID, user.getId())
                                                .putInt(SharePreferentName.SHARE_USER_ROLE, Integer.parseInt(user.getRole().getId()))
                                                .putString(SharePreferentName.SHARE_USER_FULLNAME,user.getFullName())
                                                .putString(SharePreferentName.SHARE_USER_AVATAR,user.getAvatar())
                                                .commit();
                                        sendRegistrationToServer();
                                        Intent intent = new Intent(LoginActivity.this, MainBottomBarActivity.class);
                                        startActivity(intent);
                                        finish();
                                    }else  if (user.getRole().getId().equalsIgnoreCase(RoleEnum.PLACEOWNER.getValue() + "")){
                                        DataUtils.getINSTANCE(LoginActivity.this)
                                                .getPreferences()
                                                .edit()
                                                .putString(SharePreferentName.SHARE_USER_ID,user.getId())
                                                .putInt(SharePreferentName.SHARE_USER_ROLE, Integer.parseInt(user.getRole().getId()))
                                                .putString(SharePreferentName.SHARE_USER_FULLNAME,user.getFullName())
                                                .putString(SharePreferentName.SHARE_USER_AVATAR,user.getAvatar())
                                                .apply();
                                        sendRegistrationToServer();
                                        Intent intent = new Intent(LoginActivity.this, MainActivity.class);
                                        startActivity(intent);
                                        finish();
                                    }
                                } else {
                                    Toast.makeText(LoginActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                                }
                            }
                        }

                        @Override
                        public void onFailure(Call<ResponseModel<User>> call, Throwable t) {
                            Toast.makeText(LoginActivity.this, "Can not connect to server", Toast.LENGTH_SHORT).show();
                        }
                    });
                    //checkLogin at server;

                }
                break;
        }
    }

    private void sendRegistrationToServer() {
        String refreshedToken = FirebaseInstanceId.getInstance().getToken();
        String userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        RestService service = new RestService();
        service.getAccountService().sentToken(userId,refreshedToken).enqueue(new Callback<ResponseModel<String>>() {
            @Override
            public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        Log.d("Noti", response.body().getErrorsString());
                    }else{
                        Log.d("Noti", response.body().getErrorsString());
                    }
                }else{

                }
            }

            @Override
            public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                Log.d("Noti", getString(R.string.failure));
            }
        });
    }
}

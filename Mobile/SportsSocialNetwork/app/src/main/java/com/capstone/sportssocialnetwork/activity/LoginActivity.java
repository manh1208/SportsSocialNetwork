package com.capstone.sportssocialnetwork.activity;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.AnimationUtils;
import android.widget.Button;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;

public class LoginActivity extends AppCompatActivity implements View.OnClickListener{
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
        switch (id){
            case R.id.btn_login:
                String username = txtUsername.getText().toString();
                String password = txtPassword.getText().toString();
                View focus = null;
                if (username.equals("")){
                    Animation shake = AnimationUtils.loadAnimation(
                            LoginActivity.this, R.anim.shake);
                    txtUsername.startAnimation(shake);
                    txtUsername.setError("Xin nhập tên tài khoản");
                    focus = txtUsername;
                }

                if (password.equals("")){
                    Animation shake = AnimationUtils.loadAnimation(
                            LoginActivity.this, R.anim.shake);
                    txtPassword.startAnimation(shake);
                    txtPassword.setError("Xin nhập tên mật khẩu");
                    if (focus==null) {
                        focus = txtUsername;
                    }
                }
                if (focus!=null){
                    focus.requestFocus();
                }else{
                    //checkLogin at server;
                    Intent intent = new Intent(LoginActivity.this,MainBottomBarActivity.class);
                    startActivity(intent);
                }
                break;
        }
    }
}

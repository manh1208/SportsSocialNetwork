package com.capstone.sportssocialnetwork.activity;

import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;
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
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        switch (id){
            case R.id.btn_login:

                break;
        }
    }
}

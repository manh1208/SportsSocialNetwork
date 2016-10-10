package com.capstone.sportssocialnetwork.activity;

import android.content.Intent;
import android.os.Handler;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;

import com.capstone.sportssocialnetwork.R;

public class SplashActivity extends AppCompatActivity {


    private static final long SPLASH_TIME_OUT = 3000;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_splash);
//        FacebookSdk.sdkInitialize(getApplicationContext());
        new Handler().postDelayed(new Runnable() {
            @Override
            public void run() {
                Intent i = new Intent(getApplicationContext(), LoginActivity .class);
                startActivity(i);
//                overridePendingTransition(R.anim.right_in, R.anim.left_out);
                finish();
            }
        },SPLASH_TIME_OUT);
    }
}

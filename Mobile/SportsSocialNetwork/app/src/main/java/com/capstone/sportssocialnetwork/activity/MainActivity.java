package com.capstone.sportssocialnetwork.activity;

import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.CoordinatorLayout;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.support.design.widget.NavigationView;
import android.support.v4.view.GravityCompat;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBarDrawerToggle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.animation.AnimationUtils;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.fragment.ManageOrderFragment;
import com.capstone.sportssocialnetwork.fragment.ManagePlaceFragment;
import com.capstone.sportssocialnetwork.model.CheckIn;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.google.zxing.Result;

import java.text.ParseException;
import java.util.Date;

import me.dm7.barcodescanner.zxing.ZXingScannerView;
import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;


public class MainActivity extends AppCompatActivity
        implements NavigationView.OnNavigationItemSelectedListener, ZXingScannerView.ResultHandler {
    private Toolbar toolbar;
    private DrawerLayout drawerLayout;
    private CoordinatorLayout mainLayout;
    private ZXingScannerView mScannerView;
    private boolean isStartCamera;
    private RestService service;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        initialView();
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Quản lý");
        ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(
                this, drawerLayout, toolbar, R.string.navigation_drawer_open, R.string.navigation_drawer_close) {
            @Override
            public void onDrawerSlide(View drawerView, float slideOffset) {
                super.onDrawerSlide(drawerView, slideOffset);

                mainLayout.setTranslationX(slideOffset * drawerView.getWidth());
                drawerLayout.bringChildToFront(drawerView);
                drawerLayout.requestLayout();
            }
        };
        drawerLayout.setDrawerListener(toggle);
        toggle.syncState();

        NavigationView navigationView = (NavigationView) findViewById(R.id.nav_view);
        navigationView.setNavigationItemSelectedListener(this);
    }

    private void initialView() {
        service  = new RestService();
        toolbar = (Toolbar) findViewById(R.id.toolbar);
        drawerLayout = (DrawerLayout) findViewById(R.id.drawer_layout);
        mainLayout = (CoordinatorLayout) findViewById(R.id.app_main);
    }

    @Override
    public void onBackPressed() {

        if (isStartCamera) {
            recreate();
        } else {
            DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
            if (drawer.isDrawerOpen(GravityCompat.START)) {
                drawer.closeDrawer(GravityCompat.START);
            } else {
                super.onBackPressed();
            }
        }
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.menu_checkin) {
//            Intent intent = new Intent(MainActivity.this,CheckInActivity.class);
//            startActivity(intent);
            doCheckIn();
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    private void doCheckIn() {
        String[] title = new String[]{"Quét QR Code", "Nhập mã"};
        final AlertDialog.Builder buider = new AlertDialog.Builder(MainActivity.this);
        buider.setTitle("Chọn cách nhập").setItems(title, new DialogInterface.OnClickListener() {
            @Override
            public void onClick(final DialogInterface dialog, int which) {
//                        Toast.makeText(ParticipantsActivity.this, ""+which, Toast.LENGTH_SHORT).show();
                switch (which) {
                    case 0:
                        mScannerView = new ZXingScannerView(MainActivity.this);   // Programmatically initialize the scanner view
                        setContentView(mScannerView);
                        mScannerView.setResultHandler(MainActivity.this); // Register ourselves as a handler for scan results.
                        mScannerView.startCamera();
                        isStartCamera = true;
                        break;
                    case 1:
                        AlertDialog.Builder inputCodeBuilder = new AlertDialog.Builder(MainActivity.this);
                        View dialogView = LayoutInflater.from(MainActivity.this).inflate(R.layout.dialog_input_code, null);
                        final EditText txtCode = (EditText) dialogView.findViewById(R.id.txt_your_code);
                        isStartCamera = true;
                        inputCodeBuilder.setView(dialogView)
                                .setNegativeButton("Cancel", null)
                                .setPositiveButton("Xong", null);
                        final AlertDialog dialog1 = inputCodeBuilder.create();
                        dialog1.setOnShowListener(new DialogInterface.OnShowListener() {
                            @Override
                            public void onShow(DialogInterface dialog2) {
                                Button b = dialog1.getButton(AlertDialog.BUTTON_POSITIVE);
                                b.setOnClickListener(new View.OnClickListener() {
                                    @Override
                                    public void onClick(View v) {
                                        String c = txtCode.getText().toString();
                                        View focus = null;
                                        if (c.equalsIgnoreCase("")) {
                                            txtCode.setError("Vui lòng nhập mã");
                                            txtCode.startAnimation(AnimationUtils.loadAnimation(MainActivity.this, R.anim.shake));
                                            focus = txtCode;
                                        }
                                        if (focus != null) {
                                            focus.requestFocus();
                                        } else {
//                                            int code = Integer.parseInt(c);
                                            sendCode(c);
                                        }
                                    }
                                });
                            }
                        });
                        dialog1.show();
                        break;
                }
            }
        }).create().show();

    }

    @Override
    public void handleResult(Result result) {
        Log.e("handler", result.getText()); // Prints scan results
        Log.e("handler", result.getBarcodeFormat().toString()); // Prints the scan format (qrcode)

        // show the scanner result into dialog box.
//        AlertDialog.Builder builder = new AlertDialog.Builder(this);
//        builder.setTitle("Scan Result");
//        builder.setMessage(result.getText());
//        AlertDialog alert1 = builder.create();
//        alert1.show();
        try {
//            int code = Integer.parseInt(result.getText());
//            Toast.makeText(MainActivity.this, result.getText(), Toast.LENGTH_SHORT).show();
            sendCode(result.getText());
        } catch (Exception e) {
            Toast.makeText(MainActivity.this, "QR Code không hợp lệ", Toast.LENGTH_SHORT).show();
            recreate();
        }

    }

    private void sendCode(String code) {
        Call<ResponseModel<CheckIn>> call = service.getOrderService().checkInOrder(code);

        call.enqueue(new Callback<ResponseModel<CheckIn>>() {
            @Override
            public void onResponse(Call<ResponseModel<CheckIn>> call, Response<ResponseModel<CheckIn>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        CheckIn order = response.body().getData();
                        AlertDialog.Builder buider = new AlertDialog.Builder(MainActivity.this);
                        View v = getLayoutInflater().inflate(R.layout.dialog_order_info, null, false);
                        TextView name = (TextView) v.findViewById(R.id.txt_order_detail_fullname);
                        name.setText(order.getFullName());
                        TextView createDate = (TextView) v.findViewById(R.id.txt_order_detail_create_time);
                        createDate.setText(order.getCreateDate());
                        TextView place = (TextView) v.findViewById(R.id.txt_order_detail_place);
                        place.setText(order.getPlaceName());
                        TextView field = (TextView) v.findViewById(R.id.txt_order_detail_field);
                        field.setText(order.getFieldName());
                        TextView useDate = (TextView) v.findViewById(R.id.txt_order_detail_use_date);
                        try {
                            Date date = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
                            useDate.setText( Utilities.getDateTimeString(date,"dd/MM/yyyy"));
                        } catch (ParseException e) {
                            Toast.makeText(MainActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
                        }
                        TextView startTime = (TextView) v.findViewById(R.id.txt_order_detail_start_time);
                        try {
                            Date date = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
                            startTime.setText( Utilities.getDateTimeString(date,"hh:mm a"));
                        } catch (ParseException e) {
                            Toast.makeText(MainActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
                        }


                        TextView endTime = (TextView) v.findViewById(R.id.txt_order_detail_end_time);
                        try {
                            Date date = Utilities.getDateTime(order.getEndTime(),"MM/dd/yyyy hh:mm:ss a");
                            endTime.setText( Utilities.getDateTimeString(date,"hh:mm a"));
                        } catch (ParseException e) {
                            Toast.makeText(MainActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
                        }
                        TextView payment = (TextView) v.findViewById(R.id.txt_order_detail_payment);
                        payment.setText(order.getPaidType());
                        TextView status = (TextView) v.findViewById(R.id.txt_order_detail_order_status);
                        status.setText(order.getStatus());
                        buider.setView(v)
                                .setNegativeButton("OK", new DialogInterface.OnClickListener() {
                                    @Override
                                    public void onClick(DialogInterface dialog, int which) {
                                        onBackPressed();
                                    }
                                })
                                .create()
                                .show();
                    }else{
                        Toast.makeText(MainActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                        if (response.body().getData()!=null){
                            CheckIn order = response.body().getData();
                            AlertDialog.Builder buider = new AlertDialog.Builder(MainActivity.this);
                            View v = getLayoutInflater().inflate(R.layout.dialog_order_info, null, false);
                            TextView name = (TextView) v.findViewById(R.id.txt_order_detail_fullname);
                            name.setText(order.getFullName());
                            TextView createDate = (TextView) v.findViewById(R.id.txt_order_detail_create_time);
                            createDate.setText(order.getCreateDate());
                            TextView place = (TextView) v.findViewById(R.id.txt_order_detail_place);
                            place.setText(order.getPlaceName());
                            TextView field = (TextView) v.findViewById(R.id.txt_order_detail_field);
                            field.setText(order.getFieldName());
                            TextView useDate = (TextView) v.findViewById(R.id.txt_order_detail_use_date);
                            try {
                                Date date = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
                                useDate.setText( Utilities.getDateTimeString(date,"dd/MM/yyyy"));
                            } catch (ParseException e) {
                                Toast.makeText(MainActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
                            }
                            TextView startTime = (TextView) v.findViewById(R.id.txt_order_detail_start_time);
                            try {
                                Date date = Utilities.getDateTime(order.getStartTime(),"MM/dd/yyyy hh:mm:ss a");
                                startTime.setText( Utilities.getDateTimeString(date,"hh:mm a"));
                            } catch (ParseException e) {
                                Toast.makeText(MainActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
                            }


                            TextView endTime = (TextView) v.findViewById(R.id.txt_order_detail_end_time);
                            try {
                                Date date = Utilities.getDateTime(order.getEndTime(),"MM/dd/yyyy hh:mm:ss a");
                                endTime.setText( Utilities.getDateTimeString(date,"hh:mm a"));
                            } catch (ParseException e) {
                                Toast.makeText(MainActivity.this, "Lỗi parse", Toast.LENGTH_SHORT).show();
                            }
                            TextView payment = (TextView) v.findViewById(R.id.txt_order_detail_payment);
                            payment.setText(order.getPaidType());
                            TextView status = (TextView) v.findViewById(R.id.txt_order_detail_order_status);
                            status.setText(order.getStatus());
                            buider.setView(v)
                                    .setNegativeButton("OK", new DialogInterface.OnClickListener() {
                                        @Override
                                        public void onClick(DialogInterface dialog, int which) {
                                            onBackPressed();
                                        }
                                    })
                                    .create()
                                    .show();
                        }
//                        AlertDialog.Builder builder = new AlertDialog.Builder(MainActivity.this);
//                        builder.setTitle("Warning")
//                                .setMessage(response.body().getErrorsString())
//                                .setPositiveButton("Ok", new DialogInterface.OnClickListener() {
//                                    @Override
//                                    public void onClick(DialogInterface dialog, int which) {
//                                        onBackPressed();
//                                    }
//                                })
//                                .create().show();
                    }
                }else{
                    Toast.makeText(MainActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<CheckIn>> call, Throwable t) {
                Toast.makeText(MainActivity.this, "Loi ket noi voi server", Toast.LENGTH_SHORT).show();
            }
        });

    }

    @SuppressWarnings("StatementWithEmptyBody")
    @Override
    public boolean onNavigationItemSelected(MenuItem item) {
        // Handle navigation view item clicks here.
        int id = item.getItemId();
        Fragment fragment = null;
        switch (id) {
            case R.id.nav_manage_place:
                fragment = new ManagePlaceFragment();
                getSupportActionBar().setTitle("Quản lý địa điểm");
                break;
            case R.id.nav_manage_order:
                fragment = new ManageOrderFragment();
                getSupportActionBar().setTitle("Quản lý đơn đặt sân");
                break;
            case R.id.nav_manage_logout:
                DataUtils.getINSTANCE(this).getPreferences().edit().clear().commit();
                Intent intent = new Intent(MainActivity.this,LoginActivity.class);
                intent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP);
                startActivity(intent);
                finish();
                break;
        }
        if (fragment != null) {
            getSupportFragmentManager().beginTransaction()
                    .replace(R.id.frame, fragment)
                    .commit();
        }


        DrawerLayout drawer = (DrawerLayout) findViewById(R.id.drawer_layout);
        drawer.closeDrawer(GravityCompat.START);
        return true;
    }

}

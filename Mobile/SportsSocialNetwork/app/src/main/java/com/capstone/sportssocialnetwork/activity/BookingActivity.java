package com.capstone.sportssocialnetwork.activity;

import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.view.View;
import android.view.WindowManager;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class BookingActivity extends AppCompatActivity {
    private Spinner spPlace;
    private Spinner spSport;
    private Spinner spField;
    private EditText txtUseDate;
    private EditText txtStartTime;
    private EditText txtEndTime;
    private TextView txtPrice;
    private String txtPlaceName;
    private int placeId;
    private String userId;
    private int fieldId;
    private String startTime;
    private String endTime;
    private RestService service;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_booking);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Đặt sân");
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
//        this.getWindow().setSoftInputMode(WindowManager.LayoutParams.SOFT_INPUT_STATE_ALWAYS_HIDDEN);
        initView();
        event();
    }

    private void event() {
        spField.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                Toast.makeText(BookingActivity.this, "tinh", Toast.LENGTH_SHORT).show();
                calculatePrice();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {

            }
        });
        txtStartTime.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                startTime = s.toString();
                Toast.makeText(BookingActivity.this, "Tinh", Toast.LENGTH_SHORT).show();
                calculatePrice();
            }
        });


        txtEndTime.addTextChangedListener(new TextWatcher() {
            @Override
            public void beforeTextChanged(CharSequence s, int start, int count, int after) {

            }

            @Override
            public void onTextChanged(CharSequence s, int start, int before, int count) {

            }

            @Override
            public void afterTextChanged(Editable s) {
                endTime = s.toString();
                Toast.makeText(BookingActivity.this, "Tinh", Toast.LENGTH_SHORT).show();
                calculatePrice();
            }
        });
    }

    private void initView() {
        txtPlaceName = getIntent().getStringExtra("placeName");
        placeId = getIntent().getIntExtra("placeId",-1);
        userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
        spPlace = (Spinner) findViewById(R.id.sp_booking_place);
        spSport = (Spinner) findViewById(R.id.sp_booking_sport);
        spField = (Spinner) findViewById(R.id.sp_booking_field);
        txtUseDate = (EditText) findViewById(R.id.txt_booking_use_date);
        txtUseDate.clearFocus();
        txtStartTime = (EditText) findViewById(R.id.txt_booking_start_time);
        txtStartTime.clearFocus();
        txtEndTime = (EditText) findViewById(R.id.txt_booking_end_time);
        txtEndTime.clearFocus();
        txtPrice = (TextView) findViewById(R.id.txt_booking_price);
        createPlaceSpinner();
        createSportSpinner();
        createFieldSpinner();
        Utilities.setDateField(this, txtUseDate, "dd/MM/yyyy");
        Utilities.setTimeField(this, txtStartTime, "HH:mm");
        Utilities.setTimeField(this, txtEndTime, "HH:mm");
        service = new RestService();
        fieldId = 1010;
    }



    private void createPlaceSpinner() {
        List<String> sports = new ArrayList<String>();
        sports.add(txtPlaceName);
        ArrayAdapter<String> arrayAdapter = new ArrayAdapter(this, R.layout.item_spinner, sports);
        arrayAdapter.setDropDownViewResource(R.layout.item_spinner);
        spPlace.setAdapter(arrayAdapter);
        spPlace.setSelection(0);
    }

    private void createSportSpinner() {
        List<String> sports = new ArrayList<String>();
        sports.add("Tất cả");
        sports.add("Bóng đá");
        sports.add("Bóng rổ");
        sports.add("Bóng chuyền");
        ArrayAdapter<String> arrayAdapter = new ArrayAdapter(this, R.layout.item_spinner, sports);
        arrayAdapter.setDropDownViewResource(R.layout.item_spinner);
        spSport.setAdapter(arrayAdapter);
        spSport.setSelection(0);
    }

    private void createFieldSpinner() {
        List<String> sports = new ArrayList<String>();
        sports.add("Tất cả");
        sports.add("Bóng đá");
        sports.add("Bóng rổ");
        sports.add("Bóng chuyền");
        ArrayAdapter<String> arrayAdapter = new ArrayAdapter(this, R.layout.item_spinner, sports);
        arrayAdapter.setDropDownViewResource(R.layout.item_spinner);
        spField.setAdapter(arrayAdapter);
        spField.setSelection(0);
    }

    private void calculatePrice(){
        if (fieldId==-1) return;
        if (startTime==null ||startTime.equals("")) return;
        if (endTime ==null || endTime.equals("")) return;
        Call<ResponseModel<Long>> call = service.getOrderService().getPrice(fieldId,startTime,endTime);
        call.enqueue(new Callback<ResponseModel<Long>>() {
            @Override
            public void onResponse(Call<ResponseModel<Long>> call, Response<ResponseModel<Long>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        txtPrice.setText(response.body().getData()+"");
                    }else{
                        Toast.makeText(BookingActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(BookingActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<Long>> call, Throwable t) {
                Toast.makeText(BookingActivity.this, "Loi ket noi voi server", Toast.LENGTH_SHORT).show();
            }
        });

    }

}

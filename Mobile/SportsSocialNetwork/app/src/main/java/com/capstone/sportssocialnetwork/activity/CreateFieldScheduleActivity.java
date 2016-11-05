package com.capstone.sportssocialnetwork.activity;

import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.animation.AnimationUtils;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.enumerable.FieldScheduleTypeEnum;
import com.capstone.sportssocialnetwork.model.Field;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.Utilities;

import java.util.ArrayList;
import java.util.Date;
import java.util.EnumSet;
import java.util.HashMap;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class CreateFieldScheduleActivity extends AppCompatActivity {

    private Spinner spType;
    private Spinner spField;
    private EditText txtStartTime;
    private EditText txtEndTime;
    private EditText txtDescription;
    private int placeId;
    private RestService service;
    private ArrayAdapter<String> fieldAdapter;
    private HashMap<String, Integer> fieldHash;
    private ArrayAdapter<String> typeAdapter;
    private HashMap<String, Integer> typeHash;
    private ProgressDialog progressDialog;
    private List<FieldScheduleTypeEnum> fieldScheduleTypeEnumList;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_create_field_schedule);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        getSupportActionBar().setTitle("Tạo lịch");
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        initView();

    }

    private void initView() {
        spType = (Spinner) findViewById(R.id.sp_field_schedule_type);
        spField = (Spinner) findViewById(R.id.sp_field_schedule_field);
        txtStartTime = (EditText) findViewById(R.id.txt_field_schedule_start_time);
        txtStartTime.clearFocus();
        txtEndTime = (EditText) findViewById(R.id.txt_field_schedule_end_time);
        txtEndTime.clearFocus();
        txtDescription = (EditText) findViewById(R.id.txt_field_Schedule_description);
        txtDescription.clearFocus();
        createFieldSpinner();
        progressDialog = new ProgressDialog(this);
        placeId = getIntent().getIntExtra("placeId", -1);
        EnumSet<FieldScheduleTypeEnum> set = EnumSet.allOf(FieldScheduleTypeEnum.class);
        fieldScheduleTypeEnumList = new ArrayList<>(set);
        createTypeSpinner();
        Utilities.setDatetimeField(this, txtStartTime, "dd/MM/yyyy", "HH:mm:ss");
        Utilities.setDatetimeField(this, txtEndTime, "dd/MM/yyyy", "HH:mm:ss");

    }

    private void createFieldSpinner() {
        fieldAdapter = new ArrayAdapter(this, R.layout.item_spinner, new ArrayList());
        fieldAdapter.setDropDownViewResource(R.layout.item_spinner);
        spField.setAdapter(fieldAdapter);
        spField.setSelection(0);
        service = new RestService();
    }

    private void createTypeSpinner() {
        typeAdapter = new ArrayAdapter(this, R.layout.item_spinner, new ArrayList());
        typeAdapter.setDropDownViewResource(R.layout.item_spinner);
        spType.setAdapter(typeAdapter);
        spType.setSelection(0);
        service = new RestService();
    }

    private void loadField() {
        if (fieldHash == null) {
            fieldHash = new HashMap<>();
        } else {
            fieldHash.clear();
        }
        progressDialog.setMessage("Đang tải dữ liệu sân");
        progressDialog.show();
        Call<ResponseModel<List<Field>>> call = service.getPlaceService().getFieldOfPlace(placeId);
        call.enqueue(new Callback<ResponseModel<List<Field>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Field>>> call, Response<ResponseModel<List<Field>>> response) {
                progressDialog.dismiss();
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        for (Field item : response.body().getData()
                                ) {
                            fieldHash.put(item.getName(), item.getId());
                        }
                        fieldAdapter.clear();
                        fieldAdapter.addAll(fieldHash.keySet());
                    } else {
                        Toast.makeText(CreateFieldScheduleActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(CreateFieldScheduleActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Field>>> call, Throwable t) {
                progressDialog.dismiss();
                Toast.makeText(CreateFieldScheduleActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_field_schedule, menu);
        return super.onCreateOptionsMenu(menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        switch (id) {
            case R.id.menu_create_field_schedule:
                if (isValidation()) {

                }

                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    private void loadType() {
        if (typeHash == null) {
            typeHash = new HashMap<>();
        } else {
            typeHash.clear();
        }
        for (FieldScheduleTypeEnum item : fieldScheduleTypeEnumList
                ) {
            typeHash.put(item.toString(), item.getValue());
        }
        typeAdapter.clear();
        typeAdapter.addAll(typeHash.keySet());

    }

    @Override
    protected void onResume() {
        super.onResume();
        loadField();
        loadType();
    }

    public boolean isValidation() {
        txtStartTime.setError(null);
        txtEndTime.setError(null);
        try {
            if (txtStartTime.getText().toString() == null || txtStartTime.getText().toString().equals("")) {
                txtStartTime.setError("Vui lòng chọn thời gian bắt đầu");
                txtStartTime.startAnimation(AnimationUtils.loadAnimation(this, R.anim.shake));
                return false;
            }
            if (txtEndTime.getText().toString() == null || txtEndTime.getText().toString().equals("")) {
                txtEndTime.setError("Vui lòng chọn thời gian kết thúc");
                txtEndTime.startAnimation(AnimationUtils.loadAnimation(this, R.anim.shake));
                return false;
            }
//            Log.e(TAG, "Choose Date" + Date.parse(txtUseDate.getText().toString()) + " - " + new Date().toString());
//            Log.e(TAG, "Current Date" + (new java.util.Date()).getTime() + " - " + new Date(Date.parse(txtUseDate.getText().toString())));

            Date currentDate = Utilities.getZeroSecondTimeDate(new Date());
            Date startTime = Utilities.getDateTime(txtStartTime.getText().toString(), DataUtils.FORMAT_DATE_TIME);
            Date endTime = Utilities.getDateTime(txtEndTime.getText().toString(), DataUtils.FORMAT_DATE_TIME);
            if (startTime.before(currentDate)) {
                txtStartTime.setError("Ngày sử dụng phải lớn hơn ngày hiện tại");
                txtStartTime.startAnimation(AnimationUtils.loadAnimation(this, R.anim.shake));
                return false;
            }

            if (startTime.compareTo(endTime) >= 0) {
                txtEndTime.setError("Ngày kết thúc phải lớn hơn ngày bắt đầu");
                txtEndTime.startAnimation(AnimationUtils.loadAnimation(this, R.anim.shake));
                return false;
            }


        } catch (Exception e) {
            Toast.makeText(CreateFieldScheduleActivity.this, "Lỗi cú pháp", Toast.LENGTH_SHORT).show();
            return false;
        }

        return true;
    }
}

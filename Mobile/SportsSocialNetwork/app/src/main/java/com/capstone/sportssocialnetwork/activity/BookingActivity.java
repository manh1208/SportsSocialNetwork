package com.capstone.sportssocialnetwork.activity;

import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.net.Uri;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AlertDialog;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.text.Editable;
import android.text.TextWatcher;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.view.WindowManager;
import android.view.animation.AnimationUtils;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.EditText;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RadioButton;
import android.widget.RadioGroup;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.Enumerable.OrderStatusEnum;
import com.capstone.sportssocialnetwork.Enumerable.PaidTypeEnum;
import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Field;
import com.capstone.sportssocialnetwork.model.FieldType;
import com.capstone.sportssocialnetwork.model.Order;
import com.capstone.sportssocialnetwork.model.Place;
import com.capstone.sportssocialnetwork.model.request.OrderRequestModel;
import com.capstone.sportssocialnetwork.model.response.PlaceResponseModel;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.squareup.picasso.Picasso;

import java.sql.Time;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.Date;
import java.util.HashMap;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

public class BookingActivity extends AppCompatActivity {
    private static final String TAG = "BookingActivity";
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
    private ArrayAdapter<String> placeAdapter;
    private ArrayAdapter<String> sportAdapter;
    private ArrayAdapter<String> fieldAdapter;
    private HashMap<String, Integer> placeHash;
    private HashMap<String, Integer> sportHash;
    private HashMap<String, Integer> fieldHash;
    private int placeIdSelected;
    private LinearLayout layoutTime;
    private LinearLayout layoutPrice;
    private ProgressDialog progressDialog;


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
        loadPlace();
    }

    private void event() {
        spField.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
//                Toast.makeText(BookingActivity.this, "tinh", Toast.LENGTH_SHORT).show();
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
//                Toast.makeText(BookingActivity.this, "Tinh", Toast.LENGTH_SHORT).show();
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
//                Toast.makeText(BookingActivity.this, "Tinh", Toast.LENGTH_SHORT).show();
                calculatePrice();
            }
        });

        spSport.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                String text = sportAdapter.getItem(position);
                if (sportHash.containsKey(text)) {
                    loadField(placeIdSelected, sportHash.get(text));
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
//                Toast.makeText(BookingActivity.this, "sport no choice", Toast.LENGTH_SHORT).show();
            }
        });
        spPlace.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
//                Toast.makeText(BookingActivity.this, "Place was selected", Toast.LENGTH_SHORT).show();
                String text = placeAdapter.getItem(position);

                if (placeHash.containsKey(text)) {
                    placeIdSelected = placeHash.get(text);
                    loadFieldType(placeIdSelected);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
            }
        });

        spField.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                visibleTime();
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                invisibleTime();
            }
        });
    }

    private void initView() {
        txtPlaceName = getIntent().getStringExtra("placeName");
        placeId = getIntent().getIntExtra("placeId", -1);
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
        layoutTime = (LinearLayout) findViewById(R.id.layout_booking_time);
        layoutPrice = (LinearLayout) findViewById(R.id.layout_booking_price);
        createPlaceSpinner();
        createSportSpinner();
        createFieldSpinner();
        Utilities.setDateField(this, txtUseDate, "dd/MM/yyyy");
        Utilities.setTimeField(this, txtStartTime, "HH:mm");
        Utilities.setTimeField(this, txtEndTime, "HH:mm");
        service = new RestService();
        progressDialog = new ProgressDialog(this);
    }

    private void visibleTime() {
        layoutPrice.setVisibility(View.VISIBLE);
        layoutTime.setVisibility(View.VISIBLE);
        txtUseDate.setText("");
        txtStartTime.setText("");
        txtEndTime.setText("");
        txtPrice.setText("");
    }

    private void invisibleTime() {
        layoutPrice.setVisibility(View.GONE);
        layoutTime.setVisibility(View.GONE);
    }

    private void createPlaceSpinner() {
//        List<String> sports = new ArrayList<String>();
//        sports.add(txtPlaceName);
        placeAdapter = new ArrayAdapter(this, R.layout.item_spinner, new ArrayList());
        placeAdapter.setDropDownViewResource(R.layout.item_spinner);
        spPlace.setAdapter(placeAdapter);
        spPlace.setSelection(0);
    }

    private void createSportSpinner() {
//        List<String> sports = new ArrayList<String>();
//        sports.add("Tất cả");
//        sports.add("Bóng đá");
//        sports.add("Bóng rổ");
//        sports.add("Bóng chuyền");
        sportAdapter = new ArrayAdapter(this, R.layout.item_spinner, new ArrayList());
        sportAdapter.setDropDownViewResource(R.layout.item_spinner);
        spSport.setAdapter(sportAdapter);
        spSport.setSelection(0);
    }

    private void createFieldSpinner() {
//        List<String> sports = new ArrayList<String>();
//        sports.add("Tất cả");
//        sports.add("Bóng đá");
//        sports.add("Bóng rổ");
//        sports.add("Bóng chuyền");
        fieldAdapter = new ArrayAdapter(this, R.layout.item_spinner, new ArrayList());
        fieldAdapter.setDropDownViewResource(R.layout.item_spinner);
        spField.setAdapter(fieldAdapter);
        spField.setSelection(0);
    }

    private void loadPlace() {
        if (placeHash == null) {
            placeHash = new HashMap<>();
        } else {
            placeHash.clear();
        }
        placeHash.put(txtPlaceName, placeId);
        placeAdapter.addAll(placeHash.keySet());
        progressDialog.setMessage("Đang tài dữ liệu địa điểm");
        progressDialog.show();
        Call<ResponseModel<List<PlaceResponseModel>>> callback = service.getPlaceService().getAllPlace(0, 100);
        callback.enqueue(new Callback<ResponseModel<List<PlaceResponseModel>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<PlaceResponseModel>>> call, Response<ResponseModel<List<PlaceResponseModel>>> response) {
                progressDialog.dismiss();
                if (response.isSuccessful()) {
                    ResponseModel<List<PlaceResponseModel>> responseModel = response.body();
                    if (responseModel.isSucceed()) {
                        for (PlaceResponseModel item : response.body().getData()
                                ) {
                            placeHash.put(item.getName(), item.getId());
                        }
                        placeAdapter.clear();
                        placeAdapter.addAll(placeHash.keySet());
                        for (int i = 0; i < placeAdapter.getCount(); i++) {
                            if (txtPlaceName.equalsIgnoreCase(placeAdapter.getItem(i))) {
                                spPlace.setSelection(i);
                            }
                        }
                    } else {
                        Toast.makeText(BookingActivity.this, responseModel.getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(BookingActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<PlaceResponseModel>>> call, Throwable t) {
                progressDialog.dismiss();
                Toast.makeText(BookingActivity.this, "Lỗi kết nối server", Toast.LENGTH_SHORT).show();
            }
        });


    }

    private void loadFieldType(int placeId) {
        if (sportHash == null) {
            sportHash = new HashMap<>();
        } else {
            sportHash.clear();
        }
        progressDialog.setMessage("Đang tải loại sân...");
        progressDialog.show();
        Call<ResponseModel<List<FieldType>>> call = service.getPlaceService().getFieldType(placeId);
        call.enqueue(new Callback<ResponseModel<List<FieldType>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<FieldType>>> call, Response<ResponseModel<List<FieldType>>> response) {
                progressDialog.dismiss();
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        for (FieldType item : response.body().getData()
                                ) {
                            sportHash.put(item.getName(), item.getId());
                        }
                        sportAdapter.clear();
                        fieldAdapter.clear();
                        sportAdapter.addAll(sportHash.keySet());
                        if (sportHash.size() > 0) {
                            spSport.setSelection(0);
                            if (sportHash.containsKey(spSport.getSelectedItem().toString())) {
                                loadField(placeIdSelected, sportHash.get(spSport.getSelectedItem().toString()));
                            }
                        }
                    } else {
                        Toast.makeText(BookingActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {

                    Toast.makeText(BookingActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<FieldType>>> call, Throwable t) {
                progressDialog.dismiss();
                Toast.makeText(BookingActivity.this, "Loi server", Toast.LENGTH_SHORT).show();
            }
        });

    }

    private void loadField(int placeId, int fieldTypeId) {
        if (fieldHash == null) {
            fieldHash = new HashMap<>();
        } else {
            fieldHash.clear();
        }
        progressDialog.setMessage("Đang tải dữ liệu sân");
        progressDialog.show();
        Call<ResponseModel<List<Field>>> call = service.getPlaceService().getFieldByFieldType(placeId, fieldTypeId);
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
                        Toast.makeText(BookingActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(BookingActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Field>>> call, Throwable t) {
                progressDialog.dismiss();
                Toast.makeText(BookingActivity.this, "Loi server", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void calculatePrice() {
        fieldId = fieldHash.get(spField.getSelectedItem().toString());
        if (fieldId == -1) return;
        if (startTime == null || startTime.equals("")) return;
        if (endTime == null || endTime.equals("")) return;
        if (!isValidation()) return;
        progressDialog.setMessage("Đang tính giá...");
        progressDialog.show();
        Call<ResponseModel<Double>> call = service.getOrderService().getPrice(fieldId, startTime, endTime);
        call.enqueue(new Callback<ResponseModel<Double>>() {
            @Override
            public void onResponse(Call<ResponseModel<Double>> call, Response<ResponseModel<Double>> response) {
                progressDialog.dismiss();
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        txtPrice.setText(response.body().getData().longValue() + "");
                    } else {
                        Toast.makeText(BookingActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(BookingActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<Double>> call, Throwable t) {
                progressDialog.dismiss();
                Toast.makeText(BookingActivity.this, "Loi ket noi voi server", Toast.LENGTH_SHORT).show();
            }
        });

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_booking, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        switch (id) {
            case R.id.menu_booking:
                if (isValidation()) {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    View v = getLayoutInflater().inflate(R.layout.dialog_confirm_booking, null, false);
                    final RadioGroup group = (RadioGroup) v.findViewById(R.id.rbt_paid_type);
                    RadioButton cash = (RadioButton) v.findViewById(R.id.rbt_cash);
                    cash.setChecked(true);
                    TextView txtPlace = (TextView) v.findViewById(R.id.txt_order_confirm_place);
                    txtPlace.setText(spPlace.getSelectedItem().toString());
                    TextView txtField = (TextView) v.findViewById(R.id.txt_order_confirm_field);
                    txtField.setText(spField.getSelectedItem().toString());
                    TextView txtDate = (TextView) v.findViewById(R.id.txt_order_confirm_use_date);
                    txtDate.setText(this.txtUseDate.getText().toString());
                    final TextView txtStartTime = (TextView) v.findViewById(R.id.txt_order_confirm_start_time);
                    txtStartTime.setText(this.txtStartTime.getText().toString());
                    final TextView txtEndTime = (TextView) v.findViewById(R.id.txt_order_confirm_end_time);
                    txtEndTime.setText(this.txtEndTime.getText().toString());
                    TextView txtPrice = (TextView) v.findViewById(R.id.txt_order_confirm_price);
                    txtPrice.setText(this.txtPrice.getText().toString());
                    builder.setView(v)
                            .setPositiveButton("OK", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {


                                }
                            })
                            .setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
                                @Override
                                public void onClick(DialogInterface dialog, int which) {

                                }
                            });
                    final AlertDialog dialog = builder.create();
                    dialog.show();
                    dialog.getButton(DialogInterface.BUTTON_POSITIVE).setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            try {
                                int fieldId = fieldHash.get(spField.getSelectedItem().toString());
                                String startTime = txtUseDate.getText().toString() + " " + txtStartTime.getText().toString();
                                Date startTimeDate = Utilities.getDateTime(startTime, "dd/MM/yyyy HH:mm");
                                String start = Utilities.getDateTimeString(startTimeDate, "yyyy/MM/dd HH:mm:ss");
                                String endTime = txtUseDate.getText().toString() + " " + txtEndTime.getText().toString();
                                Date endTimeDate = Utilities.getDateTime(endTime, "dd/MM/yyyy HH:mm");
                                String end = Utilities.getDateTimeString(endTimeDate, "yyyy/MM/dd HH:mm:ss");
                                int paidType = PaidTypeEnum.ChosePayByCash.getValue();
                                switch (group.getCheckedRadioButtonId()) {
                                    case R.id.rbt_cash:
                                        paidType = PaidTypeEnum.ChosePayByCash.getValue();
                                        break;
                                    case R.id.rbt_online:
                                        paidType = PaidTypeEnum.ChosePayOnline.getValue();
                                        break;
                                }

                                OrderRequestModel model = new OrderRequestModel(userId, fieldId, start, end, "", "", paidType);
                                progressDialog.setMessage("Đang lưu dữ liệu đơn hàng. Vui lòng đợi...");
                                progressDialog.show();
                                Call<ResponseModel<Order>> call = service.getOrderService().createOrder(model);
                                call.enqueue(new Callback<ResponseModel<Order>>() {
                                    @Override
                                    public void onResponse(Call<ResponseModel<Order>> call, Response<ResponseModel<Order>> response) {
                                        progressDialog.dismiss();
                                        if (response.isSuccessful()) {
                                            if (response.body().isSucceed()) {
                                                dialog.dismiss();
                                                showOrderDetail(response.body().getData());
                                            } else {
                                                Toast.makeText(BookingActivity.this, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                                            }
                                        } else {
                                            Toast.makeText(BookingActivity.this, response.message(), Toast.LENGTH_SHORT).show();
                                        }
                                    }

                                    @Override
                                    public void onFailure(Call<ResponseModel<Order>> call, Throwable t) {
                                        progressDialog.dismiss();
                                        Toast.makeText(BookingActivity.this, R.string.failure, Toast.LENGTH_SHORT).show();
                                    }
                                });

                            } catch (ParseException e) {
                                Toast.makeText(BookingActivity.this, "Thời gian không hợp lệ", Toast.LENGTH_SHORT).show();
                            }
                        }
                    });
                }

                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    private void showOrderDetail(Order order) {
        AlertDialog.Builder buider = new AlertDialog.Builder(this);
        View view = LayoutInflater.from(this).inflate(R.layout.dialog_my_order_detail, null, false);

        TextView txtTitle = (TextView) view.findViewById(R.id.txt_order_detail_title);
        txtTitle.setText("Đặt sân thành công");
        ImageView ivQR = (ImageView) view.findViewById(R.id.iv_qr_code);

        Picasso.with(this).load(Uri.parse(DataUtils.URL + order.getqRCodeUrl()))
                .placeholder(R.drawable.image_logo)
                .error(R.drawable.img_default_avatar)
                .into(ivQR);
//                Toast.makeText(mContext, (DataUtils.URL+order.getqRCodeUrl()), Toast.LENGTH_SHORT).show();

        TextView useDate = (TextView) view.findViewById(R.id.txt_order_detail_use_date);
        try {
            Date date = Utilities.getDateTime(order.getStartTime(), "MM/dd/yyyy hh:mm:ss a");
            useDate.setText(Utilities.getDateTimeString(date, "dd/MM/yyyy"));
        } catch (ParseException e) {
            Toast.makeText(this, "Lỗi parse", Toast.LENGTH_SHORT).show();
        }

        TextView place = (TextView) view.findViewById(R.id.txt_order_detail_place);
        place.setText(order.getPlaceName());
        TextView field = (TextView) view.findViewById(R.id.txt_order_detail_field);
        field.setText(order.getFieldName());
        TextView startTime = (TextView) view.findViewById(R.id.txt_order_detail_start_time);
        try {
            Date date = Utilities.getDateTime(order.getStartTime(), "MM/dd/yyyy hh:mm:ss a");
            startTime.setText(Utilities.getDateTimeString(date, "hh:mm a"));
        } catch (ParseException e) {
            Toast.makeText(this, "Lỗi parse", Toast.LENGTH_SHORT).show();
        }


        TextView endTime = (TextView) view.findViewById(R.id.txt_order_detail_end_time);
        try {
            Date date = Utilities.getDateTime(order.getEndTime(), "MM/dd/yyyy hh:mm:ss a");
            endTime.setText(Utilities.getDateTimeString(date, "hh:mm a"));
        } catch (ParseException e) {
            Toast.makeText(this, "Lỗi parse", Toast.LENGTH_SHORT).show();
        }

        TextView price = (TextView) view.findViewById(R.id.txt_order_detail_price);
        price.setText(order.getPrice().longValue() + "");

        TextView payment = (TextView) view.findViewById(R.id.txt_order_detail_payment);
        payment.setText(PaidTypeEnum.fromInteger(order.getPaidType()).toString());
        TextView status = (TextView) view.findViewById(R.id.txt_order_detail_order_status);
        status.setText(OrderStatusEnum.fromInteger(order.getStatus()).toString());
        buider.setView(view)
                .setNegativeButton("Đi dến lịch sử", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        Intent intent = new Intent(BookingActivity.this, MyOrderActivity.class);
                        startActivity(intent);
                        finish();
                    }
                })
                .setPositiveButton("Quay về", new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        dialog.dismiss();
                        onBackPressed();
                    }
                })
                .create().show();
    }

    public boolean isValidation() {
        try {
            if (txtUseDate.getText().toString() == null || txtUseDate.getText().toString().equals("")) {
                txtUseDate.setError("Vui lòng chọn ngày sử dụng sân");
                txtUseDate.startAnimation(AnimationUtils.loadAnimation(this, R.anim.shake));
                return false;
            }
            Log.e(TAG, "Choose Date" + Date.parse(txtUseDate.getText().toString()) + " - " + new Date().toString());
            Log.e(TAG, "Current Date" + (new java.util.Date()).getTime() + " - " + new Date(Date.parse(txtUseDate.getText().toString())));

            Date currentDate = Utilities.getZeroTimeDate(new Date());
            Date useDate = Utilities.getDateTime(txtUseDate.getText().toString(), "dd/MM/yyyy");

            if (useDate.before(currentDate)) {
                txtUseDate.setError("Ngày sử dụng phải lớn hơn ngày hiện tại");
                txtUseDate.startAnimation(AnimationUtils.loadAnimation(this, R.anim.shake));
                return false;
            }

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
            Date starttime = Utilities.getDateTime(txtStartTime.getText().toString(), "HH:mm");
            Date endtime = Utilities.getDateTime(txtEndTime.getText().toString(), "HH:mm");
            if (starttime.compareTo(endtime) >= 0) {
                txtEndTime.setError("Giờ kết thúc phải lớn hơn giờ bắt đầu");
                txtEndTime.startAnimation(AnimationUtils.loadAnimation(this, R.anim.shake));
                return false;
            }
//            if (Time.valueOf(txtStartTime.getText().toString()).after(Time.valueOf(txtEndTime.getText().toString())) ) {
//                txtEndTime.setError("Giờ kết thúc phải lớn hơn giờ bắt đầu");
//                txtEndTime.startAnimation(AnimationUtils.loadAnimation(this, R.anim.shake));
//                return false;
//            }
        } catch (Exception e) {
            Toast.makeText(BookingActivity.this, "Lỗi cú pháp", Toast.LENGTH_SHORT).show();
            return false;
        }

        return true;
    }
}

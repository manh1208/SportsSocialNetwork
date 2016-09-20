package com.capstone.sportssocialnetwork.activity;

import android.app.DatePickerDialog;
import android.content.Context;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.DatePicker;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Locale;

public class ProfileDetailActivity extends AppCompatActivity {
    private Button btnAddFavouriteSport;
    private Spinner spCity,spDistrict,spWard,spGender;
    private DatePickerDialog datePickerDialog;
    private TextView txtBirthday;
    private SimpleDateFormat dateFormatter;


    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_profile_detail);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        toolbar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                onBackPressed();
            }
        });
        getSupportActionBar().setDisplayShowHomeEnabled(true);
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);
        initView();
        createDatePicker();
        prepareData();
        event();


    }
    @Override
    public boolean onPrepareOptionsMenu(Menu menu) {
        return super.onPrepareOptionsMenu(menu);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_save_profile_changes, menu);
        return super.onCreateOptionsMenu(menu);
    }

    private void initView() {
        LayoutInflater inflater = (LayoutInflater)getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        View v = inflater.inflate(R.layout.content_profile_detail,null);
        ViewGroup container = (ViewGroup) findViewById(R.id.container_profile_detail);
        container.addView(v);
        btnAddFavouriteSport= (Button) container.findViewById(R.id.btn_add_favourite_sport);
        spCity = (Spinner) container.findViewById(R.id.sp_city);
        spDistrict = (Spinner) container.findViewById(R.id.sp_district);
        spWard = (Spinner) container.findViewById(R.id.sp_ward);
        spGender = (Spinner) container.findViewById(R.id.sp_gender);
        txtBirthday = (TextView) container.findViewById(R.id.txt_birthday);


    }

    public void event(){
        btnAddFavouriteSport.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Toast.makeText(getApplicationContext(), "Add Sport",
                        Toast.LENGTH_LONG).show();
            }
        });

        txtBirthday.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                datePickerDialog.show();
            }
        });

    }

    public void prepareData(){
        String[] cityItems = new String[] {"Thành phố Hồ Chí Minh", "Hà Nội"};
        String[] districtItems = new String[] {"Gò Vấp"};
        String[] wardItems = new String[] {"Phường 17"};
        String[] genderItems = new String[] {"Nam","Nữ","Khác"};
        ArrayAdapter<String> cityAdapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_spinner_item, cityItems);
        ArrayAdapter<String> districtAdapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_spinner_item, districtItems);
        ArrayAdapter<String> wardAdapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_spinner_item, wardItems);
        ArrayAdapter<String> genderAdapter = new ArrayAdapter<String>(this,
                android.R.layout.simple_spinner_item, genderItems);
        cityAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        districtAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        wardAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        genderAdapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        spCity.setAdapter(cityAdapter);
        spDistrict.setAdapter(districtAdapter);
        spWard.setAdapter(wardAdapter);
        spGender.setAdapter(genderAdapter);


    }

    public void createDatePicker(){

        Calendar newCalendar = Calendar.getInstance();
        dateFormatter = new SimpleDateFormat("dd-MM-yyyy", Locale.US);
        datePickerDialog = new DatePickerDialog(this, new DatePickerDialog.OnDateSetListener() {

            public void onDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth) {
                Calendar newDate = Calendar.getInstance();
                newDate.set(year, monthOfYear, dayOfMonth);
                txtBirthday.setText(dateFormatter.format(newDate.getTime()));
            }

        },newCalendar.get(Calendar.YEAR), newCalendar.get(Calendar.MONTH), newCalendar.get(Calendar.DAY_OF_MONTH));
    }

}

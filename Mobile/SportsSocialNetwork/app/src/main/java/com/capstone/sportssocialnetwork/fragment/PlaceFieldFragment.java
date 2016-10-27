package com.capstone.sportssocialnetwork.fragment;

import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.widget.DefaultItemAnimator;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Spinner;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.BookingActivity;
import com.capstone.sportssocialnetwork.adapter.PlaceDetailAdapter;
import com.capstone.sportssocialnetwork.adapter.PlaceFieldAdapter;
import com.capstone.sportssocialnetwork.model.Field;
import com.capstone.sportssocialnetwork.model.FieldType;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/8/16.
 */
public class PlaceFieldFragment extends Fragment {
    private final static String ALL_STRING = "Tất cả";
    RecyclerView lvPlaceField;
    Spinner spinerFieldType;
    RestService service;
    private int placeId;
    PlaceFieldAdapter adapter;
    private ProgressDialog progressDialog;
    ArrayAdapter<String> fieldTypeAdapter;
    private HashMap<String, Integer> fieldTypeHash;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_place_field, container, false);
        initView(v);
        prepareData();
        createSpinner();
        event();
        return v;
    }

    private void event() {
        spinerFieldType.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {
            @Override
            public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
                String text = fieldTypeAdapter.getItem(position);
                if (fieldTypeHash.containsKey(text)) {
                    adapter.getFilter(fieldTypeHash.get(text));
                }else{
                    adapter.getFilter(-1);
                }
            }

            @Override
            public void onNothingSelected(AdapterView<?> parent) {
//                Toast.makeText(BookingActivity.this, "sport no choice", Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void createSpinner() {
        fieldTypeAdapter = new ArrayAdapter(getActivity(), R.layout.item_spinner, new ArrayList());
        fieldTypeAdapter.setDropDownViewResource(R.layout.item_spinner);
        spinerFieldType.setAdapter(fieldTypeAdapter);
        spinerFieldType.setSelection(0);
    }

    private void prepareData() {
         adapter = new PlaceFieldAdapter(getActivity(), new ArrayList<Field>() {
         });
        RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(getActivity().getApplicationContext());
        lvPlaceField.setLayoutManager(mLayoutManager);
        lvPlaceField.setItemAnimator(new DefaultItemAnimator());
        lvPlaceField.setAdapter(adapter);
        progressDialog = new ProgressDialog(getActivity());
        fieldTypeHash = new HashMap<>();
    }

    private void loadFieldType(int placeId) {
        progressDialog.setMessage("Đang tải loại sân...");
        progressDialog.show();
        Call<ResponseModel<List<FieldType>>> call = service.getPlaceService().getFieldType(placeId);
        call.enqueue(new Callback<ResponseModel<List<FieldType>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<FieldType>>> call, Response<ResponseModel<List<FieldType>>> response) {
                progressDialog.dismiss();
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        fieldTypeHash.clear();
                        for (FieldType item : response.body().getData()
                                ) {
                            fieldTypeHash.put(item.getName(), item.getId());
                        }
                        fieldTypeAdapter.clear();
                        fieldTypeAdapter.add(ALL_STRING);
                        fieldTypeAdapter.addAll(fieldTypeHash.keySet());
                        if (fieldTypeHash.size() > 0) {
                            spinerFieldType.setSelection(0);
                            adapter.getFilter(-1);
                        }
                    } else {
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {

                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<FieldType>>> call, Throwable t) {
                progressDialog.dismiss();
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });

    }

    private void loadData(){
        service.getPlaceService().getFieldOfPlace(placeId).enqueue(new Callback<ResponseModel<List<Field>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Field>>> call, Response<ResponseModel<List<Field>>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setFields(response.body().getData());
                    }else{
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Field>>> call, Throwable t) {
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private void initView(View v) {
        service = new RestService();
        placeId = getArguments().getInt("placeId");
        lvPlaceField = (RecyclerView) v.findViewById(R.id.lv_place_field);
        spinerFieldType  = (Spinner) v.findViewById(R.id.spinner_sport);
    }

    @Override
    public void onResume() {
        super.onResume();
        loadFieldType(placeId);
        loadData();
    }
}

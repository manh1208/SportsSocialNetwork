package com.capstone.sportssocialnetwork.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.support.v7.app.AlertDialog;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.PlaceFieldScheduleAdapter;
import com.capstone.sportssocialnetwork.model.FieldSchedule;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 11/5/16.
 */

public class PlaceFieldScheduleFragment extends Fragment {
    private ViewHolder viewHolder;
    private RestService service;
    private int placeId;
    private PlaceFieldScheduleAdapter adapter;


    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {

        View v = inflater.inflate(R.layout.fragment_place_field_schedule, container, false);
        viewHolder = new ViewHolder(v);
        init();
        viewHolder.lvFieldSchedule.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                FieldSchedule fieldSchedule = adapter.getItem(position);

                AlertDialog.Builder builder = new AlertDialog.Builder(getActivity());
                View v = getActivity().getLayoutInflater().inflate(R.layout.dialog_field_schedule,null,false);
                TextView txtFieldName = (TextView) v.findViewById(R.id.txt_field_schedule_field_name);
                TextView txtStartDate = (TextView) v.findViewById(R.id.txt_field_schedule_start_date);
                TextView txtEndDate = (TextView) v.findViewById(R.id.txt_field_schedule_end_date);
                TextView txtStartTime = (TextView) v.findViewById(R.id.txt_field_schedule_start_time);
                TextView txtEndTime = (TextView) v.findViewById(R.id.txt_field_schedule_end_time);
                TextView txtRepeatDays = (TextView) v.findViewById(R.id.txt_field_schedule_repeat_days);
                TextView txtType = (TextView) v.findViewById(R.id.txt_field_schedule_type);
                txtFieldName.setText(fieldSchedule.getFieldName());
                        txtStartDate.setText(fieldSchedule.getStartDateString());
                                txtEndDate.setText(fieldSchedule.getEndDateString());
                                        txtStartTime.setText(fieldSchedule.getStartTimeString());
                                                txtEndTime.setText(fieldSchedule.getEndTimeString());
                                                        txtRepeatDays.setText(fieldSchedule.getRepeatDay());
                txtType.setText(fieldSchedule.getTypeString());
                builder.setView(v)
                        .setNegativeButton("Đóng",null)
                        .create().show();

            }
        });
        return v;
    }

    private void init() {
        service = new RestService();
        placeId = getArguments().getInt("placeId");
        adapter = new PlaceFieldScheduleAdapter(getActivity(), R.layout.item_field_schedule, new ArrayList<FieldSchedule>());
        viewHolder.lvFieldSchedule.setAdapter(adapter);
    }

    private void loadData() {
        service.getPlaceService().getFieldSchedule(placeId).enqueue(new Callback<ResponseModel<List<FieldSchedule>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<FieldSchedule>>> call, Response<ResponseModel<List<FieldSchedule>>> response) {
                if (response.isSuccessful()) {
                    if (response.body().isSucceed()) {
                        adapter.setFieldSchedules(response.body().getData());
                    } else {
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                } else {
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<FieldSchedule>>> call, Throwable t) {
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }

    private final class ViewHolder {
        ListView lvFieldSchedule;

        ViewHolder(View v) {
            lvFieldSchedule = (ListView) v.findViewById(R.id.lv_place_field_schedule);

        }
    }

    @Override
    public void onResume() {
        super.onResume();
        loadData();
    }
}

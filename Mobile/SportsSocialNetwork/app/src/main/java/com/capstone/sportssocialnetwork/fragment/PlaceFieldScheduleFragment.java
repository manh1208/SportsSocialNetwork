package com.capstone.sportssocialnetwork.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;
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

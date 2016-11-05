package com.capstone.sportssocialnetwork.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.capstone.sportssocialnetwork.R;

/**
 * Created by ManhNV on 11/5/16.
 */

public class PlaceFieldScheduleFragment extends Fragment {
    ViewHolder viewHolder;
    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {

        View v = inflater.inflate(R.layout.fragment_place_field_schedule,container,false);
        viewHolder = new ViewHolder(v);
        return v;
    }

    private final class ViewHolder{

        ViewHolder(View v){


        }
    }
}

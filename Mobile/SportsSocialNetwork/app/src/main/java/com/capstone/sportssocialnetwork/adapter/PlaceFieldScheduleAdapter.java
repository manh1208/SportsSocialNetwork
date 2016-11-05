package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.FieldSchedule;

import java.util.List;

/**
 * Created by ManhNV on 11/5/16.
 */

public class PlaceFieldScheduleAdapter extends ArrayAdapter<FieldSchedule> {
    private Context mContext;
    private List<FieldSchedule> fieldSchedules;

    public PlaceFieldScheduleAdapter(Context context, int resource, List<FieldSchedule> objects) {
        super(context, resource, objects);
        mContext = context;
        fieldSchedules = objects;
    }

    @Override
    public int getCount() {
        return fieldSchedules.size();
    }

    @Nullable
    @Override
    public FieldSchedule getItem(int position) {
        return fieldSchedules.get(position);
    }

    public void setFieldSchedules(List<FieldSchedule> fieldSchedules) {
        this.fieldSchedules = fieldSchedules;
        notifyDataSetChanged();
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView ==null){
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_field_schedule,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);

        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        FieldSchedule fieldSchedule= getItem(position);
        viewHolder.txtFieldName.setText(fieldSchedule.getFieldName());
        viewHolder.txtStartTime.setText(fieldSchedule.getStartTimeString());
        viewHolder.txtEndTime.setText(fieldSchedule.getEndTimeString());
        viewHolder.txtType.setText(fieldSchedule.getTypeString());

        return convertView;
    }

    private final class ViewHolder{
        TextView txtFieldName;
        TextView txtStartTime;
        TextView txtEndTime;
        TextView txtType;
        ViewHolder(View v){
            txtFieldName = (TextView) v.findViewById(R.id.txt_field_schedule_field_name);
            txtStartTime = (TextView) v.findViewById(R.id.txt_field_schedule_start_time);
            txtEndTime = (TextView) v.findViewById(R.id.txt_field_schedule_end_time);
            txtType = (TextView) v.findViewById(R.id.txt_field_schedule_type);
        }
    }
}

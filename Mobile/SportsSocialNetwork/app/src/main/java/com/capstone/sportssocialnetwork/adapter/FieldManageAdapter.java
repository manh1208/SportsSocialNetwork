package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.content.Intent;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.BookingActivity;
import com.capstone.sportssocialnetwork.model.Field;
import com.capstone.sportssocialnetwork.model.Place;

import java.util.List;

/**
 * Created by MyPC on 02/10/2016.
 */
public class FieldManageAdapter extends ArrayAdapter<Field> {

    private Context mContext;
    private List<Field> field;

    public FieldManageAdapter(Context context, int resource, List<Field> objects) {
        super(context, resource, objects);
        mContext = context;
        field = objects;
    }

    @Override
    public int getCount() {
        return 5;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView ==null)
        {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_place_field,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        return convertView;
    }

    private class ViewHolder{
        public ViewHolder(View v){
            v.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    //testing testing
                    Intent intent = new Intent(mContext, BookingActivity.class);
                    mContext.startActivity(intent);
                }
            });
        }
    }

}

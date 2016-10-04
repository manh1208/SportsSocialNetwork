package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Group;

import org.w3c.dom.Text;

import java.util.List;

/**
 * Created by ManhNV on 10/3/16.
 */
public class MenuProfileAdapter extends ArrayAdapter<Group> {
    private Context mContext;
    private List<Group> mGroups;

    public MenuProfileAdapter(Context context, int resource, List<Group> objects) {
        super(context, resource, objects);
        this.mContext = context;
        this.mGroups = objects;
    }

    @Override
    public int getCount() {
        return 20;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView==null){
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_menu_profile,parent,false);
            viewHolder = new ViewHolder((convertView));
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }
        if (position==0){
            viewHolder.header.setVisibility(View.VISIBLE);
            viewHolder.image.setVisibility(View.GONE);
            viewHolder.groupName.setVisibility(View.GONE);
        }else{
            viewHolder.header.setVisibility(View.GONE);
            viewHolder.image.setVisibility(View.VISIBLE);
            viewHolder.groupName.setVisibility(View.VISIBLE);
        }

        return convertView;
    }

    private final class ViewHolder{
        TextView header;
        RoundedImageView image;
        TextView groupName;
        ViewHolder(View v){
            image = (RoundedImageView) v.findViewById(R.id.iv_menu_image);
            groupName = (TextView) v.findViewById(R.id.txt_menu_group_name);
            header = (TextView) v.findViewById(R.id.txt_name_group);

        }
    }
}

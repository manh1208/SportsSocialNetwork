package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.net.Uri;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.custom.CustomImage;
import com.capstone.sportssocialnetwork.model.Event;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.capstone.sportssocialnetwork.utils.Utilities;
import com.squareup.picasso.Picasso;

import java.text.ParseException;
import java.util.List;

/**
 * Created by ManhNV on 7/5/2016.
 */
public class ManageEventAdapter extends ArrayAdapter<Event> implements View.OnClickListener {
    private Context mContext;
    private List<Event> mEvents;
    private String userId;
    private RestService restService;
    private boolean isSaved;
    private boolean isOwnerEvent;

    public ManageEventAdapter(Context context, int resource, List<Event> events) {
        super(context, resource, events);
        mContext = context;
        mEvents = events;
        userId = DataUtils.getINSTANCE(mContext).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
        restService = new RestService();
    }

    @Override
    public long getItemId(int position) {
        return super.getItemId(position);
    }

    @Override
    public Event getItem(int position) {
        return mEvents.get(position);
    }

    @Override
    public int getCount() {
        return mEvents.size();
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_manage_event, parent, false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        } else {
            viewHolder = (ViewHolder) convertView.getTag();
        }
        Event event = getItem(position);

        Picasso.with(mContext).load(Uri.parse(DataUtils.URL + event.getImage()))
                .placeholder(R.drawable.placeholder)
                .error(R.drawable.placeholder)
                .noFade()
                .into(viewHolder.ivCoverEvent);
        viewHolder.tvEventName.setText(event.getName());
        viewHolder
                .tvEventDate
                .setText(event.getStartDateString() + " - " + event.getEndDateString());


        viewHolder.tvEventType.setText(event.getPlaceId() + "");
//        viewHolder.btnSave.setTag(position);
//        viewHolder.btnSave.setOnClickListener(this);
        return convertView;
    }

    @Override
    public void onClick(View v) {
        int id = v.getId();
        switch (id) {

        }
    }

    public void setEventList(List<Event> eventList) {
        this.mEvents = eventList;
        notifyDataSetChanged();
    }

    private class ViewHolder {
        CustomImage ivCoverEvent;
        TextView tvEventName;
        TextView tvEventDate;
        TextView tvEventType;

        ViewHolder(View convertView) {
            ivCoverEvent = (CustomImage) convertView.findViewById(R.id.iv_event_cover);
            tvEventName = (TextView) convertView.findViewById(R.id.tv_event_name);
            tvEventDate = (TextView) convertView.findViewById(R.id.tv_event_date_time);
            tvEventType = (TextView) convertView.findViewById(R.id.txt_event_place);
        }

    }

}

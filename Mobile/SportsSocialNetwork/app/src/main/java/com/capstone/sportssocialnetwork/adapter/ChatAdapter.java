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
import com.capstone.sportssocialnetwork.model.Message;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;

import java.util.List;

/**
 * Created by ManhNV on 11/11/16.
 */

public class ChatAdapter extends ArrayAdapter<Message> {
    private Context mContext;
    private List<Message> messages;
    private String userId;

    public ChatAdapter(Context context, int resource, List<Message> objects) {
        super(context, resource, objects);
        mContext = context;
        messages = objects;
        userId = DataUtils.getINSTANCE(mContext).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
    }

    @Override
    public int getCount() {
        return messages.size();
    }

    @Nullable
    @Override
    public Message getItem(int position) {
        return messages.get(position);
    }

    @Override
    public void add(Message object) {
        messages.add(object);
        notifyDataSetChanged();
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        ViewHolder viewHolder;
        if (convertView==null){
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_chat,parent,false);
            viewHolder = new ViewHolder(convertView);
            convertView.setTag(viewHolder);
        }else{
            viewHolder = (ViewHolder) convertView.getTag();
        }

        Message message = getItem(position);
        if (message.getSender().equals("System")){
            viewHolder.txtSender.setVisibility(View.VISIBLE);
            viewHolder.txtSender.setText(message.getMessage());
            viewHolder.txtReceive.setVisibility(View.GONE);
            viewHolder.txtSend.setVisibility(View.GONE);
        }else {
            if (message.getUserId().equals(userId)) {
                viewHolder.txtSender.setVisibility(View.GONE);
                viewHolder.txtReceive.setVisibility(View.GONE);
                viewHolder.txtSend.setVisibility(View.VISIBLE);
                viewHolder.txtSend.setText(message.getMessage());
            } else {
                viewHolder.txtReceive.setVisibility(View.VISIBLE);
                viewHolder.txtSend.setVisibility(View.GONE);
                if (position > 0 && getItem(position).getUserId().equals(getItem(position - 1).getUserId())) {
                    viewHolder.txtSender.setVisibility(View.GONE);
                } else {
                    viewHolder.txtSender.setVisibility(View.VISIBLE);
                }
                viewHolder.txtSender.setText(message.getSender());
                viewHolder.txtReceive.setText(message.getMessage());
            }
        }

        return convertView;
    }

    private final class ViewHolder{
        TextView txtSender;
        TextView txtReceive;
        TextView txtSend;
        ViewHolder(View v){
            txtSender = (TextView) v.findViewById(R.id.txt_sender);
            txtReceive = (TextView) v.findViewById(R.id.txt_message_receive);
            txtSend = (TextView) v.findViewById(R.id.txt_message_send);
        }
    }
}

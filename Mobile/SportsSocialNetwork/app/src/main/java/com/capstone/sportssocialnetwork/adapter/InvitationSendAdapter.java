package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.support.annotation.NonNull;
import android.support.annotation.Nullable;
import android.support.v7.widget.PopupMenu;
import android.view.LayoutInflater;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageButton;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.Invitation;
import com.capstone.sportssocialnetwork.model.UserInvitation;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;

import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 11/7/16.
 */

public class InvitationSendAdapter extends ArrayAdapter<Invitation> {
    private Context mContext;
    private List<Invitation> invitations;
    private String userId;
    private RestService service;


    public InvitationSendAdapter(Context context, int resource, List<Invitation> objects) {
        super(context, resource, objects);
        mContext = context;
        invitations = objects;
        userId = DataUtils.getINSTANCE(mContext).getPreferences().getString(SharePreferentName.SHARE_USER_ID, "");
        service = new RestService();
    }

    @Override
    public int getCount() {
        return invitations.size();
    }

    @Nullable
    @Override
    public Invitation getItem(int position) {
        return invitations.get(position);
    }

    @NonNull
    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        Viewholder viewholder;
        if (convertView == null) {
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_send, parent, false);
            viewholder = new Viewholder(convertView);
            convertView.setTag(viewholder);
        } else {
            viewholder = (Viewholder) convertView.getTag();
        }

        Invitation invitation = getItem(position);
        viewholder.txtSender.setText(invitation.getSubject());
        if (invitation.getContent().length() > 30) {
            viewholder.txtContent.setText(invitation.getContent().substring(0, 30) + "...");
        } else {
            viewholder.txtContent.setText(invitation.getContent());
        }

        viewholder.txtAcceptedCount.setText(invitation.getAcceptedCount()+"");

        return convertView;
    }



    public void setInvitations(List<Invitation> invitations) {
        this.invitations = invitations;
        notifyDataSetChanged();
    }

    private final class Viewholder {
        TextView txtSender;
        TextView txtContent;
        TextView txtAcceptedCount;

        Viewholder(View v) {
            txtSender = (TextView) v.findViewById(R.id.txt_invitation_sender_name);
            txtContent = (TextView) v.findViewById(R.id.txt_invitation_content);
            txtAcceptedCount = (TextView) v.findViewById(R.id.txt_invitation_accepted_count);
        }
    }
}

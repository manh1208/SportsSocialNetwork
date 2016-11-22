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
import com.capstone.sportssocialnetwork.model.User;
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

public class InvitationReceiveAdapter extends ArrayAdapter<Invitation> {
    private Context mContext;
    private List<Invitation> invitations;
    private String userId;
    private RestService service;


    public InvitationReceiveAdapter(Context context, int resource, List<Invitation> objects) {
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
            convertView = LayoutInflater.from(mContext).inflate(R.layout.item_receive, parent, false);
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


        if (invitation.isAccepted(userId) == null) {
            viewholder.txtStatus.setText("Đang chờ");
            viewholder.txtStatus.setBackgroundResource(R.drawable.button_invitation_waiting);
            viewholder.ivDropdown.setVisibility(View.VISIBLE);
        } else {
            if (invitation.isAccepted(userId)) {
                viewholder.txtStatus.setText("Đã chấp nhận");
                viewholder.txtStatus.setBackgroundResource(R.drawable.button_invitation_approve);
            } else {
                viewholder.txtStatus.setText("Không chấp nhận");
                viewholder.txtStatus.setBackgroundResource(R.drawable.button_invitation_approve);
            }
            viewholder.ivDropdown.setVisibility(View.INVISIBLE);
        }

        viewholder.ivDropdown.setTag(position);
        viewholder.ivDropdown.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                int position = (int) v.getTag();
                showPopupMenu(v, position);
            }
        });

        return convertView;
    }

    public void showPopupMenu(View v, int position) {
        PopupMenu popupMenu = new PopupMenu(mContext, v);
        final MenuInflater inflater = popupMenu.getMenuInflater();
        inflater.inflate(R.menu.menu_invitation_popup, popupMenu.getMenu());
        popupMenu.show();
        MenuItem item = popupMenu.getMenu().getItem(0);
        Invitation invitation = getItem(position);
        final UserInvitation userInvitation = invitation.getUserInvitation(userId);
        popupMenu.setOnMenuItemClickListener(new PopupMenu.OnMenuItemClickListener() {
            @Override
            public boolean onMenuItemClick(MenuItem item) {
                int id = item.getItemId();
                switch (id) {
                    case R.id.menu_invitation_approve:
                        service.getAccountService()
                                .acceptInvitation(userInvitation.getId())
                                .enqueue(new Callback<ResponseModel<String>>() {
                                    @Override
                                    public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                                        if (response.isSuccessful()){
                                            if(response.body().isSucceed()){
                                                userInvitation.setAccepted(true);
                                                notifyDataSetChanged();
                                            }else{
                                                Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                                            }
                                        }else{
                                            Toast.makeText(mContext,response.message(), Toast.LENGTH_SHORT).show();
                                        }
                                    }

                                    @Override
                                    public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                                        Toast.makeText(mContext,R.string.failure, Toast.LENGTH_SHORT).show();
                                    }
                                });

                        return true;
                    case R.id.menu_invitation_unapprove:
                        service.getAccountService()
                                .refuseInvitation(userInvitation.getId())
                                .enqueue(new Callback<ResponseModel<String>>() {
                                    @Override
                                    public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                                        if (response.isSuccessful()){
                                            if(response.body().isSucceed()){
                                                userInvitation.setAccepted(false);
                                            }else{
                                                Toast.makeText(mContext, response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                                            }
                                        }else{
                                            Toast.makeText(mContext,response.message(), Toast.LENGTH_SHORT).show();
                                        }
                                    }

                                    @Override
                                    public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                                        Toast.makeText(mContext,R.string.failure, Toast.LENGTH_SHORT).show();
                                    }
                                });

                        return true;


                }
                return false;
            }
        });
    }

    public void setInvitations(List<Invitation> invitations) {
        this.invitations = invitations;
        notifyDataSetChanged();
    }

    private final class Viewholder {
        TextView txtSender;
        TextView txtContent;
        TextView txtStatus;
        ImageButton ivDropdown;

        Viewholder(View v) {
            txtSender = (TextView) v.findViewById(R.id.txt_invitation_sender_name);
            txtContent = (TextView) v.findViewById(R.id.txt_invitation_content);
            txtStatus = (TextView) v.findViewById(R.id.txt_invitation_status);
            ivDropdown = (ImageButton) v.findViewById(R.id.btn_invitation_receive_menu_down);
        }
    }
}

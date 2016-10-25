package com.capstone.sportssocialnetwork.fragment;

import android.content.Intent;
import android.content.SharedPreferences;
import android.net.Uri;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ListView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.LoginActivity;
import com.capstone.sportssocialnetwork.activity.MyOrderActivity;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.adapter.MenuProfileAdapter;
import com.capstone.sportssocialnetwork.custom.RoundedImageView;
import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.squareup.picasso.Picasso;

import java.util.ArrayList;
import java.util.List;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/6/16.
 */
public class ProfileFragment extends Fragment {
    private ListView lvMenuProfile;
    private MenuProfileAdapter adapter;
    private View headerView;
    private ViewHolder headerHolder;
    private String fullName;
    private String avatar;
    private String userId;
    private RestService service;


    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
        SharedPreferences sharedPreferences = DataUtils.getINSTANCE(getActivity()).getPreferences();
        fullName = sharedPreferences.getString(SharePreferentName.SHARE_USER_FULLNAME,"No name");
        avatar = sharedPreferences.getString(SharePreferentName.SHARE_USER_AVATAR,"");
        userId = sharedPreferences.getString(SharePreferentName.SHARE_USER_ID,"");
        service = new RestService();
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile, container, false);
        lvMenuProfile = (ListView) v.findViewById(R.id.lv_menu_profile);
        headerView = inflater.inflate(R.layout.item_header_menu_profile,null,false);
        lvMenuProfile.addHeaderView(headerView);
        adapter = new MenuProfileAdapter(getActivity(),R.layout.item_menu_profile,new ArrayList<Group>());
        headerHolder = new ViewHolder(headerView);
        lvMenuProfile.setAdapter(adapter);
        lvMenuProfile.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                if (position>1){
                    int curPosition = position-2;
                    Toast.makeText(getActivity(), "Vị trí:" + curPosition , Toast.LENGTH_SHORT).show();
                }
            }
        });

        headerHolder.layoutProfile.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(), ProfileActivity.class);
                startActivity(intent);
            }
        });

        headerHolder.layoutMyOrder.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(), MyOrderActivity.class);
                startActivity(intent);
            }
        });
        headerHolder.txtFullName.setText(fullName);
        Picasso.with(getActivity()).load(Uri.parse(DataUtils.URL+avatar))
                .placeholder(R.drawable.img_default_avatar)
                .error(R.drawable.img_default_avatar_error)
                .into(headerHolder.ivAvatar);
        getGroup();
        return v;
    }

    private void getGroup(){
        Call<ResponseModel<List<Group>>> call = service.getGroupService().getGroups(userId);
        call.enqueue(new Callback<ResponseModel<List<Group>>>() {
            @Override
            public void onResponse(Call<ResponseModel<List<Group>>> call, Response<ResponseModel<List<Group>>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        adapter.setGroups(response.body().getData());
                    }else{
                        Toast.makeText(getActivity(), response.body().getErrorsString(), Toast.LENGTH_SHORT).show();
                    }
                }else{
                    Toast.makeText(getActivity(), response.message(), Toast.LENGTH_SHORT).show();
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<List<Group>>> call, Throwable t) {
                Toast.makeText(getActivity(), R.string.failure, Toast.LENGTH_SHORT).show();
            }
        });
    }



    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.menu_profile, menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id= item.getItemId();
        switch (id){
            case R.id.menu_logout:
                DataUtils.getINSTANCE(getActivity()).getPreferences().edit().clear().commit();
                Intent intent = new Intent(getActivity(),LoginActivity.class);
                intent.setFlags(Intent.FLAG_ACTIVITY_SINGLE_TOP);
                startActivity(intent);
                getActivity().finish();
                return true;
        }
        return super.onOptionsItemSelected(item);
    }

    private final class ViewHolder{
        RelativeLayout layoutProfile;
        RelativeLayout layoutMyOrder;
        RoundedImageView ivAvatar;
        TextView txtFullName;

        ViewHolder(View v){
            layoutProfile = (RelativeLayout) v.findViewById(R.id.layout_menu_profile);
            layoutMyOrder = (RelativeLayout) v.findViewById(R.id.layout_menu_my_order);
            ivAvatar = (RoundedImageView) v.findViewById(R.id.iv_user_menu_avatar);
            txtFullName = (TextView) v.findViewById(R.id.txt_user_menu_fullname);
        }
    }
}

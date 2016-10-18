package com.capstone.sportssocialnetwork.fragment;

import android.content.Intent;
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
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.LoginActivity;
import com.capstone.sportssocialnetwork.activity.MyOrderActivity;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.adapter.MenuProfileAdapter;
import com.capstone.sportssocialnetwork.model.Group;
import com.capstone.sportssocialnetwork.utils.DataUtils;

import java.util.ArrayList;

/**
 * Created by ManhNV on 9/6/16.
 */
public class ProfileFragment extends Fragment {
    ListView lvMenuProfile;
    MenuProfileAdapter menuProfileAdapter;
    View headerView;

    ViewHolder headerHolder;


    @Override
    public void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_profile, container, false);
        lvMenuProfile = (ListView) v.findViewById(R.id.lv_menu_profile);
        headerView = inflater.inflate(R.layout.item_header_menu_profile,null,false);
        lvMenuProfile.addHeaderView(headerView);
        menuProfileAdapter = new MenuProfileAdapter(getActivity(),R.layout.item_menu_profile,new ArrayList<Group>());
        headerHolder = new ViewHolder(headerView);
        lvMenuProfile.setAdapter(menuProfileAdapter);
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
        return v;
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

        ViewHolder(View v){
            layoutProfile = (RelativeLayout) v.findViewById(R.id.layout_menu_profile);
            layoutMyOrder = (RelativeLayout) v.findViewById(R.id.layout_menu_my_order);
        }
    }
}

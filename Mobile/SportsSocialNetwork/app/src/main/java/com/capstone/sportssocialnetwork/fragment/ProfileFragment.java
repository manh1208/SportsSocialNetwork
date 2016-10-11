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
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.adapter.MenuProfileAdapter;
import com.capstone.sportssocialnetwork.model.Group;

import java.util.ArrayList;

/**
 * Created by ManhNV on 9/6/16.
 */
public class ProfileFragment extends Fragment {
    ListView lvMenuProfile;
    MenuProfileAdapter menuProfileAdapter;
    View headerView;


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
        lvMenuProfile.setAdapter(menuProfileAdapter);
        lvMenuProfile.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {

                if (position==0){
                    Intent intent = new Intent(getActivity(), ProfileActivity.class);
                    startActivity(intent);
                }else if (position>1){
                    int curPosition = position-2;
                    Toast.makeText(getActivity(), "Vị trí:" + curPosition , Toast.LENGTH_SHORT).show();
                }
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
        return super.onOptionsItemSelected(item);
    }
}

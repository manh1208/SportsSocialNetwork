package com.capstone.sportssocialnetwork.fragment;

import android.content.Context;
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
import android.widget.Button;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.PlaceDetailActivity;
import com.capstone.sportssocialnetwork.activity.PostActivity;
import com.capstone.sportssocialnetwork.adapter.FeedAdapter;
import com.capstone.sportssocialnetwork.adapter.PlaceAdapter;
import com.capstone.sportssocialnetwork.model.Feed;
import com.capstone.sportssocialnetwork.model.Place;

import java.util.ArrayList;

/**
 * Created by ManhNV on 9/6/16.
 */
public class BookingFragment extends Fragment {
    private ListView lvPlace;
    private PlaceAdapter adapter;
//    private Button btnPost;
//    private View  header;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_booking,container,false);
        initView(v);
        prepareData();
        event();
        return v;
    }

    private void event() {
//        btnPost.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View v) {
//                Intent intent = new Intent(getActivity(), PostActivity.class);
//                startActivity(intent);
//            }
//        });

    }

    private void prepareData() {


//        lvPlace.addHeaderView(header);
        adapter = new PlaceAdapter(getActivity(),R.layout.item_feed,new ArrayList<Place>());
        lvPlace.setAdapter(adapter);
        lvPlace.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                Intent intent =  new Intent(getActivity(), PlaceDetailActivity.class);
                startActivity(intent);
            }
        });
    }

    private void initView(View v) {
        lvPlace = (ListView) v.findViewById(R.id.lv_list_place);
//        header= ((LayoutInflater) getActivity()
//                .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
//                R.layout.item_header_feed, null, false);
//        btnPost = (Button) header.findViewById(R.id.btn_feed_post);
    }

    @Override
    public void onPrepareOptionsMenu(Menu menu) {
        super.onPrepareOptionsMenu(menu);
    }

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.menu_booking, menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();
        switch (id){
            case R.id.menu_filter:
                break;
        }
        return super.onOptionsItemSelected(item);
    }
}

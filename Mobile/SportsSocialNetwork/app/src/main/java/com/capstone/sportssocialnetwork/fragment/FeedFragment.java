package com.capstone.sportssocialnetwork.fragment;

import android.content.Context;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.adapter.FeedAdapter;
import com.capstone.sportssocialnetwork.model.Feed;

import java.util.ArrayList;

/**
 * Created by ManhNV on 9/6/16.
 */
public class FeedFragment extends Fragment {
    private ListView lvFeed;
    private FeedAdapter adapter;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_feed,container,false);
        initView(v);
        prepareData();
        return v;
    }

    private void prepareData() {
        View  header= ((LayoutInflater) getActivity()
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                R.layout.item_header_feed, null, false);
        lvFeed.addHeaderView(header);
        adapter = new FeedAdapter(getActivity(),R.layout.item_feed,new ArrayList<Feed>());
        lvFeed.setAdapter(adapter);
    }

    private void initView(View v) {
        lvFeed = (ListView) v.findViewById(R.id.lv_list_feed);
    }


}

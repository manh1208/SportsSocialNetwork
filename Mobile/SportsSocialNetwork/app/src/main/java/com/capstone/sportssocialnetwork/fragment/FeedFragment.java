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
import android.widget.Button;
import android.widget.ListView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.NotificationActivity;
import com.capstone.sportssocialnetwork.activity.PostActivity;
import com.capstone.sportssocialnetwork.adapter.FeedAdapter;
import com.capstone.sportssocialnetwork.model.Feed;

import java.util.ArrayList;

/**
 * Created by ManhNV on 9/6/16.
 */
public class FeedFragment extends Fragment {
    private ListView lvFeed;
    private FeedAdapter adapter;
    private Button btnPost;
    private View header;

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setHasOptionsMenu(true);
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_feed, container, false);
        initView(v);
        prepareData();
        event();
        return v;
    }

    private void event() {
        btnPost.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getActivity(), PostActivity.class);
                startActivity(intent);
            }
        });

    }

    private void prepareData() {


        lvFeed.addHeaderView(header);
        adapter = new FeedAdapter(getActivity(), R.layout.item_feed, new ArrayList<Feed>());
        lvFeed.setAdapter(adapter);


    }

    private void initView(View v) {
        lvFeed = (ListView) v.findViewById(R.id.lv_list_feed);
        header = ((LayoutInflater) getActivity()
                .getSystemService(Context.LAYOUT_INFLATER_SERVICE)).inflate(
                R.layout.item_header_feed, null, false);
        btnPost = (Button) header.findViewById(R.id.btn_feed_post);
    }

    @Override
    public void onPrepareOptionsMenu(Menu menu) {
        super.onPrepareOptionsMenu(menu);
    }

    @Override
    public void onCreateOptionsMenu(Menu menu, MenuInflater inflater) {
        super.onCreateOptionsMenu(menu, inflater);
        inflater.inflate(R.menu.menu_feed, menu);
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        switch (id) {
            case R.id.menu_notice:
                Intent intent = new Intent(getActivity(), NotificationActivity.class);
                startActivity(intent);
                return true;
        }
        return super.onOptionsItemSelected(item);
    }
}

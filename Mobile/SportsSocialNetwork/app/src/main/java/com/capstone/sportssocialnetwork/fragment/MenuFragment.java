package com.capstone.sportssocialnetwork.fragment;

import android.content.Intent;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.ProfileActivity;
import com.capstone.sportssocialnetwork.activity.ProfileDetailActivity;

/**
 * Created by ManhNV on 9/6/16.
 */
public class MenuFragment extends Fragment {
    private ViewGroup vgMenu;

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View v = inflater.inflate(R.layout.fragment_menu, container, false);
        initView(v);
        createMenu(inflater);
        return v;
    }


    public void createMenu(LayoutInflater inflater){
        String menuContent="";
        int img=0;


        for(int i=0;i<10;i++){
            View item= inflater.inflate(R.layout.item_menu,null,false);
            ImageView imageView= (ImageView) item.findViewById(R.id.iv_menu_image);
            TextView textView= (TextView) item.findViewById(R.id.txt_menu_content);
            switch (i){
                case (0):{
                    menuContent="ManhNV \nXem trang cá nhân của bạn";
                    img=R.drawable.img_default_avatar;
                    item.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Intent intent = new Intent(getActivity(), ProfileActivity.class);
                            startActivity(intent);
                        }
                    });
                    break;
                }
                case (1):{
                    insertSeparator(inflater);
                    menuContent="Cài đặt";
                    img=R.drawable.ic_settings;
                    item.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Toast.makeText(getContext(),"Cài đặt",Toast.LENGTH_SHORT).show();
                        }
                    });
                    break;
                }
                case (2):{
                    menuContent="Đặt sân";
                    img=R.drawable.ic_order_field;
                    item.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Toast.makeText(getContext(),"Đặt sân",Toast.LENGTH_SHORT).show();
                        }
                    });
                    break;
                }
                case (3):{
                    menuContent="Mời";
                    img=R.drawable.ic_invitation;
                    item.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Toast.makeText(getContext(),"Mời",Toast.LENGTH_SHORT).show();
                        }
                    });
                    break;
                }
            }
            imageView.setImageResource(img);
            textView.setText(menuContent);
            vgMenu.addView(item);

        }



    }

    public void initView(View v){
        vgMenu = (ViewGroup) v.findViewById(R.id.container_menu);
    }

    public void insertSeparator(LayoutInflater inflater){
        View separator= inflater.inflate(R.layout.item_menu_separator,null,false);
        vgMenu.addView(separator);

    }

}

package com.capstone.sportssocialnetwork.adapter;

import android.content.Context;
import android.support.v7.widget.RecyclerView;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.response.PlaceResponseModel;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;

import org.w3c.dom.Text;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 9/8/16.
 */
public class PlaceDetailAdapter extends RecyclerView.Adapter<PlaceDetailAdapter.ContentViewHolder>   {

    private Context mContext;
    private int placeId;
    private PlaceResponseModel place;
    private RestService service;

    public PlaceDetailAdapter(Context mContext, int placeId) {
        this.mContext = mContext;
        this.placeId = placeId;
        service = new RestService();
        place = null;
        loadData();
    }


    @Override
    public ContentViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View itemView = LayoutInflater.from(mContext)
                .inflate(R.layout.item_place_detail,parent,false);
        return new ContentViewHolder(itemView);
    }

    private void loadData() {

        Call<ResponseModel<PlaceResponseModel>> call = service.getPlaceService().getPlaceDetail(placeId);
        call.enqueue(new Callback<ResponseModel<PlaceResponseModel>>() {
            @Override
            public void onResponse(Call<ResponseModel<PlaceResponseModel>> call, Response<ResponseModel<PlaceResponseModel>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        place = response.body().getData();
                        notifyDataSetChanged();

                    }
                }
            }

            @Override
            public void onFailure(Call<ResponseModel<PlaceResponseModel>> call, Throwable t) {

            }
        });
    }

    @Override
    public void onBindViewHolder(ContentViewHolder holder, int position) {
        if (place!=null){
            holder.txtPhoneNumber.setText(place.getPhoneNumber());
            holder.txtEmail.setText(place.getEmail());
            holder.txtAddress.setText(place.getAddressString());
            holder.txtDescription.setText(place.getDescription());
        }
    }

    @Override
    public int getItemCount() {
        return 1;
    }

    public class ContentViewHolder extends RecyclerView.ViewHolder {
        TextView txtPhoneNumber;
        TextView txtEmail;
        TextView txtAddress;
        TextView txtDescription;

        public ContentViewHolder(View itemView) {
            super(itemView);

            txtPhoneNumber = (TextView) itemView.findViewById(R.id.txt_place_detail_phone_number);
            txtEmail = (TextView) itemView.findViewById(R.id.txt_place_detail_email);
            txtAddress = (TextView) itemView.findViewById(R.id.txt_place_detail_address);
            txtDescription = (TextView) itemView.findViewById(R.id.txt_place_detail_description);

        }
    }
}

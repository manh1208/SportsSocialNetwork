package com.capstone.sportssocialnetwork.firebase;

import android.util.Log;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.model.response.ResponseModel;
import com.capstone.sportssocialnetwork.service.RestService;
import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.capstone.sportssocialnetwork.utils.SharePreferentName;
import com.google.firebase.iid.FirebaseInstanceId;
import com.google.firebase.iid.FirebaseInstanceIdService;

import retrofit2.Call;
import retrofit2.Callback;
import retrofit2.Response;

/**
 * Created by ManhNV on 10/27/16.
 */

public class MyFirebaseInstanceIDService extends FirebaseInstanceIdService
{

    @Override
    public void onTokenRefresh() {
        String refreshedToken = FirebaseInstanceId.getInstance().getToken();
        Log.d("Ahihi", "Refreshed token: " + refreshedToken);

        // TODO: Implement this method to send any registration to your app's servers.
        sendRegistrationToServer(refreshedToken);
    }

    private void sendRegistrationToServer(final String refreshedToken) {
        String userId = DataUtils.getINSTANCE(this).getPreferences().getString(SharePreferentName.SHARE_USER_ID,"");
        RestService service = new RestService();
        service.getAccountService().sentToken(userId,refreshedToken).enqueue(new Callback<ResponseModel<String>>() {
            @Override
            public void onResponse(Call<ResponseModel<String>> call, Response<ResponseModel<String>> response) {
                if (response.isSuccessful()){
                    if (response.body().isSucceed()){
                        Log.d("Noti", response.body().getErrorsString());
                    }else{
                        Log.d("Noti", response.body().getErrorsString());
                    }
                }else{

                }
            }

            @Override
            public void onFailure(Call<ResponseModel<String>> call, Throwable t) {
                Log.d("Noti", getString(R.string.failure));
            }
        });
    }
}

package com.capstone.sportssocialnetwork.service;

import com.capstone.sportssocialnetwork.utils.DataUtils;
import com.google.gson.Gson;
import com.google.gson.GsonBuilder;

import okhttp3.OkHttpClient;
import okhttp3.logging.HttpLoggingInterceptor;
import retrofit2.Retrofit;
import retrofit2.converter.gson.GsonConverterFactory;

public class RestService {
    private final String url = DataUtils.URL;
    private Retrofit retrofit;
    private ISampleService sampleService;
    private ISocialNetworkService socialNetworkService;

    public RestService() {
        Gson gson = new GsonBuilder()
                .setDateFormat("yyyy-MM-dd'T'HH:mm:ssZ")
                .create();
        HttpLoggingInterceptor logging = new HttpLoggingInterceptor();
        logging.setLevel(HttpLoggingInterceptor.Level.BODY);

        OkHttpClient.Builder httpClient = new OkHttpClient.Builder();
        httpClient.addInterceptor(logging);
        retrofit = new Retrofit.Builder()
                .baseUrl(url)
                .addConverterFactory(GsonConverterFactory.create(gson))
                .client(httpClient.build())
                .build();
        sampleService = retrofit.create(ISampleService.class);
        socialNetworkService = retrofit.create(ISocialNetworkService.class);
    }

    public ISampleService getSampleService() {
        return sampleService;
    }

    public ISocialNetworkService getSocialNetworkService() {
        return socialNetworkService;
    }
}

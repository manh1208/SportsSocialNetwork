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
    private IAccountService accountService;
    private IPlaceService placeService;
    private IOrderService orderService;
    private IPostService postService;
    private IGroupService groupService;
    private INotificationService notificationService;
    private INewsService newsService;

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
        accountService = retrofit.create(IAccountService.class);
        placeService = retrofit.create(IPlaceService.class);
        orderService = retrofit.create(IOrderService.class);
        postService = retrofit.create(IPostService.class);
        groupService = retrofit.create(IGroupService.class);
        notificationService = retrofit.create(INotificationService.class);
        newsService = retrofit.create(INewsService.class);

    }

    public ISampleService getSampleService() {
        return sampleService;
    }

    public ISocialNetworkService getSocialNetworkService() {
        return socialNetworkService;
    }

    public IAccountService getAccountService() {
        return accountService;
    }

    public IPlaceService getPlaceService() {
        return placeService;
    }

    public IOrderService getOrderService() {
        return orderService;
    }

    public IPostService getPostService() {
        return postService;
    }

    public IGroupService getGroupService() {
        return groupService;
    }

    public INotificationService getNotificationService() {
        return notificationService;
    }

    public INewsService getNewsService() {
        return newsService;
    }
}

package com.capstone.sportssocialnetwork.firebase;

import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.media.RingtoneManager;
import android.net.Uri;
import android.support.v4.app.NotificationCompat;
import android.util.Log;
import android.widget.Toast;

import com.capstone.sportssocialnetwork.R;
import com.capstone.sportssocialnetwork.activity.InvitationActivity;
import com.capstone.sportssocialnetwork.activity.MainBottomBarActivity;
import com.capstone.sportssocialnetwork.activity.MyOrderActivity;
import com.capstone.sportssocialnetwork.activity.PostDetailActivity;
import com.capstone.sportssocialnetwork.enumerable.NotificationTypeEnum;
import com.google.firebase.messaging.FirebaseMessagingService;
import com.google.firebase.messaging.RemoteMessage;

import java.util.Map;

/**
 * Created by ManhNV on 10/27/16.
 */

public class MyFirebaseMessagingService extends FirebaseMessagingService {

    private static final String TAG = "Demo";

    @Override
    public void onMessageReceived(RemoteMessage remoteMessage) {
        super.onMessageReceived(remoteMessage);
        Log.d(TAG, "From: " + remoteMessage.getFrom());
        sendNotification(remoteMessage.getData());
        // Check if message contains a data payload.
//        if (remoteMessage.getData().size() > 0) {
//            Log.d(TAG, "Message data payload: " + remoteMessage.getData());
//        }
//
//        // Check if message contains a notification payload.
//        if (remoteMessage.getNotification() != null) {
//            Log.d(TAG, "Message Notification Body: " + remoteMessage.getNotification().getBody());
//            sendNotification(remoteMessage.getData());
//        }

    }

    private void sendNotification(Map<String, String> data) {

        String message = data.get("Message");
        String title = data.get("Title");
//        int eventId = Integer.parseInt(data.get("EventId"));
        int notificationId = Integer.parseInt(data.get("Id"));
        Log.i("Otto", "Gởi message");
//        String ipAddress = data.getString("IpAddress");
//        BusStation.getBus().post(new Message(ipAddress));
        Uri defaultSoundUri = RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION);
        NotificationCompat.Builder notificationBuilder = new NotificationCompat.Builder(this)
                .setSmallIcon(R.drawable.image_logo)
                .setContentTitle("Sport Social Network")
                .setContentText(message)
                .setAutoCancel(true)
                .setSound(defaultSoundUri)
                .setPriority(android.app.Notification.PRIORITY_HIGH);
        String typeStr = data.get("Type");
        int type = 0;
        try {
            type = Integer.parseInt(typeStr);

        } catch (Exception e) {

        }
        Intent intent;

        if (NotificationTypeEnum.POST == NotificationTypeEnum.fromInteger(type)) {
            String postIdStr = data.get("PostId");
            int postId = 0;
            try {
                postId = Integer.parseInt(postIdStr);

            } catch (Exception e) {

            }
            intent = new Intent(this, PostDetailActivity.class);
            intent.putExtra("postId", postId);
        } else if (NotificationTypeEnum.ORDER == NotificationTypeEnum.fromInteger(type)) {
            intent = new Intent(this, MyOrderActivity.class);
        } else if (NotificationTypeEnum.INVITATION == NotificationTypeEnum.fromInteger(type)) {
            intent = new Intent(this, InvitationActivity.class);
        } else {
            intent = new Intent(this, MainBottomBarActivity.class);
        }


//            intent.putExtra("eventId", eventId);
//            intent.putExtra("start",true);
//                intent.putExtra("notiId", notificationId);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        PendingIntent pendingIntent = PendingIntent.getActivity(this, notificationId /* Request code */, intent,
                PendingIntent.FLAG_ONE_SHOT);
        notificationBuilder.setContentIntent(pendingIntent);

        NotificationManager notificationManager =
                (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.notify(notificationId /* ID of notification */, notificationBuilder.build());
    }
}

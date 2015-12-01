package com.klab.ap_refresh_localpush_sample_android;

import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

import java.util.Calendar;

/**
 * Created by kensei on 12/1/15.
 */
public class SampleClientPlugin {

    private static final String TAG = SampleClientPlugin.class.getSimpleName();
    private static int lastNotificatinId;
    private String objectName = "";

    public SampleClientPlugin() {
    }

    public void Init(final String gameObject) {

        Log.d(TAG, "Init:" + gameObject);
        lastNotificatinId = -1;
        objectName = gameObject;
    }

    public void SetLocalNotification(int notificationId, String message, String title, int secAfter) {

        Log.i(TAG, "sendNotification notificationId:" + String.valueOf(notificationId) + " secAfter:" + String.valueOf(secAfter));

        // 設定されたアラームがあればキャンセル
        if (lastNotificatinId != -1) {
            CancelLocalNotification(notificationId);
        }

        // make intent
        Context context = UnityPlayer.currentActivity.getApplicationContext();
        Intent intent = new Intent(context, LocalNotificationReceiver.class);
        intent.putExtra("TITLE", title);
        intent.putExtra("MESSAGE", message);
        intent.putExtra("NOTIFICATION_ID", notificationId);
        intent.setAction("com.klab.ap-refresh-localpush-sample.intent.action.LOCALPUSH");

        // set alarm time
        Calendar calendar = Calendar.getInstance();
        calendar.setTimeInMillis(System.currentTimeMillis());
        calendar.add(Calendar.SECOND, secAfter);

        // alarm set
        PendingIntent sender = PendingIntent.getBroadcast(context,  notificationId, intent, PendingIntent.FLAG_CANCEL_CURRENT);
        AlarmManager alarm = (AlarmManager)context.getSystemService(Context.ALARM_SERVICE);
        alarm.set(AlarmManager.RTC_WAKEUP, calendar.getTimeInMillis() , sender);

        lastNotificatinId = notificationId;
    }

    public void CancelLocalNotification(int notificationId) {

        Log.i(TAG, "CancelLocalNotification:" + String.valueOf(notificationId));

        Context context = UnityPlayer.currentActivity.getApplicationContext();

        Intent intent = new Intent(context, LocalNotificationReceiver.class);
        intent.setAction("com.klab.ap-refresh-localpush-sample.intent.action.LOCALPUSH");
        PendingIntent pendingIntent = PendingIntent.getBroadcast(context, notificationId, intent, PendingIntent.FLAG_CANCEL_CURRENT);

        AlarmManager am = (AlarmManager)context.getSystemService(Context.ALARM_SERVICE);
        am.cancel(pendingIntent);

        lastNotificatinId = -1;
    }
}

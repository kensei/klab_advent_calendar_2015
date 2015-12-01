package com.klab.ap_refresh_localpush_sample_android;

import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.app.Service;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.pm.ApplicationInfo;
import android.content.pm.PackageManager;
import android.content.pm.PackageManager.NameNotFoundException;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

/**
 * Created by kensei on 12/1/15.
 */
public class LocalNotificationReceiver extends BroadcastReceiver {

    private static final String TAG = LocalNotificationReceiver.class.getSimpleName();

    @Override
    public void onReceive(Context context, Intent intent) {

        Log.d(TAG, "onReceive");

        String title = intent.getStringExtra("TITLE");
        String message = intent.getStringExtra("MESSAGE");
        Integer notificationId = intent.getIntExtra("NOTIFICATION_ID", 0);

        PendingIntent pendingIntent = PendingIntent.getActivity(context, 0, intent, PendingIntent.FLAG_CANCEL_CURRENT);

        // generate app icon
        final PackageManager pm = context.getPackageManager();
        ApplicationInfo applicationInfo = null;
        try {
            applicationInfo = pm.getApplicationInfo(context.getPackageName(),PackageManager.GET_META_DATA);
        } catch (NameNotFoundException e) {
            e.printStackTrace();
            return;
        }
        final int appIconResId = applicationInfo.icon;
        Bitmap largeIcon = BitmapFactory.decodeResource(context.getResources(), appIconResId);

        // make notification
        NotificationCompat.Builder builder = new NotificationCompat.Builder(context);
        builder.setContentIntent(pendingIntent);
        builder.setTicker(title);                       // ステータスバーに届くテキスト
        builder.setSmallIcon(appIconResId);             // アイコン
        builder.setContentTitle(title);                 // タイトル
        builder.setContentText(message);                // 本文（サブタイトル）
        builder.setLargeIcon(largeIcon);                // 開いた時のアイコン
        builder.setWhen(System.currentTimeMillis());    // 通知に表示される時間
        builder.setDefaults(Notification.DEFAULT_ALL);
        builder.setAutoCancel(true);

        NotificationManager manager = (NotificationManager) context.getSystemService(Service.NOTIFICATION_SERVICE);
        manager.notify(notificationId, builder.build());
    }
}

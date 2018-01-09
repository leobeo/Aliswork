using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

using Android.Media;
using Android.Support.V4.App;
using Android.Util;
using Aliswork.Droid;

using System.Net;
using Newtonsoft.Json;
using Aliswork;



[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
//GET_ACCOUNTS is only needed for android versions 4.0.3 and below
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]

namespace Gcm.Client
{
    [BroadcastReceiver(Permission = Gcm.Client.Constants.PERMISSION_GCM_INTENTS)]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_MESSAGE }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_REGISTRATION_CALLBACK }, Categories = new string[] { "@PACKAGE_NAME@" })]
    [IntentFilter(new string[] { Gcm.Client.Constants.INTENT_FROM_GCM_LIBRARY_RETRY }, Categories = new string[] { "@PACKAGE_NAME@" })]
    public class PushHandlerBroadcastReceiver : GcmBroadcastReceiverBase<GcmService>
    {
        public static string[] SENDER_IDS = new string[] {Constants.SENDER_ID };
    }
    [Service]
    public class GcmService : GcmServiceBase
    {
        public static string RegistrationID { get; private set; }

        public GcmService()
            : base(PushHandlerBroadcastReceiver.SENDER_IDS) { }

        protected override void OnRegistered(Context context, string registrationId)
        {
            Log.Verbose("PushHandlerBroadcastReceiver", "GCM Registered: " + registrationId);
            RegistrationID = registrationId;
            var wc = new WebClient();

             Aliswork.ContentGlobal.registrationId = registrationId;
            //string uId = Xamarin.Forms.Application.Current.Properties["uId"].ToString();

            //string vm = "{\"uid\" : \"" + uId + ",\"registrationId\" : \"" + registrationId + "\" }";
           /* string vm = "{\"uid\" : \"registrationId\",\"registrationId\" : \"" + registrationId + "\" }";
            var dataString = JsonConvert.SerializeObject(vm);
            wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
            wc.UploadString(new Uri("https://us-central1-xamarinfcm-ce873.cloudfunctions.net/pushData"), "POST", vm);*/
        }

        protected override void OnMessage(Context context, Intent intent)
        {
            Log.Info("PushHandlerBroadcastReceiver", "GCM Message Received!");
            ContentGlobal.dictNotiRecieve.Clear();
            var msg = new StringBuilder();
            
            Log.Debug("Recieve Notification", "context :" + context.ToString());
            Log.Debug("Recieve Notification", "intent :" + intent.ToString());

            if (intent != null && intent.Extras != null)
            {
                foreach (var key in intent.Extras.KeySet())
                {
                    msg.AppendLine(key + "=" + intent.Extras.Get(key).ToString());

                    ContentGlobal.dictNotiRecieve.Add(key, intent.Extras.Get(key).ToString());

                    Log.Debug("Recieve Notification---------------------------------------------------------------------------------------", intent.Extras.Get(key).ToString());
                    if (key== "path")
                    {
                        ContentGlobal.path = intent.Extras.Get(key).ToString();
                    }
                }
                    
            }

            //Store the message
            var prefs = GetSharedPreferences(context.PackageName, FileCreationMode.Private);
            var edit = prefs.Edit();
            edit.PutString("last_msg", msg.ToString());
            edit.Commit();

            string title = intent.Extras.GetString("title");
            if (!string.IsNullOrEmpty(title))
            {
                string message = intent.Extras.GetString("message");
                if (!string.IsNullOrEmpty(message))
                {
                    createNotification(title, message);
                    return;
                }
               /* else
                {
                    createNotification(title, "");
                    return;
                } */
            }

           

            createNotification("Unknown message details", msg.ToString());
        }

        void createNotification(string title, string desc)
        {
            //Create notification
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            //Create an intent to show ui
            var uiIntent = new Intent(this, typeof(MainActivity));

            //Use Notification Builder
            Android.Support.V7.App.NotificationCompat.Builder builder = new Android.Support.V7.App.NotificationCompat.Builder(this);

            //Create the notification
            //we use the pending intent, passing our ui intent over which will get called
            //when the notification is tapped.
            var notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 0, uiIntent, 0))
			                          .SetSmallIcon(Aliswork.Droid.Resource.Drawable.logo_icon)
			                          .SetTicker(title)
			                          .SetContentTitle(title)
			                          .SetContentText(desc)

                    //Set the notification sound
                    .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))

                    //Auto cancel will remove the notification once the user touches it
                    .SetAutoCancel(true).Build();

            //Show the notification
            notificationManager.Notify(1, notification);
        }

        protected override void OnUnRegistered(Context context, string registrationId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "Unregistered RegisterationId : " + registrationId);
        }

        protected override void OnError(Context context, string errorId)
        {
            Log.Error("PushHandlerBroadcastReceiver", "GCM Error: " + errorId);
        }

    }
}
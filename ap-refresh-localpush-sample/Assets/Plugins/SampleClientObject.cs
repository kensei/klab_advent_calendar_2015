using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public class SampleClientObject : MonoBehaviour
{
	#region inner classes, enum, and structs

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
#elif UNITY_IPHONE
	IntPtr sampleClient;
#elif UNITY_ANDROID
	AndroidJavaObject sampleClient;
#endif

#if UNITY_EDITOR || UNITY_STANDALONE_OSX
#elif UNITY_IPHONE
	[DllImport("__Internal")]
	private static extern IntPtr _SampleClientPlugin_Init(string gameObject);
	[DllImport("__Internal")]
	private static extern int _SampleClientPlugin_Destroy(IntPtr instance);
	[DllImport("__Internal")]
	private static extern void _SampleClientPlugin_SetLocalNotification(IntPtr instance, int id, string title, string msg, int interval);
	[DllImport("__Internal")]
	private static extern void _SampleClientPlugin_CancelLocalNotification(IntPtr instance, int id);
#endif

	#endregion

	#region Public Method

	public void Initialize()
	{
#if UNITY_EDITOR || UNITY_STANDALONE_OSX
#elif UNITY_IPHONE
		sampleClient = _SampleClientPlugin_Init(name);
#elif UNITY_ANDROID
		sampleClient = new AndroidJavaObject("com.klab.ap_refresh_localpush_sample_android.SampleClientPlugin");
		sampleClient.Call("Init", "if plugin send to message unity then set this section receiver gameobject name");
#endif
	}

	public void SetLocalNotificationInterval(int id, string title, string msg, int interval)
	{
		Debug.Log("SetLocalNotificationInterval:" + interval);
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
#elif UNITY_IPHONE
		if (sampleClient == IntPtr.Zero)
			return;
		_SampleClientPlugin_SetLocalNotification(sampleClient, id, title, msg, interval);
#elif UNITY_ANDROID
		if(sampleClient == null)
			return;
		sampleClient.Call("SetLocalNotification", id, title, msg, interval);
#endif
	}

	public void CancelLocalNotification(int id)
	{
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN
#elif UNITY_IPHONE
		if (sampleClient == IntPtr.Zero)
			return;
		_SampleClientPlugin_CancelLocalNotification(sampleClient, id);
#elif UNITY_ANDROID
		if(sampleClient == null)
			return;
		sampleClient.Call("CancelLocalNotification", id);
#endif
	}

	#endregion
}

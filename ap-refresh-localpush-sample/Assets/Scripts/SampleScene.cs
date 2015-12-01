using UnityEngine;
using System;
using System.Collections;

public class SampleScene : MonoBehaviour
{
	#region inner classes, enum, and structs

	readonly string PREFS_KEY_AP_COUNT = "AP_COUNT";
	readonly string PREFS_KEY_AP_UPDATE_DATETIME = "AP_UPDATE_DATETIME";
	readonly int AP_MAX = 5;
	readonly int AP_REFRESH_INTERVAL = 20;

	int apCount;
	float apUpdateTime;

	#endregion

	#region override unity methods

	void Awake()
	{
		Application.targetFrameRate = 10;
		apCount = PlayerPrefs.GetInt(PREFS_KEY_AP_COUNT, AP_MAX);
		apUpdateTime = Time.realtimeSinceStartup;
	}

	void Start()
	{
		if (apCount >= AP_MAX)
			return;
		if (!PlayerPrefs.HasKey(PREFS_KEY_AP_UPDATE_DATETIME))
			return;
		
		var lastAwakeInterval = DateTimeOffset.Now - DateTimeOffset.Parse(PlayerPrefs.GetString(PREFS_KEY_AP_UPDATE_DATETIME));
		if (lastAwakeInterval.TotalSeconds > GetApMaxSec(apCount, apUpdateTime)) {
			apCount = AP_MAX;
			apUpdateTime = Time.realtimeSinceStartup;
			UpdateAp(AP_MAX);
			UpdateApUpdateDatetime();
		}
		else
		{
			apUpdateTime -= (float) lastAwakeInterval.TotalSeconds;
		}
	}

	void Update()
	{
		if (apCount >= AP_MAX)
			return;
		
		var restCount = Mathf.CeilToInt((float) GetApMaxSec(apCount, apUpdateTime) / (float)AP_REFRESH_INTERVAL);
		if (apCount != (AP_MAX - restCount))
		{
			RecoverAp();
		}
	}

	void OnGUI()
	{
		GUI.Box(new Rect(10, 10, 200, 300), "Sample");

		GUI.Label(new Rect(30, 30, 200, 100), "AP:" + apCount);
		GUI.Label(new Rect(30, 50, 200, 100), "Time:" + GetApMaxSec(apCount, apUpdateTime));

		if (GUI.Button(new Rect(30, 80, 160, 40), "Use AP"))
		{
			UseAp();
		}

		if (GUI.Button(new Rect(30, 140, 160, 40), "Recover AP"))
		{
			RecoverAp();
		}
	}

	#endregion

	#region Private Method

	void UseAp()
	{
		Debug.Log("Use AP:" + apCount);
		apCount--;
		apUpdateTime = Time.realtimeSinceStartup;

		UpdateAp(apCount);
		UpdateApUpdateDatetime();
	}

	void RecoverAp()
	{
		Debug.Log("RecoverAp:" + apCount);

		if (apCount >= AP_MAX)
			return;
		
		apCount++;
		apUpdateTime = Time.realtimeSinceStartup;
		UpdateAp(apCount);
		UpdateApUpdateDatetime();
	}

	int GetApMaxSec(int ap, float updateTime)
	{
		if (ap >= AP_MAX)
			return 0;

		var apDiff = AP_MAX - ap;
		var updateTimeSpan = Time.realtimeSinceStartup - updateTime;
		var restTime = Mathf.CeilToInt((AP_REFRESH_INTERVAL * apDiff) - Mathf.Abs(updateTimeSpan));

		return (restTime > 0) ? restTime : 0;
	}

	void UpdateAp(int ap)
	{
		PlayerPrefs.SetInt(PREFS_KEY_AP_COUNT, ap);
	}

	void UpdateApUpdateDatetime()
	{
		PlayerPrefs.SetString(PREFS_KEY_AP_UPDATE_DATETIME, DateTimeOffset.Now.ToString ("yyyy/MM/dd HH:mm:ss"));
	}

	#endregion
}

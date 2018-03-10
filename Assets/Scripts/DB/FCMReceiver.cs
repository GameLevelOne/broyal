using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FCMReceiver : MonoBehaviour {

	Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

	// Use this for initialization
	void Start () {
		Firebase.FirebaseApp.CheckAndFixDependenciesAsync ().ContinueWith(task => {
			dependencyStatus = task.Result;
			if (dependencyStatus == Firebase.DependencyStatus.Available) {
				InitializeFirebase();
				MessageLog("FCM Init","Firebase Initialized");
			} else {
				MessageLog("FCM Init","Firebase Not Initialized");
			}
		});
	}
	
	// Update is called once per frame
	void MessageLog (string title, string log) {
		if (DBManager.API.debugConsole) {
			int idx = DBManager.API.debugConsole.SetRequest (title);
			DBManager.API.debugConsole.SetResult (log,idx);
		}
	}

	void InitializeFirebase() {
		Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
		Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
		Firebase.Messaging.FirebaseMessaging.Subscribe("auction_reminder");
		Firebase.Messaging.FirebaseMessaging.Subscribe("auction_won");
		Firebase.Messaging.FirebaseMessaging.Subscribe("claim_reminder");
		Firebase.Messaging.FirebaseMessaging.Subscribe("miss_you");
	}
	public virtual void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e) {
		string s="";
		var notification = e.Message.Notification;
		GameObject g = GameObject.FindGameObjectWithTag ("HangingNotification"); 

		if (notification!=null) {
			if (g != null) {
				g.GetComponent<HangingNotification> ().ShowNotification (notification.Body);
			}
		}

		if (notification != null) {
			s += "title: " + notification.Title +"\n";
			s += "body: " + notification.Body + "\n";
		}
		if (e.Message.From.Length > 0)
			s += "from: " + e.Message.From + "\n";
		if (e.Message.Link != null) {
			s += "link: " + e.Message.Link.ToString() + "\n";
		}
		if (e.Message.Data.Count > 0) {
			s += "data:\n";
			foreach (System.Collections.Generic.KeyValuePair<string, string> iter in
				e.Message.Data) {
				s += "  " + iter.Key + ": " + iter.Value + "\n";
			}
		}
		MessageLog("FCM Message",s);


	}

	public virtual void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token) {
		DBManager.API.firebaseToken = token.Token;
	}
	// End our messaging session when the program exits.
	public void OnDestroy() {
		Firebase.Messaging.FirebaseMessaging.MessageReceived -= OnMessageReceived;
		Firebase.Messaging.FirebaseMessaging.TokenReceived -= OnTokenReceived;
	}
}

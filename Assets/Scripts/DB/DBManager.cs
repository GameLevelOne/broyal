using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using SimpleJSON;

public class DBManager : MonoBehaviour {
//===========================Singleton Coding=======================================================
	static DBManager _instance = null;
	void Awake() {
		if (_instance == null) {
			_instance = this;
		} else if (_instance!=this) {
			Destroy (gameObject);			
		}
		DontDestroyOnLoad (gameObject);
	}
	public static DBManager API {
		get {
			return _instance;
		}
	}
//==================================================================================================


	public DBManagerSettings config;
	string tokenType = null;
	string accessToken = null;

	public void UserRegistration(bool termsCondition, int gender, string userName, string password, string mobile, string email,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userRegistrationAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"termsCondition\":"+termsCondition+",\n"+
			"\"gender\":"+gender+",\n"+
			"\"username\":\""+userName+"\",\n"+
			"\"password\":\""+password+"\",\n"+
			"\"mobile\":\""+mobile+"\",\n"+
			"\"email\":\""+email+"\"\n"+
			"}";

		DebugMsg ("USER REGISTRATION Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithKey(),onComplete, onError);
	}

	public void VerifyUser(string userName, string verificationOTP, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.verifyUserAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"username\":\""+userName+"\",\n"+
			"\"verificationOtp\":\""+verificationOTP+"\"\n"+
			"}";

		DebugMsg ("VERIFY USER Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithKey(),onComplete, onError);
	}

	public void UserLogin(string userName, string password, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userLoginAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"username\":\""+userName+"\",\n"+
			"\"password\":\""+password+"\"\n"+
			"}";

		DebugMsg ("USER LOGIN Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeader(),
			(response)=> {
				JSONNode jdata = JSON.Parse(response);
				tokenType = jdata["token_type"];
				accessToken = jdata["access_token"];
				if (onComplete!=null)
					onComplete(response);
			}, onError);
	}

	public void GetUserProfile(System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getUserProfileAPI;
		DebugMsg ("USER LOGIN Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}



//===========================Utilities==============================================================

	WWW PostRequest(string url, byte[] data, Dictionary<string,string> postHeader, System.Action<string> onComplete, System.Action<string> onError) {
		if (postHeader == null) {
			if (onError!=null)
				onError("{\"errors\":\"NOT AUTHORIZED\"}");
			return null;
		} else {
			WWW www = new WWW (url, data, postHeader);
			StartCoroutine (WaitForRequest (www, onComplete, onError));
			return www;
		}
	}

	IEnumerator WaitForRequest(WWW www, System.Action<string> onComplete, System.Action<string> onError) {
		yield return www;
		if (www.error == null) {
			DebugMsg ("","RESULT: \n"+www.text);
			if (onComplete!=null)
				onComplete(www.text);
		} else {
			DebugError ("ERROR: "+www.error);
			if (onError!=null)
				onError(www.text);
		}
	}

	void DebugMsg(string msg = "", string cmsg = "")
	{
		if (config.debugMode) {
			if (msg!="")
				Debug.Log (config.messagePrefix+msg);
			if ((config.comprehensiveDebug) && (cmsg!=""))
				Debug.Log (config.messagePrefix+cmsg);
		}
	}

	void DebugError(string msg = "", string cmsg = "")
	{
		if (msg!="")
			Debug.LogError (config.messagePrefix+msg);
		if ((config.debugMode) && (config.comprehensiveDebug) && (cmsg!="")) 
			Debug.LogError (config.messagePrefix+cmsg);
	}

	Dictionary<string,string> CreateHeader() {
		Dictionary<string,string> header = new Dictionary<string, string> ();
		header.Add ("Content-Type", "application/json");
		return header;
	}
	Dictionary<string,string> CreateHeaderWithKey() {
		Dictionary<string,string> header = CreateHeader();
		header.Add ("royalBidKey", config.royalBidKey);
		header.Add ("royalBidSecret", config.royalBidSecret);
		return header;
	}
	Dictionary<string,string> CreateHeaderWithAuthorization() {
		Dictionary<string,string> header = CreateHeaderWithKey();
		if ((accessToken == null) || (accessToken == "")) {
			DebugError ("NOT AUTHORIZED!");
			return null;
		} else {
			header.Add ("Authorization", tokenType +" "+ accessToken);
			return header;
		}
	}

//==================================================================================================

}

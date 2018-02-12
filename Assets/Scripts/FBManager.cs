using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public struct FBData {
	public string firstName;
	public string lastName;
	public int gender;
	public string accessToken;
	public string id;
	public string email;
}


public class FBManager : MonoBehaviour {
	private static FBManager instance = null;
	private bool fbLogin = false;

	public static FBManager Instance{get{return instance;}}
	public bool FBLogin{ get { return fbLogin;}}

	System.Action<string> onFBSuccess;
	System.Action<string> onFBError;
	public FBData fbData;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance=this;
		}

		DontDestroyOnLoad(this.gameObject);
	}

	void Start ()
	{
		if (!FB.IsInitialized) {
			FB.Init (OnInitFBComplete, null, null);
		} else {
			FB.ActivateApp();
		}

	}

	void OnInitFBComplete (){
		Debug.Log("Facebook API initialized");
		if (FB.IsLoggedIn) {
			fbLogin = true;
		} else {
			fbLogin = false;
		}
	}

	void OnInitFBCompleteThenLogin (){
		OnInitFBComplete ();
		FB.LogInWithReadPermissions(new List<string>{"public_profile", "email", "user_friends"},FBHandleLoginResult);
	}

	public void OnFBLogin (System.Action<string> onComplete , System.Action<string> onError)
	{
		onFBSuccess = onComplete;
		onFBError = onError;
		fbData = new FBData ();
		if (!FB.IsInitialized) {
			FB.Init (OnInitFBCompleteThenLogin, null, null);
		} else {
			FB.LogInWithReadPermissions(new List<string>{"public_profile", "email", "user_friends"},FBHandleLoginResult);
		}
	}
	void FBHandleLoginResult (IResult result)
	{
		if (result == null) {
			Debug.Log (result.ToString());
		} else if (!string.IsNullOrEmpty (result.Error)) {
			Debug.Log ("FB Login Error");
			if (onFBError != null)
				onFBError("{\"errors\":\""+result.RawResult+"\"}");
		} else if (result.Cancelled) {
			Debug.Log ("FB Login Cancelled");
			if (onFBError != null)
				onFBError("{\"errors\":\"CANCELLED\"}");
		} else if (!string.IsNullOrEmpty (result.RawResult)) { //success
			Debug.Log("FB Login success");
			fbData.accessToken = result.ResultDictionary ["access_token"].ToString ();
			fbLogin=true;
			GetFBDetails ();
		}
	}
	void GetFBDetails(){
		FB.API ("/me?fields=first_name,last_name,gender,id,email", HttpMethod.GET, OnFBGetDetails);

	}
	void OnFBGetDetails(Facebook.Unity.IGraphResult result) {
		fbData.firstName = result.ResultDictionary["first_name"].ToString();
		fbData.lastName = result.ResultDictionary["last_name"].ToString();
		fbData.gender = result.ResultDictionary["gender"].ToString() == "male" ? 1 : 2;
		fbData.id = result.ResultDictionary["id"].ToString();
		fbData.email = result.ResultDictionary["email"].ToString();

		if (onFBSuccess != null) {
			onFBSuccess ("{}");
		}
	}		
}

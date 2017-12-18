using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBManager : MonoBehaviour {
	private static FBManager instance = null;
	private bool fbLogin = false;

	public static FBManager Instance{get{return instance;}}
	public bool FBLogin{ get { return fbLogin;}}


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
			Debug.Log("init fb");
		} else {
			FB.ActivateApp();
		}

	}

	void OnInitFBComplete (){
		Debug.Log(string.Format("OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",FB.IsLoggedIn,FB.IsInitialized));

//		if(FB.IsLoggedIn){
//			GetFBFirstName();
//		}
	}

	public void OnFBLogin ()
	{
		if (!FB.IsInitialized) {
			FB.Init (OnInitFBComplete, null, null);
		} else {
			FB.LogInWithReadPermissions(new List<string>{"public_profile", "email", "user_friends"},this.HandleLoginResult);
		}
	}

	void GetFBFirstName(){
		FB.API("/me?fields=first_name",HttpMethod.GET,OnFBGetFirstName);
	}

	void GetFBLastName(){
		FB.API("/me?fields=last_name",HttpMethod.GET,OnFBGetLastName);
	}

	void GetFBEmail(){
		FB.API ("/me?fields=email", HttpMethod.GET, OnFBGetEmail);
	}

	void GetFBProfilePicture(){
		FB.API ("/me?fields=picture", HttpMethod.GET, OnFBGetProfilePicture);
	}

	void GetFBGender(){
		FB.API ("/me?fields=gender", HttpMethod.GET, OnFBGetGender);
	}

	void GetFBAgeRange(){
		FB.API ("/me?fields=age_range", HttpMethod.GET, OnFBGetAgeRange);
	}

	void GetFBFriends(){
		FB.API ("/me?fields=friends", HttpMethod.GET, OnFBGetFriends);
	}

	void OnFBGetFirstName(Facebook.Unity.IGraphResult result){
		string fbFirstName = result.ResultDictionary["first_name"].ToString();
		Debug.Log("FBFirstName: "+fbFirstName);
	}

	void OnFBGetLastName(Facebook.Unity.IGraphResult result){
		string fbLastName = result.ResultDictionary["last_name"].ToString();
		Debug.Log("FBLastName: "+fbLastName);
	}

	void OnFBGetEmail(Facebook.Unity.IGraphResult result){
		string fbEmail = result.ResultDictionary["email"].ToString();
		Debug.Log("FBEmail: "+fbEmail);
	}

	void OnFBGetProfilePicture(Facebook.Unity.IGraphResult result){
		Debug.Log (result.RawResult);

	}

	void OnFBGetGender(Facebook.Unity.IGraphResult result){
		string fbGender = result.ResultDictionary ["gender"].ToString ();
		Debug.Log ("FBGender: " + fbGender);
	}

	void OnFBGetAgeRange(Facebook.Unity.IGraphResult result){
		string fbAgeRange = result.ResultDictionary ["age_range"].ToString ();
		Debug.Log ("FBAgeRange: " + fbAgeRange);
	}

	void OnFBGetFriends(Facebook.Unity.IGraphResult result){
		string fbFriends = result.ResultDictionary ["friends"].ToString ();
		Debug.Log ("FBFriends: " + fbFriends);
	}

	void GetFBDetails(){
//		GetFBEmail ();
//		GetFBFirstName ();
//		GetFBLastName ();
		GetFBProfilePicture ();
//		GetFBGender ();
//		GetFBAgeRange ();
//		GetFBFriends ();
	}

	void HandleLoginResult (IResult result)
	{
		if (result == null) {
			Debug.Log (result.ToString());
		} else if (!string.IsNullOrEmpty (result.Error)) {
			Debug.Log ("error login 01: " + result.ToString());
		} else if (result.Cancelled) {
			Debug.Log ("error login 02: " + result.ToString());
		} else if (!string.IsNullOrEmpty (result.RawResult)) { //success
			Debug.Log("login success");
			Debug.Log (result.RawResult);
			fbLogin=true;
			GetFBDetails ();
		}
	}

}

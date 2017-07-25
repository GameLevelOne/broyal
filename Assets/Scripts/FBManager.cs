using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class FBManager : MonoBehaviour {
	private static FBManager instance = null;

	public static FBManager Instance{get{return instance;}}

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
			return;
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

		if(FB.IsLoggedIn){
			GetFBName();
		}
	}

	public void OnFBLogin ()
	{
		if (!FB.IsInitialized) {
			FB.Init (OnInitFBComplete, null, null);
		} else {
			FB.LogInWithReadPermissions(new List<string>{"public_profile","email"},this.HandleLoginResult);
		}
	}

	void GetFBName(){
		FB.API("/me?fields=first_name",HttpMethod.GET,OnFBGetName);
	}

	void OnFBGetName(Facebook.Unity.IGraphResult result){
		string fbName = result.ResultDictionary["first_name"].ToString();
		Debug.Log("FBFirstName: "+fbName);
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
			GetFBName();
		}
	}

}

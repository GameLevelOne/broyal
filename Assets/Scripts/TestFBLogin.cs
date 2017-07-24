using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;

public class TestFBLogin : MonoBehaviour {
	public Text logText;
	// Use this for initialization
	void Start ()
	{
		if (!FB.IsInitialized) {
			FB.Init (OnInitFBComplete, null, null);
		} else {
			FB.ActivateApp();
		}

	}

	void OnInitFBComplete (){
		ShoWLog(string.Format("OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",FB.IsLoggedIn,FB.IsInitialized));

		if(FB.IsLoggedIn){
			GetFBName();
		}
	}

	public void OnClickButtonFBLogin ()
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
		ShoWLog("FBFirstName: "+fbName);
	}

	void HandleLoginResult (IResult result)
	{
		if (result == null) {
			ShoWLog (result.ToString());
		} else if (!string.IsNullOrEmpty (result.Error)) {
			ShoWLog ("error login 01: " + result.ToString());
		} else if (result.Cancelled) {
			ShoWLog ("error login 02: " + result.ToString());
		} else if (!string.IsNullOrEmpty (result.RawResult)) { //success
			ShoWLog("login success");
			GetFBName();
		}
	}

	void ShoWLog (string log){
		logText.text = log;
	}
}

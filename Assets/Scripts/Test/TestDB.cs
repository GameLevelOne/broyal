using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class TestDB : MonoBehaviour {

	public Text resultBox;
	void Start () {
		
	}
	public void RegisterPressed () {
		resultBox.color = Color.white;
		resultBox.text = "Loading...";
		DBManager.API.UserRegistration (true,2,"helgawij0","hw","9909944483","tkrdaisuki@gmail.com",
			(response)=> {
				resultBox.text = response;
				resultBox.color = Color.white;
			}, 
			(error)=> {
				resultBox.text = error;
				resultBox.color = Color.red;
			});
	}	
	public void VerifyPressed () {
		resultBox.color = Color.white;
		resultBox.text = "Loading...";
		DBManager.API.VerifyUser ("motmot1","5292",
			(response)=> {
				resultBox.text = response;
			}, 
			(error)=> {
				resultBox.text = error;
				resultBox.color = Color.red;
			});
	}
	public void LoginPressed () {
		resultBox.color = Color.white;
		resultBox.text = "Loading...";
		DBManager.API.UserLogin ("helgawij0","hw",
			(response)=> {
				JSONNode jdata = JSON.Parse(response);
				resultBox.text = "Result: \n";
				resultBox.text += "UserName: "+jdata["username"]+"\n";
				resultBox.text += "Roles: "+jdata["roles"][0]+"\n";
				resultBox.text += "TokenType: "+jdata["token_type"]+"\n";
				resultBox.text += "AccessToken: "+jdata["access_token"]+"\n";
				resultBox.text += "ExpiresIn: "+jdata["expires_in"]+"\n";
				resultBox.text += "RefreshToken: "+jdata["refresh_token"]+"\n";
			}, 
			(error)=> {
				resultBox.text = error;
				resultBox.color = Color.red;
			});
	}
	public void GetProfilePressed () {
		resultBox.color = Color.white;
		resultBox.text = "Loading...";
		DBManager.API.GetUserProfile(
			(response)=> {
				resultBox.text = response;
			}, 
			(error)=> {
				resultBox.text = error;
				resultBox.color = Color.red;
			});
	}
}

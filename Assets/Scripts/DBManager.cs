using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class DBManager : MonoBehaviour {

	static string results;

	public void Login(string userName, string password, System.Action<string> onComplete )
	{
		string url = "http://63.142.251.107/bidroyalapi/api/login";
		Debug.Log (url);

		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\"username\":\""+userName+"\",\"password\":\""+password+"\"}";
		Debug.Log (jsondata);
		Dictionary<string,string> header = new Dictionary<string, string> ();
		header.Add ("Content-Type", "application/json");
		PostRequest(url,encoder.GetBytes(jsondata),header,onComplete);
	}


	public WWW PostRequest(string url, byte[] data, Dictionary<string,string> postHeader, System.Action<string> onComplete) {
		WWW www = new WWW(url, data, postHeader);

		StartCoroutine(WaitForRequest(www, onComplete));
		return www;

	}

	IEnumerator WaitForRequest(WWW www, System.Action<string> onComplete) {
		yield return www;
		// check for errors
		if (www.error == null) {
			results = www.text;
			onComplete(www.text);
		} else {
			Debug.Log ("Error: "+www.error);
			onComplete("Error: "+www.error);
		}
	}

}

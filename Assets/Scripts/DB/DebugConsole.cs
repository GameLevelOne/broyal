using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour {

	public Text restUrlText;
	public Text responseText;

	public GameObject showLayer;
	public GameObject hideLayer;
	public Text counterLabel;

	List<string> restUrl;
	List<string> response;
	int curIndex;

	void Awake() {
		restUrl = new List<string> ();
		response = new List<string> ();
		curIndex = -1;
		DBManager.API.debugConsole = this;
		UpdateText ();
	}

	void OnEnable() {
		Hide ();
	}

	public int SetRequest(string msg) {
		restUrl.Add(msg);
		response.Add("");
		curIndex = restUrl.Count - 1;
		return curIndex;
		UpdateText ();
	}

	public void SetResult(string msg, int idx = -1) {
		if (idx > -1) {
			response [idx] = msg;
			UpdateText ();
		} else if (response.Count>0) {
			response [response.Count-1] = msg;
			UpdateText ();
		}
	}
	public void SetError(string msg, int idx = -1) {
		if (idx > -1) {
			response [idx] = msg;
			UpdateText ();
		} else if (response.Count>0)  {
			response [response.Count-1] = msg;
			UpdateText ();
		}
	}

	void UpdateText() {
		if (restUrl.Count > 0) {
			restUrlText.text = restUrl [curIndex];
			if (response [curIndex] != "") {
				if (response [curIndex].Substring (0, 7) != "ERROR: ") {
					responseText.color = Color.white;
				} else {
					responseText.color = Color.red;
				}
			}
			responseText.text = response [curIndex];
			counterLabel.text = "" + (curIndex + 1) + "/" + restUrl.Count;
		} else {
			restUrlText.text = "REST URL";
			responseText.text = "RESPONSE";
			counterLabel.text = "0/0";
		}
	}

	public void Show() {
		showLayer.SetActive (PlayerPrefs.GetInt ("ShowDebugConsole",0)==1);
		hideLayer.SetActive (false);
	}
	public void Hide() {
		showLayer.SetActive (false);
		hideLayer.SetActive (PlayerPrefs.GetInt ("ShowDebugConsole",0)==1);
	}
	public void Prev() {
		if (curIndex > 0) {
			curIndex--;
			UpdateText ();
		}
		
	}
	public void Next() {
		if (curIndex < restUrl.Count-1) {
			curIndex++;
			UpdateText ();
		}

	}
}

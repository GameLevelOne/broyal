using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDB : MonoBehaviour {

	public DBManager dbManager;
	public Text resultBox;
	void Start () {
		
	}
	
	public void LoginPressed () {
		resultBox.text = "Loading...";
		dbManager.Login ("motmot1","mot",(response)=> {
			resultBox.text = response;
		});
	}
}

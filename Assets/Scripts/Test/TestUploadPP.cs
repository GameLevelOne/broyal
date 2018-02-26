using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUploadPP : MonoBehaviour {

    public Texture2D sourceTex;
    public Text statusText;


	void Start () {
        statusText.text = "AUTHENTICATING";
        
        DBManager.API.UserLogin("testing3", "password",
            (response) =>
            {
                statusText.text = "READY";
            },
            (error) =>
            {
                statusText.text = "AUTHENTICATE ERROR";
            });
	}
	
	public void UploadClicked () {
        statusText.text = "UPLOADING";
        byte[] databytes = sourceTex.GetRawTextureData();
        string s = "";
        for (int i=0;i<databytes.Length;i++) {
            s += databytes[i].ToString();
        }
        Debug.Log(s);
		DBManager.API.UpdateProfilePicture(databytes,sourceTex.GetHashCode().ToString()+".png",
            (response) =>
            {
                statusText.text = "UPLOAD SUCCESS";
            },
            (error) =>
            {
                statusText.text = "UPLOAD ERROR";
            }
        );
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;

public class SignInManager : MonoBehaviour {

	public void OnClickFBLogin (){
		FBManager.Instance.OnFBLogin();
	}

}

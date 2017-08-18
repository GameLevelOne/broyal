using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "DBSettings", menuName = "DBManager/Settings", order = 1)]
public class DBManagerSettings : ScriptableObject {
	[Header("DEBUG")]
	public bool debugMode;
	public bool comprehensiveDebug;
	public string messagePrefix;
	[Space(10)]

	[Header("MAIN SETTINGS")]
	public string restURL;
	public string royalBidKey;
	public string royalBidSecret;
	[Space(10)]

	[Header("API STRING")]
	public string userRegistrationAPI;
	public string verifyUserAPI;
	public string userLoginAPI;
	public string getUserStarsAPI;
	public string unsubscribeUserAPI;
	public string getUserProfileAPI;
	public string updateUserNameAPI;
	public string updateUserProfileAPI;
	public string userForgotPasswordAPI;
	public string userResetPasswordAPI;
	public string userChangePasswordAPI;

}

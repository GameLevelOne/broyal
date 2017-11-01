using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using SimpleJSON;

public class DBManager : MonoBehaviour {
//===========================Singleton Coding=======================================================
	static DBManager _instance = null;
	void Awake() {
		if (_instance == null) {
			_instance = this;
		} else if (_instance!=this) {
			Destroy (gameObject);			
		}
		DontDestroyOnLoad (gameObject);
	}
	public static DBManager API {
		get {
			return _instance;
		}
	}
//==================================================================================================


	public DBManagerSettings config;
	public string username = null;
	string tokenType = null;
	string accessToken = null;

//===========================Game=======================================================
	public void GetEligibleToEnterGame(int auctionId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getEligibleToEnterGame + auctionId;
		DebugMsg ("GET ELIGIBLE TO ENTER GAME Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void GetBidRumbleGame(int auctionId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getBidRumbleGame + auctionId;
		DebugMsg ("GET BID RUMBLE GAME Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void SubmitBidRumbleResult(int auctionId, int roundNumber, float score,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.submitBidRumbleResult;
		UTF8Encoding encoder = new UTF8Encoding ();
		long longScore = (long)(score * 1000000000);
		string jsondata = "{\n"+
			"\"auctionId\":"+auctionId+",\n"+
			"\"roundNumber\":"+roundNumber+",\n"+
			"\"score\":"+longScore+"\n"+
			"}";

		DebugMsg ("SUBMIT BID RUMBLE RESULT Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithAuthorization()),onComplete, onError);
	}

	public void SubmitBidRoyaleResult(int auctionId, int roundNumber, int answer,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.submitBidRoyaleResult;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"auctionId\":"+auctionId+",\n"+
			"\"roundNumber\":"+roundNumber+",\n"+
			"\"chest\":"+answer+"\n"+
			"}";

		DebugMsg ("SUBMIT BID ROYALE RESULT Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithAuthorization()),onComplete, onError);
	}

	public void GetPassingUserResult(int auctionId, int roundNumber,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getPassingUserResult + auctionId + "&roundNumber=" + roundNumber;
		DebugMsg ("GET PASSING USER RESULT Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

//===========================Pet API=======================================================
	public void PetListing( 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.petListing;
		DebugMsg ("PET LISTING Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void GetUserPetProfile( 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getUserPetProfile;
		DebugMsg ("GET USER PET PROFILE Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void ClaimPetTrainExp( 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.claimPetTrainExp;
		DebugMsg ("CLAIM PET TRAIN EXP Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void ClaimPetShareExp( 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.claimPetShareExp;
		DebugMsg ("CLAIM PET SHARE EXP Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void PurchasePet(int petId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.purchasePet + petId;
		DebugMsg ("PURCHASE PET Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void ChangePetName(int petId, string newName,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.changePetName;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"petId\":\""+petId+"\",\n"+
			"\"petName\":\""+newName+"\"\n"+
			"}";

		DebugMsg ("CHANGE PET NAME Request","\nurl = "+url);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithAuthorization()),onComplete, onError);
	}

	public void StartTrainingTime(
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.startTrainingTime;
		DebugMsg ("START TRAINING TIME Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void CheckTrainingTime( 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.checkTrainingTime;
		DebugMsg ("CHECK TRAINING TIME Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

//===========================Shop=======================================================
	public void GetShopList( 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getShopList;
		DebugMsg ("GET SHOP LIST Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

//===========================User API=======================================================

	public void UserRegistration(bool termsCondition, int gender, string userName, string password, string mobile, string email,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userRegistrationAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"termsCondition\":"+termsCondition+",\n"+
			"\"gender\":"+gender+",\n"+
			"\"username\":\""+userName+"\",\n"+
			"\"password\":\""+password+"\",\n"+
			"\"mobile\":\""+mobile+"\",\n"+
			"\"email\":\""+email+"\"\n"+
			"}";

		DebugMsg ("USER REGISTRATION Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithKey(),onComplete, onError);
	}

	public void VerifyUser(string userName, string verificationOtp, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.verifyUserAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"username\":\""+userName+"\",\n"+
			"\"verificationOtp\":\""+verificationOtp+"\"\n"+
			"}";

		DebugMsg ("VERIFY USER Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithKey(),onComplete, onError);
	}

	public void GetResendOTP(string userName, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getResendOTPAPI + userName;
		UTF8Encoding encoder = new UTF8Encoding ();

		DebugMsg ("GET RESEND OTP","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithKey(),onComplete, onError);
	}

	public void UserLogin(string userName, string password, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userLoginAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"username\":\""+userName+"\",\n"+
			"\"password\":\""+password+"\"\n"+
			"}";

		DebugMsg ("USER LOGIN Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeader(),
			(response)=> {
				JSONNode jdata = JSON.Parse(response);
				tokenType = jdata["token_type"];
				accessToken = jdata["access_token"];
				username = userName;
				PlayerPrefs.SetString("LastUserLogin",userName);
				PlayerPrefs.SetString("LastUserPassword",password);
				if (onComplete!=null)
					onComplete(response);
			}, onError);
	}

	public void CreateTopUp(int stars, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.createTopUp;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"stars\":"+stars+"\n"+
			"}";

		DebugMsg ("CREATE TOP UP Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void GetUserStars(System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getUserStarsAPI;
		DebugMsg ("GET USER STARS Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}
		
	public void GetUserProfile(System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getUserProfileAPI;
		DebugMsg ("GET USER PROFILE Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void UnsubscribeUser(System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.unsubscribeUserAPI;
		DebugMsg ("UNSUBSCRIBE USER Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void UpdateUserName(string userName,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.updateUserNameAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"username\":\""+userName+"\"\n"+
			"}";

		DebugMsg ("UPDATE USER NAME Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithAuthorization()),onComplete, onError);
	}

	public void UpdateUserProfile(int gender, string phoneNumber, string email, string completeAddress, string province, string city,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.updateUserProfileAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"gender\":"+gender+",\n"+
			"\"phoneNumber\":\""+phoneNumber+"\",\n"+
			"\"email\":\""+email+"\",\n"+
			"\"completeAddress\":\""+completeAddress+"\",\n"+
			"\"province\":\""+province+"\",\n"+
			"\"city\":\""+city+"\"\n"+
			"}";

		DebugMsg ("UPDATE USER PROFILE Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithAuthorization()),onComplete, onError);
	}

	public void UserForgotPassword(string userName,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userForgotPasswordAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"username\":\""+userName+"\"\n"+
			"}";

		DebugMsg ("USER FORGOT PASSWORD Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithKey()),onComplete, onError);
	}

	public void UserResetPassword(string userName, string verificationOtp, string password, string confirmPassword,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userResetPasswordAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"username\":\""+userName+"\",\n"+
			"\"verificationOtp\":\""+verificationOtp+"\",\n"+
			"\"password\":\""+password+"\",\n"+
			"\"confirmPassword\":\""+confirmPassword+"\"\n"+
			"}";

		DebugMsg ("USER RESET PASSWORD Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithKey()),onComplete, onError);
	}

	public void UserChangePassword(string oldPassword, string password, string confirmPassword,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userChangePasswordAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"oldPassword\":\""+oldPassword+"\",\n"+
			"\"password\":\""+password+"\",\n"+
			"\"confirmPassword\":\""+confirmPassword+"\"\n"+
			"}";

		DebugMsg ("USER CHANGE PASSWORD Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithAuthorization()),onComplete, onError);
	}
		
//===========================Auction API=======================================================
	public void AuctionBidding(int auctionId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.auctionBidding + auctionId;
		DebugMsg ("AUCTION BIDDING Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void AuctionJoin(int auctionId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.auctionJoin + auctionId;
		DebugMsg ("AUCTION JOIN Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void GetAuctionDetails(int auctionId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getAuctionDetails + auctionId;
		DebugMsg ("GET AUCTION DETAILS Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void GetWinnerList(
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getWinnerList;
		DebugMsg ("GET WINNER LIST Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void GetAuctionListing(int auctionType, int start, int end,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getAuctionListing + auctionType + "&start=" + start + "&end=" + end;
		DebugMsg ("GET AUCTION LISTING Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}
				
	public void GetLandingAuctionData(
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getLandingAuctionData;
		DebugMsg ("GET LANDING AUCTION DATA Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}


//===========================Utilities==============================================================

	WWW PostRequest(string url, byte[] data, Dictionary<string,string> postHeader, System.Action<string> onComplete, System.Action<string> onError) {
		if (postHeader == null) {
			if (onError!=null)
				onError("{\"errors\":\"NOT AUTHORIZED\"}");
			return null;
		} else {
			WWW www = new WWW (url, data, postHeader);
			StartCoroutine (WaitForRequest (www, onComplete, onError));
			return www;
		}
	}

	IEnumerator WaitForRequest(WWW www, System.Action<string> onComplete, System.Action<string> onError) {
		yield return www;
		if (www.error == null) {
			DebugMsg ("","RESULT: \n"+www.text);
//			Debug.Log ("ResponseHeaders: "+www.responseHeaders["content-type"]);
			if (onComplete!=null)
				onComplete(www.text);
		} else {
			DebugError ("ERROR: "+www.error, www.text);
			if (onError != null) {
				onError (www.error+"|"+www.text);
			}
		}
	}

	void DebugMsg(string msg = "", string cmsg = "")
	{
		if (config.debugMode) {
			if (msg!="")
				Debug.Log (config.messagePrefix+msg);
			if ((config.comprehensiveDebug) && (cmsg!=""))
				Debug.Log (config.messagePrefix+cmsg);
		}
	}

	void DebugError(string msg = "", string cmsg = "")
	{
		if (msg!="")
			Debug.LogError (config.messagePrefix+msg);
		if ((config.debugMode) && (config.comprehensiveDebug) && (cmsg!="")) 
			Debug.LogError (config.messagePrefix+cmsg);
	}

	Dictionary<string,string> CreateHeader() {
		Dictionary<string,string> header = new Dictionary<string, string> ();
		header.Add ("Content-Type", "application/json");
		return header;
	}
	Dictionary<string,string> CreateHeaderWithKey() {
		Dictionary<string,string> header = CreateHeader();
		header.Add ("royalBidKey", config.royalBidKey);
		header.Add ("royalBidSecret", config.royalBidSecret);
		return header;
	}
	Dictionary<string,string> PutRequestHeader(Dictionary<string,string> header) {
		header.Add ("X-HTTP-Method-Override", "PUT");
		return header;
	}
	Dictionary<string,string> CreateHeaderWithAuthorization() {
		Dictionary<string,string> header = CreateHeaderWithKey();
		if ((accessToken == null) || (accessToken == "")) {
			DebugError ("NOT AUTHORIZED!");
			return null;
		} else {
			header.Add ("Authorization", tokenType +" "+ accessToken);
			return header;
		}
	}

//==================================================================================================

}

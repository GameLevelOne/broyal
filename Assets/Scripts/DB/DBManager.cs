using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using SimpleJSON;
using System.IO;

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
	public DebugConsole debugConsole;
	public float timeOutThreshold;
	string tokenType = null;
	string accessToken = null;
	NotificationPopUp notifPopUp = null;

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

    public void GetVideoAds(
        System.Action<string> onComplete, System.Action<string> onError = null)
    {
        string url = config.restURL + config.getVideoAds;
        DebugMsg("GET VIDEO ADS Request", "\nurl = " + url);
        PostRequest(url, null, CreateHeaderWithAuthorization(), onComplete, onError);
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

	public void EquipPet(int petId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.equipPet;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"petId\":"+petId+"\n"+
			"}";
		DebugMsg ("EQUIP PET Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void ChangePetName(string newName,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.changePetName;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"petName\":\""+newName+"\"\n"+
			"}";

		DebugMsg ("CHANGE PET NAME Request","\nurl = "+url);
		PostRequest(url,encoder.GetBytes(jsondata),PutRequestHeader(CreateHeaderWithAuthorization()),onComplete, onError);
	}

	public void StartTrainingTime(
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.startTrainingTime;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{}";
		
		DebugMsg ("START TRAINING TIME Request","\nurl = "+url);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithAuthorization(),onComplete, onError);
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
			"\"email\":\""+email+"\",\n"+
			"\"isFbLogin\":false\n"+
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

	public void UserSocialLogin(string firstName,string lastName, int gender, string token, string userId, string email,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userSocialLoginAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"firstName\":\""+firstName+"\",\n"+
			"\"lastName\":\""+lastName+"\",\n"+
			"\"gender\":"+gender+",\n"+
			"\"token\":\""+token+"\",\n"+
			"\"userId\":\""+userId+"\",\n"+
			"\"email\":\""+email+"\""+
			"}";

		DebugMsg ("USER SOCIAL LOGIN Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithKey(),(response)=> {
			JSONNode jdata = JSON.Parse(response);
			tokenType = "Bearer";
			accessToken = jdata["token"];
			username = firstName+lastName;
			PlayerPrefs.SetString("LastUserLogin","FB_"+username);
			PlayerPrefs.SetString("LastUserPassword","FB_"+username);
			if (onComplete!=null)
				onComplete(response);
		}, onError);
	}
	public void UserLogout(
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.userLogoutAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{}";

		DebugMsg ("USER LOGOUT Request","\nurl = "+url);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithAuthorization(),onComplete, onError);
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

	public void UpdateUserProfile(int gender, string phoneNumber, string email, string completeName, string province, string provinceId, string city, string cityId, string streetAddress,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.updateUserProfileAPI;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"gender\":"+gender+",\n"+
//			"\"email\":\""+email+"\",\n"+
			"\"phoneNumber\":\""+phoneNumber+"\",\n"+
			"\"streetAddress\":\""+streetAddress+"\",\n"+
			"\"completeName\":\""+completeName+"\",\n"+
			"\"province\":\""+province+"\",\n"+
			"\"city\":\""+city+"\",\n"+
			"\"cityId\":\""+cityId+"\",\n"+
			"\"provinceId\":\""+provinceId+"\"\n"+
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


	public void UpdateProfilePicture(byte[] texData,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.updateProfilePicture;
        WWWForm data = new WWWForm();
		data.AddBinaryData("profilePictureImage",texData);
		Dictionary<string,string> header = CreateHeaderWithAuthorization ();
		header["Content-Type"]= "multipart/form-data";
       		
		DebugMsg ("UPLOAD PROFILE PICTURE REQUEST","\nurl = "+url+"\ndata = "+data.ToString());
		PostRequest(url,data.data,header,onComplete, onError);
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

	public void GetWinnerList(int auctionType,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getWinnerList+auctionType;
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
//		accessToken = "";
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

//===========================Claim==============================================================

	public void GetClaimAuction(int auctionId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getClaimAuction + auctionId;
		DebugMsg ("GET CLAIM AUCITON Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void GetPaymentDetails(int auctionId, 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getPaymentDetails + auctionId;
		DebugMsg ("GET PAYMENT DETAILS Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void SubmitClaimedOtp(int auctionId, string verificationOtp,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.submitClaimedOtp;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"auctionId\":"+auctionId+",\n"+
			"\"verificationOtp\":\""+verificationOtp+"\"\n"+
			"}";

		DebugMsg ("SUBMIT CLAIMED OTP Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithAuthorization(),onComplete, onError);
	}
	public void GeneratePreOrder(int purchaseType, int itemId, string paymentChannel,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.generatePreOrder;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n" + "\"purchaseType\":" + purchaseType + ",\n";
		jsondata += (purchaseType == 1 ? ("\"starShopId\":" + itemId + ",\n") : ("\"auctionId\":" + itemId + ",\n"));
		jsondata += "\"paymentChannel\":\""+paymentChannel+"\"\n"+"}";

		DebugMsg ("GENERATE PRE ORDER Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithAuthorization(),onComplete, onError);
	}

//===========================RajaOngkir=============================================================
	public void GetProvinceList( 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.rajaOngkirRestURL + config.getProvinceList;
		DebugMsg ("GET PROVINCE LIST Request","\nurl = "+url);
		PostRequest(url,null,CreateRajaOngkirHeader(),onComplete, onError);
	}

	public void GetCityList(string provinceId,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.rajaOngkirRestURL + config.getCityList + provinceId;
		DebugMsg ("GET CITY LIST Request","\nurl = "+url);
		PostRequest(url,null,CreateRajaOngkirHeader(),onComplete, onError);
	}


//===========================News=======================================================
	public void GetNewsList( 
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.getNewsList;
		DebugMsg ("GET NEWS LIST Request","\nurl = "+url);
		PostRequest(url,null,CreateHeaderWithAuthorization(),onComplete, onError);
	}

	public void ReadNews(int newsId,
		System.Action<string> onComplete , System.Action<string> onError = null)
	{
		string url = config.restURL + config.readNews;
		UTF8Encoding encoder = new UTF8Encoding ();
		string jsondata = "{\n"+
			"\"newsId\":"+newsId+"\n"+
			"}";

		DebugMsg ("READ NEWS Request","\nurl = "+url+"\ndata = "+jsondata);
		PostRequest(url,encoder.GetBytes(jsondata),CreateHeaderWithAuthorization(),onComplete, onError);
	}




//===========================Utilities==============================================================

	WWW PostRequest(string url, byte[] data, Dictionary<string,string> postHeader, System.Action<string> onComplete, System.Action<string> onError) {
		if (postHeader == null) {
			ShowUnauthorizedError ();
			return null;
		} else {
			int debugIndex = -1;
			if (debugConsole != null) {
				string serverUrl = url.Substring (0, config.restURL.Length);
				string apiUrl = url.Substring (config.restURL.Length);
				debugIndex = debugConsole.SetRequest ("REST URL: " + serverUrl + "\n" +  apiUrl);
			}
			WWW www;
			www = new WWW (url, data, postHeader); 

			StartCoroutine (WaitForRequest (www, onComplete, onError,debugIndex));
			return www;
		}
	}
    WWW PostRequestForm(string url, WWWForm data, System.Action<string> onComplete, System.Action<string> onError)
    {
        if (data.headers.Count==0)
        {
            ShowUnauthorizedError();
            return null;
        }
        else
        {
            int debugIndex = -1;
            if (debugConsole != null)
            {
                string serverUrl = url.Substring(0, config.restURL.Length);
                string apiUrl = url.Substring(config.restURL.Length);
                debugIndex = debugConsole.SetRequest("REST URL: " + serverUrl + "\n" + apiUrl);
            }
            WWW www;
            www = new WWW(url,data);

            StartCoroutine(WaitForRequest(www, onComplete, onError, debugIndex));
            return www;
        }
    }
	IEnumerator WaitForRequest(WWW www, System.Action<string> onComplete, System.Action<string> onError,int debugIndex) {
		float timeOutCheck = timeOutThreshold;
		while ((timeOutCheck > 0f) && (!www.isDone)) {
			timeOutCheck -= Time.deltaTime;
			yield return null;
		}
		if (timeOutCheck > 0f) {
			if (www.error == null) {
				DebugMsg ("", "RESULT: \n" + www.text);
				if (debugConsole != null)
					debugConsole.SetResult (www.text, debugIndex);
				if (onComplete != null)
					onComplete (www.text);
			} else {
				DebugError ("ERROR: " + www.error, www.text);
				if (debugConsole != null)
					debugConsole.SetError ("ERROR: " + www.error + "\n" + www.text, debugIndex);

				if (www.error.Trim () != "401 Unauthorized") {
					if (onError != null)
						onError (www.error + "|" + www.text);
                } else if (www.error.Contains("ConnectException"))
                {
                    //DebugError ("ERROR: " + www.error, www.text);
                    if (debugConsole != null)
                        debugConsole.SetError("ERROR: REQUEST_TIME_OUT", debugIndex);
                    if (onError != null)
                        onError("{\"errors\":\"REQUEST_TIME_OUT\"}");
                }
                else
                {
                    ShowUnauthorizedError();
                }
			}
		} else {
            //DebugError ("ERROR: " + www.error, www.text);
			if (debugConsole != null)
				debugConsole.SetError ("ERROR: REQUEST_TIME_OUT", debugIndex);
			if (onError != null)
				onError ("{\"errors\":\"REQUEST_TIME_OUT\"}");
		}
	}

	void DebugMsg(string msg = "", string cmsg = "")
	{
		if (config.debugMode) {
			if (msg != "") {
				Debug.Log (config.messagePrefix + msg);
			}
			if ((config.comprehensiveDebug) && (cmsg!=""))
				Debug.Log (config.messagePrefix+cmsg);
		}
	}

	void DebugError(string msg = "", string cmsg = "")
	{
		if (msg!="")
			Debug.LogWarning (config.messagePrefix+msg);
		if ((config.debugMode) && (config.comprehensiveDebug) && (cmsg!="")) 
			Debug.LogWarning (config.messagePrefix+cmsg);
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
			DebugError ("UNAUTHORIZED!");
			return null;
		} else {
			header.Add ("Authorization", tokenType +" "+ accessToken);
			return header;
		}
	}
	Dictionary<string,string> CreateHeaderNoJSON() {
		Dictionary<string,string> header = CreateHeaderWithAuthorization();
		header.Remove ("Content-Type");
		return header;
	}
	Dictionary<string,string> CreateRajaOngkirHeader() {
		Dictionary<string,string> header = new Dictionary<string, string> ();
		header.Add ("key", config.rajaOngkirKey);
		return header;
	}

	void ShowUnauthorizedError() {
		GameObject g = GameObject.FindWithTag ("NotifPopUp");
		if (g!=null) {
			notifPopUp = g.transform.GetChild(g.transform.childCount-1).GetComponent<NotificationPopUp>();
			notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error.UNAUTHORIZED"));
			notifPopUp.OnFinishOutro += RestartApp;
		}
		GameObject c = GameObject.FindWithTag ("ConnectingPanel");
		if (c != null) {
			c.GetComponent<ConnectingPanel> ().Connecting (false);
		}
	}

	public void RestartApp() {
		if (notifPopUp!=null)
			notifPopUp.OnFinishOutro -= RestartApp;
		PlayerPrefs.DeleteKey ("LastUserLogin");
		Application.LoadLevel ("SceneSignIn");
	}

//==================================================================================================

}

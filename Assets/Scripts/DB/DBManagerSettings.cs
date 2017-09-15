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
	public string getBidRumbleRoundDetails;
	public string submitBidRumbleResult;
	public string petListing;
	public string getUserPetProfile;
	public string claimPetTrainExp;
	public string claimPetShareExp;
	public string purchasePet;
	public string changePetName;
	public string startTrainingTime;
	public string checkTrainingTime;
	public string userRegistrationAPI;
	public string verifyUserAPI;
	public string userLoginAPI;
	public string createTopUp;
	public string getUserStarsAPI;
	public string getUserProfileAPI;
	public string unsubscribeUserAPI;
	public string getAuctionLandingData;
	public string getAuctionListingCurrent;
	public string getAuctionListingPast;
	public string getAuctionListingFuture;
	public string updateUserNameAPI;
	public string updateUserProfileAPI;
	public string userForgotPasswordAPI;
	public string userResetPasswordAPI;
	public string userChangePasswordAPI;
	public string auctionBidding;
	public string auctionJoin;
	public string getAuctionDetails;



}

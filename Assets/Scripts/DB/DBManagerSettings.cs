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
	public string rajaOngkirKey;
	public string rajaOngkirRestURL;
	[Space(10)]

	[Header("API STRING")]
	public string userRegistrationAPI;
	public string verifyUserAPI;
	public string getResendOTPAPI;
	public string userLoginAPI;
	public string userForgotPasswordAPI;
	public string userResetPasswordAPI;
	public string userChangePasswordAPI;
	[Space(10)]
	public string getLandingAuctionData;
	public string getAuctionListing;
	public string auctionJoin;
	public string getAuctionDetails;
	public string auctionBidding;
	public string getEligibleToEnterGame;
	[Space(10)]
	public string getClaimAuction;
	public string getPaymentDetails;
	public string submitClaimedOtp;
	[Space(10)]
	public string getBidRumbleGame;
	public string submitBidRumbleResult;
	public string submitBidRoyaleResult;
	public string getPassingUserResult;
	[Space(10)]
	public string getUserStarsAPI;
	public string createTopUp;
	public string unsubscribeUserAPI;
	public string updateUserNameAPI;
	public string updateUserProfileAPI;
	public string getUserProfileAPI;
	[Space(10)]
	public string getUserPetProfile;
	public string claimPetTrainExp;
	public string claimPetShareExp;
	public string changePetName;
	public string startTrainingTime;
	public string checkTrainingTime;
	public string equipPet;
	[Space(10)]
	public string getWinnerList;
	public string petListing;
	public string purchasePet;
	public string getShopList;
	[Space(10)]
	public string getProvinceList;
	public string getCityList;



}

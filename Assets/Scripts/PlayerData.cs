using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerData : MonoBehaviour {
	private static PlayerData instance;
	private string username;
	private string email;
	private string gender;
	private string phone;
	private int stars;
	private string profilePicUrl; //temp

	private string currentPetId;
	private string currentPetName;
	private int currentPetExp = 0;

//	private GameType gameType;
//	private TrainingType trainingType;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance=this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public static PlayerData Instance{ get { return instance; }}

//	public string Username {
//		set{ 
//			currentUsername = value; 
//			PlayerPrefs.SetString("PlayerData/Username",currentUsername);
//		}
//		get{ return currentUsername;}
//	}
//
//	public string Password{
//		set{ 
//			currentPassword = value;
//			PlayerPrefs.SetString("PlayerData/Password",currentPassword);
//		}
//		get{ return currentPassword;}
//	}
//
//	public string Email {
//		set{ currentEmail = value; }
//		get{ return currentEmail;}
//	}
//
//	public string Gender {
//		set{ currentGender = value; }
//		get{ return currentGender;}
//	}
//
//	public string PhoneNum {
//		set{ currentPhoneNum = value; }
//		get{ return currentPhoneNum;}
//	}
//
//	public int StarsSpent {
//		set{ currentStarsSpent = value; }
//		get{ return currentStarsSpent;}
//	}
//
//	public int AvailableStars {
//		set{ currentAvailableStars = value; }
//		get{ return currentAvailableStars;}
//	}
//
//	public string ProfilePic {
//		set{ currentProfilePic = value;}
//		get{ return currentProfilePic;}
//	}
//
//	public string PetId{
//		set{ currentPetId = value;}
//		get{ return currentPetId;}
//	}
//
//	public string PetName{
//		set{ currentPetName = value;}
//		get{ return currentPetName;}
//	}
//
//	public int PetExp {
//		set{ currentPetExp = value; }
//		get{ return currentPetExp;}
//	}
//
//	public GameType CurrentGameType{
//		set{ gameType = value;}
//		get{ return gameType;}
//	}
//
//	public TrainingType CurrentTrainingType{
//		set{ trainingType = value;}
//		get{ return trainingType;}
//	}
//
	void OnApplicationQuit(){
		PlayerPrefs.Save();
	}
}

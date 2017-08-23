﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
	private static PlayerData instance;
	private string currentUsername;
	private string currentEmail;
	private string currentGender;
	private string currentPhoneNum;
	private int currentStarsSpent;
	private int currentAvailableStars;
	private string currentProfilePic; //temp

	private string currentPetName = "Kochirou"; //temp
	private int currentPetExp = 0;

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

	public string Username {
		set{ currentUsername = value; }
		get{ return currentUsername;}
	}

	public string Email {
		set{ currentEmail = value; }
		get{ return currentEmail;}
	}

	public string Gender {
		set{ currentGender = value; }
		get{ return currentGender;}
	}

	public string PhoneNum {
		set{ currentPhoneNum = value; }
		get{ return currentPhoneNum;}
	}

	public int StarsSpent {
		set{ currentStarsSpent = value; }
		get{ return currentStarsSpent;}
	}

	public int AvailableStars {
		set{ currentAvailableStars = value; }
		get{ return currentAvailableStars;}
	}

	public string ProfilePic {
		set{ currentProfilePic = value;}
		get{ return currentProfilePic;}
	}

	public string PetName{
		set{ currentPetName = value;}
		get{ return currentPetName;}
	}

	public int PetExp {
		set{ currentPetExp = value; }
		get{ return currentPetExp;}
	}
}

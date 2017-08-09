using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilesManager : MonoBehaviour {
	public GameObject panelAuctionLobby;
	public GameObject panelUserProfile;
	public GameObject panelPetProfile;
	public GameObject panelLandingPage;
	public GameObject panelEditProfile;
	public GameObject panelEditUsername;
	public GameObject panelEditPassword;
	public GameObject panelEditPetName;

	public void OnClickUserProfile (){
		panelUserProfile.SetActive(true);
		panelPetProfile.SetActive(false);
	}

	public void OnClickPetProfile(){
		panelUserProfile.SetActive(false);
		panelPetProfile.SetActive(true);
	}

	public void OnClickBack(){
		panelLandingPage.SetActive(true);
		panelAuctionLobby.SetActive(false);
		this.gameObject.SetActive(false);
	}

	public void OnClickEditProfile(){
		panelEditProfile.SetActive(true);
	}

	public void OnClickCancelEditProfile(){
		panelEditProfile.SetActive(false);
	}

	public void OnClickEditUsername(){
		panelEditUsername.SetActive(true);
	}

	public void OnClickCancelEditUsername(){
		panelEditUsername.SetActive(false);
	}

	public void OnClickEditPassword(){
		panelEditPassword.SetActive(true);
	}

	public void OnClickCancelEditPassword(){
		panelEditPassword.SetActive(false);
	}

	public void OnClickEditPetName(){
		panelEditPetName.SetActive(true);
	}

	public void OnClickCancelEditPetName(){
		panelEditPetName.SetActive(false);
	}
}

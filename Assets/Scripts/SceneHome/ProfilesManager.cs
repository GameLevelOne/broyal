using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilesManager : MonoBehaviour {
	public GameObject panelUserProfile;
	public GameObject panelPetProfile;
	public GameObject panelLandingPage;

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
		this.gameObject.SetActive(false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderAreaManager : MonoBehaviour {
	public GameObject panelLandingPage;
	public GameObject panelParentProfile;
	public GameObject panelUserProfile;
	public GameObject panelPetProfile;
	public GameObject panelSettings;

	public void OnClickUserProfile(){
		panelParentProfile.SetActive(true);
		panelLandingPage.SetActive(false);
		panelUserProfile.SetActive(true);
		panelPetProfile.SetActive(false);
	}

	public void OnClickPetProfile(){
		panelParentProfile.SetActive(true);
		panelLandingPage.SetActive(false);
		panelPetProfile.SetActive(true);
		panelUserProfile.SetActive(false);
	}

	public void OnClickSettings(){
		panelLandingPage.SetActive(false);
		panelSettings.SetActive(true);
	}
}

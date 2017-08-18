using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeaderAreaManager : MonoBehaviour {
	public NavigationBarManager navigationBar;

	public GameObject panelLandingPage;
	public GameObject panelParentProfile;
	public GameObject panelUserProfile;
	public GameObject panelPetProfile;
	public GameObject panelSettings;

	public void OnClickUserProfile(){
		navigationBar.CloseCurrentActivePanel();
		panelParentProfile.SetActive(true);
		panelUserProfile.SetActive(true);
		panelPetProfile.SetActive(false);
	}

	public void OnClickPetProfile(){
		navigationBar.CloseCurrentActivePanel();
		panelParentProfile.SetActive(true);
		panelPetProfile.SetActive(true);
		panelUserProfile.SetActive(false);
	}

	public void OnClickSettings(){
		navigationBar.CloseCurrentActivePanel();
		panelSettings.SetActive(true);
	}
}

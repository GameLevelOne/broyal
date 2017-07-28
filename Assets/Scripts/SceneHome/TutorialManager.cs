using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

	public GameObject panelCompleteFBProfile;

	public void OnClickSkipTutorial(){
		this.gameObject.SetActive(false);
		panelCompleteFBProfile.SetActive(true);
	}
}

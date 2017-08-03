using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaimConfirmationManager : MonoBehaviour {

	public GameObject panelCheckout;

	public void OnClickEnter (){
		panelCheckout.SetActive(true);
		this.gameObject.SetActive(false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : BasePage {
	public GameObject panelPetDescription;
	public GameObject panelPetNaming;

	public NumberCountUpEffect textStarAmount;
	public Transform iconHeaderStar;

	float elapsedTime = 0f;
	float time = 2f;

//	void OnEnable (){
//		NumberCountUpEffect.OnCountUpFinished += OnCountFinished;
//	}
//
//	void OnDisable(){
//		NumberCountUpEffect.OnCountUpFinished -= OnCountFinished;
//	}

	public void OnClickPetDesc(){
		panelPetDescription.SetActive(true);
	}

	public void OnClickBuyPet (){
		panelPetDescription.SetActive(false);
		panelPetNaming.SetActive(true);
	}

	public void OnClickCancel(){
		panelPetDescription.SetActive(false);
	}

	public void OnClickConfirmPetName(){
		panelPetNaming.SetActive(false);
	}

	public void OnClickBuyStars(int amount){ //temporary
		int currValue = PlayerData.Instance.AvailableStars;
		int targetValue = currValue + amount;

		StartCoroutine(ScaleUpDownStar(true));
		textStarAmount.DoAnimCountUp(currValue,targetValue);

		PlayerData.Instance.StarsSpent += amount;
		PlayerData.Instance.AvailableStars = targetValue;
	}

	IEnumerator ScaleUpDownStar (bool scaleUp)
	{
		Vector3 oriSize = new Vector3(1f,1f,1f);
		Vector3 targetSize = new Vector3(1.1f,1.1f,1f);
		elapsedTime=0;

		if (scaleUp) {
			while (elapsedTime < time) {
				Debug.Log("scale up");
				iconHeaderStar.localScale = Vector3.LerpUnclamped (oriSize, targetSize, (elapsedTime / time));
				elapsedTime+=Time.deltaTime;
				yield return null;
			}
		} else {
			while (elapsedTime < time) {
				Debug.Log("scale down");
				iconHeaderStar.localScale = Vector3.LerpUnclamped (targetSize, oriSize, (elapsedTime / time));
				elapsedTime+=Time.deltaTime;
				yield return null;
			}
		}
	}

	void OnCountFinished(){
		StartCoroutine(ScaleUpDownStar(false));
	}
}

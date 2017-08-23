using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberCountUpEffect : MonoBehaviour {
	Text textObj;

	public delegate void CountUpFinished();
	public static event CountUpFinished OnCountUpFinished;

	// Use this for initialization
	void Start () {
		textObj = GetComponent<Text>();
	}
	
	public void DoAnimCountUp(int currValue,int targetValue){
		StartCoroutine(numberCountUp(currValue,targetValue));
	}

	IEnumerator numberCountUp (int currValue,int targetValue)
	{
		for (int i = currValue; i <= targetValue; i++) {
			textObj.text = i.ToString();
			yield return null;
		}
		OnCountUpFinished();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesIntroOutro : MonoBehaviour {
	public delegate void IntroOutroEvent();
	public event IntroOutroEvent OnFinishIntro;
	public event IntroOutroEvent OnFinishOutro;

	Animator anim;

	void Awake() {
		anim = GetComponent<Animator> ();
	}

	public void Activate(bool show)
	{
		gameObject.SetActive (true);
		if (!show)
			anim.SetTrigger ("Outro");
	}
				
	void FinishIntroEvent()
	{
//		Debug.Log ("Reset Trigger: " + name);
		anim.ResetTrigger ("Intro");
		if (OnFinishIntro != null)
			OnFinishIntro();
	}

	void FinishOutroEvent()
	{
		if (OnFinishOutro != null)
			OnFinishOutro();
		gameObject.SetActive (false);
	}

	protected void OnEnable()
	{
		anim.SetTrigger ("Intro");
//		Debug.Log ("Intro: " + name);
	}

	protected void OnDisable()
	{
		anim.ResetTrigger ("Intro");
		anim.ResetTrigger ("Outro");
	}
}

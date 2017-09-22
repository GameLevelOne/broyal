using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesIntroOutro : MonoBehaviour {
	public delegate void IntroOutroEvent();
	public static event IntroOutroEvent OnFinishIntro;
	public static event IntroOutroEvent OnFinishOutro;

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
		if (OnFinishIntro != null)
			OnFinishIntro();
	}

	void FinishOutroEvent()
	{
		if (OnFinishOutro != null)
			OnFinishOutro();
		gameObject.SetActive (false);
	}

	void OnEnable()
	{
		anim.SetTrigger ("Intro");
	}

	void OnDisable()
	{
		anim.ResetTrigger ("Intro");
		anim.ResetTrigger ("Outro");
	}
}

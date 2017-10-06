using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePage : PagesIntroOutro {

	protected new void OnEnable()
	{
		Init ();
		base.OnEnable ();
	}

	protected virtual void Init()
	{
	}

	public void NextPage(string nextPage)
	{
		BasePage futurePage = PagesManager.instance.GetPagesByName(nextPage);
		if (futurePage!=null)
			PagesManager.instance.CurrentPageOutro (futurePage);
	}

	public bool IsIdle()
	{
		Animator anim = GetComponent<Animator>();
		bool idle = anim.GetCurrentAnimatorStateInfo (0).IsName ("Idle");
		return idle;
	}

//	public void BackPage()
//	{
//		PagesManager.instance.BackToPrevPage ();
//	}
//
}

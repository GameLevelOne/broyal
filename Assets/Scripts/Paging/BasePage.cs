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

//	public void BackPage()
//	{
//		PagesManager.instance.BackToPrevPage ();
//	}
//
}

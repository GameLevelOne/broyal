using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppInitPages : PagesIntroOutro {

	AppInitPages nextPage;

	protected new void OnEnable()
	{
		Init ();
		base.OnEnable ();
	}

	protected virtual void Init()
	{
	}

	public void CloseAndGoToNextPage(AppInitPages toNextPage)
	{
		nextPage = toNextPage;
		OnFinishOutro += OpenNextPage;
		Activate (false);
	}

	void OpenNextPage() {
		OnFinishOutro -= OpenNextPage;
		nextPage.Activate (true);
	}
}

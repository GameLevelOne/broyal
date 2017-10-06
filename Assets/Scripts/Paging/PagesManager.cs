using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesManager : MonoBehaviour {
	public List<string> smallPagesName;
	public List<BasePage> smallPagesList;
	public List<string> bigPagesName;
	public List<BasePage> bigPagesList;

//	public List<BasePage> pagesStack;
	public PagesIntroOutro headerNav;
	public NavigationBarManager navigationBar;
	BasePage nextPage;
	BasePage currentPage;

//===========================Singleton Coding=======================================================
	static PagesManager _instance = null;
	void Awake() {
		if (_instance == null) {
			_instance = this;
		} 
	}
	public static PagesManager instance {
		get {
			return _instance;
		}
	}
//==================================================================================================

	void Start()
	{
		currentPage = GetPagesByName ("LANDING");
	}

	public void NextPageIntro()
	{
//		pagesStack.Add (page);
		currentPage.OnFinishOutro -= NextPageIntro;
//		Debug.Log ("PageIntro: "+nextPage.name);
		nextPage.Activate (true);
		currentPage = nextPage;
		if ((IsSmallPage (nextPage)) && (!headerNav.gameObject.activeSelf))
			headerNav.Activate (true);
	}

	public void CurrentPageOutro(BasePage destinationPage)
	{
		if (currentPage.IsIdle ()) {
			nextPage = destinationPage;
//		Debug.Log ("PageOutro: "+currentPage.name);
			currentPage.Activate (false);
			currentPage.OnFinishOutro += NextPageIntro;
			navigationBar.CheckHighlightBox (nextPage);
			if ((IsSmallPage (currentPage)) && (!IsSmallPage (nextPage)))
				headerNav.Activate (false);
		}
	}
		
	public BasePage GetPagesByName(string nextPage)
	{
		int index = smallPagesName.IndexOf(nextPage);
		if (index >= 0) {
			return smallPagesList [index];
		} else {
			index = bigPagesName.IndexOf (nextPage);
			if (index >= 0) {
				return bigPagesList [index];
			} else {
				return null;
			}
		}
	}

	public bool IsSmallPage(BasePage page)
	{
		int index = smallPagesList.IndexOf (page);
		if (index >= 0)
			return true;
		else
			return false;
	}

//	public BasePage GetCurrentPage()
//	{
//		return pagesStack [pagesStack.Count - 1];
//	}
//
//	public void BackToPrevPage()
//	{
//		if (pagesStack.Count > 1) {
//			GetCurrentPage ().Activate (false);
//			int lastIndex = pagesStack.Count - 1;
//			pagesStack.RemoveAt (lastIndex);
//		}
//	}


}

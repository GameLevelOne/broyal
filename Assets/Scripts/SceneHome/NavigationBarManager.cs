using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationBarManager : MonoBehaviour {
	public List<BasePage> navigationPage;
	public GameObject[] highlightBox;
	public Button[] navigationButton;

	public void CheckHighlightBox(BasePage nextPage)
	{
		int index = navigationPage.IndexOf (nextPage);
		for (int i = 0; i < highlightBox.Length; i++) {
			highlightBox [i].SetActive (false);
			navigationButton [i].enabled = true;
		}
		if (index > -1) {
			highlightBox [index].SetActive (true);	
			navigationButton [index].enabled = false;
		}
	}

	public void GoToPage(string pageName)
	{
        SoundManager.Instance.PlaySFX(SFXList.Button01);
        BasePage nextPage = PagesManager.instance.GetPagesByName(pageName);
		PagesManager.instance.CurrentPageOutro (nextPage);
	}

}

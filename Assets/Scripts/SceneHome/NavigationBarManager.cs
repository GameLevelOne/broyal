using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MenuNames{Winner,News,Home,Training,Shop}

public class NavigationBarManager : MonoBehaviour {
	public Transform bgHighlight;
	public BasePage[] pages;
	public GameObject[] panels = new GameObject[5];

	float[] highlightXPos = new float[5];
	float highlightYPos = -466f;

	GameObject currentActivePanel;

	void Start(){
		highlightXPos[0]= -282f;
		highlightXPos[1]= -137.6f;
		highlightXPos[2]= 6.8f;
		highlightXPos[3]= 151.2f;
		highlightXPos[4]= 295.6f;

		currentActivePanel = panels[(int)MenuNames.Home];
	}

	public void OnClickWinner(){
		SetBGHighlight((int)MenuNames.Winner);
		SetActivePanel((int)MenuNames.Winner);
	}

	public void OnClickNews (){
		SetBGHighlight((int)MenuNames.News);
		SetActivePanel((int)MenuNames.News);
	}

	public void OnClickHome(){
		SetBGHighlight((int)MenuNames.Home);
		for (int i = 0; i < panels.Length; i++) {
			if (i == (int)MenuNames.Home) {
				PagesManager.instance.CurrentPageOutro (pages[0]);
				currentActivePanel = panels[i];
			} else {
				panels[i].SetActive(false);
			}
		}
	}

	public void OnClickTraining(){
		SetBGHighlight((int)MenuNames.Training);
		SetActivePanel((int)MenuNames.Training);
	}

	public void OnClickShop(){
		SetBGHighlight((int)MenuNames.Shop);
		SetActivePanel((int)MenuNames.Shop);
	}

	void SetActivePanel (int currentPanel)
	{
		for (int i = 0; i < panels.Length; i++) {
			if (i == currentPanel) {
				panels [i].SetActive (true);
				currentActivePanel = panels[i];
			} else {
				panels[i].SetActive(false);
			}
		}
	}

	void SetBGHighlight (int currentPanel){
		bgHighlight.localPosition = new Vector3(highlightXPos[currentPanel],highlightYPos,0);
	}

	public void CloseCurrentActivePanel(){
		Debug.Log(currentActivePanel);
		currentActivePanel.SetActive(false);
	}

	public void BackToHome(){
		currentActivePanel = panels[(int)MenuNames.Home];
		SetBGHighlight((int)MenuNames.Home);
	}

}

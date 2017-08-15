using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MenuNames{Winner,News,Home,Training,Shop,AuctionLobby}

public class NavigationBarManager : MonoBehaviour {
	public Transform bgHighlight;

	public GameObject[] panels = new GameObject[6];

	float[] highlightXPos = new float[5];
	float highlightYPos = -466f;

	void Start(){
		highlightXPos[0]= -282f;
		highlightXPos[1]= -137.6f;
		highlightXPos[2]= 6.8f;
		highlightXPos[3]= 151.2f;
		highlightXPos[4]= 295.6f;
	}

	public void OnClickWinner(){
		bgHighlight.localPosition=new Vector3(highlightXPos[(int)MenuNames.Winner],highlightYPos,0);
		SetActivePanel((int)MenuNames.Winner);
	}

	public void OnClickNews (){
		bgHighlight.localPosition=new Vector3(highlightXPos[(int)MenuNames.News],highlightYPos,0);
		SetActivePanel((int)MenuNames.News);
	}

	public void OnClickHome(){
		bgHighlight.localPosition=new Vector3(highlightXPos[(int)MenuNames.Home],highlightYPos,0);
		SetActivePanel((int)MenuNames.Home);;
	}

	public void OnClickTraining(){
		bgHighlight.localPosition=new Vector3(highlightXPos[(int)MenuNames.Training],highlightYPos,0);
		SetActivePanel((int)MenuNames.Training);
	}

	public void OnClickShop(){
		bgHighlight.localPosition=new Vector3(highlightXPos[(int)MenuNames.Shop],highlightYPos,0);
		SetActivePanel((int)MenuNames.Shop);
	}

	void SetActivePanel (int currentPanel)
	{
		for (int i = 0; i < panels.Length; i++) {
			if (i == currentPanel) {
				panels [i].SetActive (true);
			} else {
				panels[i].SetActive(false);
			}
		}
	}

}

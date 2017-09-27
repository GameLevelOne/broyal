using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPairingTile : MonoBehaviour {
	bool isBlue = false; //blue or red
	Color[] tileColors = new Color[2]{new Color(0.91f,0.29f,0.29f,1),new Color(0.07f,0.71f,0.83f,1)};

	public delegate void TileClicked();
	public static event TileClicked OnTileClicked;

	public void InitTile(bool isBlue){
		Image tileImg = GetComponent<Image>();
		this.isBlue=isBlue;
		if(isBlue){
			tileImg.color = tileColors[0];
		} else{
			tileImg.color = tileColors[1];
		}
	}

	public void ChangeColor(){
		Image tileImg = GetComponent<Image>();
		if(tileImg.color == tileColors[0]){
			tileImg.color=tileColors[1];
		} else{
			tileImg.color=tileColors[0];
		}
		OnTileClicked();
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPairingTile : MonoBehaviour {
	bool isBlue = false; //blue or red

	public delegate void TileClicked();
	public static event TileClicked OnTileClicked;

	public void InitTile(bool isBlue){
		Image tileImg = GetComponent<Image>();
		tileImg.raycastTarget=true;
		this.isBlue=isBlue;
		if(isBlue){
			tileImg.color = Color.blue;
		} else{
			tileImg.color = Color.red;
		}
	}

	public void ChangeColor(){
		Image tileImg = GetComponent<Image>();
		if(tileImg.color == Color.blue){
			tileImg.color=Color.red;
		} else{
			tileImg.color=Color.blue;
		}
		OnTileClicked();
	}
}

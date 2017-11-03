using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameTile : MonoBehaviour {
	[SerializeField]
	int picValue;
	[SerializeField]
	int tileIdx;

	public delegate void MemoryTileClicked(int picValue,int tileIdx);
	public static event MemoryTileClicked OnMemoryTileClicked;

	Sprite tilePic;
	Sprite closedTile;

	public void InitTile(int value,int idx,Sprite pic){
		closedTile = GetComponent<Image> ().sprite;
		picValue = value;
		tileIdx = idx;
		tilePic = pic;
	}

	public void OnClick(){
		GetComponent<Image>().sprite = tilePic;
		OnMemoryTileClicked(picValue,tileIdx);
	}

	public void Reset() {
		GetComponent<Image> ().sprite = closedTile;
	}
}

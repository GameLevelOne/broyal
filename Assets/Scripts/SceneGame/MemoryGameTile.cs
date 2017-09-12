using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameTile : MonoBehaviour {
	public int picValue;

	public delegate void MemoryTileClicked(int picValue);
	public static event MemoryTileClicked OnMemoryTileClicked;

	public void InitTile(int value,Sprite pic){
		picValue = value;
		GetComponent<Image>().sprite = pic;
	}

	public void OnClick(){
		OnMemoryTileClicked(picValue);
	}
}

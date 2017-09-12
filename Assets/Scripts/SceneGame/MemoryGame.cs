using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGame : MonoBehaviour {
	public Transform tileParent;
	public GameObject tilePrefab;
	public GameObject overlay;
	public Text countdownText;
	public Sprite[] tilePictures;

	GameObject[] tileObjects = new GameObject[6];
	int tileCounter=0;
	int picCounter=0; 
	int pairCounter=0; 
	List<int> pictureValues = new List<int>();
	List<int> slots = new List<int>();

	void Start(){
		GenerateTiles();
	}

	void OnEnable(){
		MemoryGameTile.OnMemoryTileClicked += HandleOnMemoryTileClicked;
	}

	void HandleOnMemoryTileClicked (int picValue)
	{
		Debug.Log("value:"+picValue);
	}

	void GenerateTiles(){
		for(int i=0;i<3;i++){
			for(int j=0;j<2;j++){
				GameObject tileObj = Instantiate(tilePrefab,tileParent,false);
				tileObj.transform.localPosition = new Vector3((-150f+j*300f),(300-i*300f),0);
				tileObjects[tileCounter]=tileObj;
				tileCounter++;
			}
		}
		RandomPic();
	}

	void RandomPic ()
	{
		for (int i = 0; i < 6; i++) {
			slots.Add (i);
		}
		for (int i = 0; i < 6; i++) {
			int temp = Random.Range (0, slots.Count);
			if (pairCounter < 2) {
				pairCounter++;
			} else {
				pairCounter = 1;
				picCounter++;
			}
			tileObjects [slots[temp]].GetComponent<MemoryGameTile>().InitTile(picCounter,tilePictures[picCounter]);
			//tileObjects [slots[temp]].GetComponent<Image> ().sprite = tilePictures [picCounter];
			slots.RemoveAt(temp);
		}
	}
}

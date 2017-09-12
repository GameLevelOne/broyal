using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPairing : MonoBehaviour {
	public Transform tileParent;
	public GameObject tilePrefab;
	public GameObject overlay;
	public Text countdownText;

	Vector3 tileStartPos = new Vector3(-230f,230f,0);
	int areaSize = 4;
	int totalBlue = 8;
	int totalRed = 8;
	int counterBlue = 0;
	int counterRed = 0;
	bool gameOver = false;

	GameObject[] tileObjects = new GameObject[16];
	int tileCounter = 0;

	void Start(){
		GenerateTiles();
		StartCoroutine(GameTimer());
	}

	void OnEnable(){
		ColorPairingTile.OnTileClicked += HandleOnTileClicked;
	}

	void OnDisable(){
		ColorPairingTile.OnTileClicked -= HandleOnTileClicked;
	}

	void HandleOnTileClicked (bool isBlue)
	{
		if(counterBlue != totalBlue && counterRed != totalRed){
			if(isBlue){
				counterBlue++;
			}else{
				counterRed++;
			}
			if(counterBlue == totalBlue || counterRed == totalRed){
				gameOver=true;
			}
		} 

		if(gameOver){
			overlay.SetActive(true);
			Debug.Log("You Win");
		}
	}

	void GenerateTiles ()
	{
		for (int i = 0; i < areaSize; i++) {
			for (int j = 0; j < areaSize; j++) {
				GameObject tileObj = Instantiate (tilePrefab, tileParent, false);
				tileObj.transform.localPosition = new Vector3 ((tileStartPos.x + j * 150f), (tileStartPos.y - i * 150f), 0);

				tileObjects[tileCounter] = tileObj;
				tileCounter++;
				RandomColor(tileObj);
			}
			counterBlue=0;
			counterRed=0;
		}
	}

	void RandomColor(GameObject obj){
		if(counterBlue < 2 && counterRed < 2){
			if(Random.value <= 0.5f){
				obj.GetComponent<ColorPairingTile>().InitTile(true);
				counterBlue++;
			} else{
				obj.GetComponent<ColorPairingTile>().InitTile(false);
				counterRed++;
			}
		} else{
			if(counterBlue == 2){
				obj.GetComponent<ColorPairingTile>().InitTile(false);
				counterRed++;
			} else{
				obj.GetComponent<ColorPairingTile>().InitTile(true);
				counterBlue++;
			}
		}
	}

	void ResetGame(){
		counterBlue = 0;
		counterRed = 0;
		for(int i=0;i<tileObjects.Length;i++){
			RandomColor(tileObjects[i]);
		}
	}

	IEnumerator WaitForReset(){
		yield return new WaitForSeconds(2);
		ResetGame();
	}

	IEnumerator GameTimer(){
		for(int i=5;i>=0;i--){
			yield return new WaitForSeconds(1);
			countdownText.text = "0"+i.ToString();
		}
//		SoundManager.Instance.PlaySFX(SFXList.TimeUp);
		gameOver = true;
		overlay.SetActive(true);
		StartCoroutine(WaitForReset());
	}
}

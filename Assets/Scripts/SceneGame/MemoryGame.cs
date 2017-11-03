using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGame : PagesIntroOutro {
	public SceneGameManager gameManager;
	public PanelTrainingScores panelScore;

	public Transform tileParent;
	public GameObject tilePrefab;
	public GameObject overlay;
	public Text overlayLabel;
	public GameObject overlayTile;
	public Text countdownText;
	public Sprite[] tilePictures;

	GameObject[] tileObjects = new GameObject[6];
	int tileCounter=0;
	int picCounter=0; 
	int pairCounter=0; 
	int answerCount=0;
	int currPicValue = -1;
	int currTileIdx = -1;
	int[] pairTileIdx = new int[2];
	bool win = false;
	bool isAPair = false;
	float gameTimer = 0f;
	float gameTimeLimit = 6f;
	List<int> pictureValues = new List<int>();
	List<int> slots = new List<int>();

	void Start(){
		GenerateTiles();
		StartCoroutine(Countdown());
		StartCoroutine(CountPlayTime());
	}

	void OnEnable(){
		MemoryGameTile.OnMemoryTileClicked += HandleOnMemoryTileClicked;
	}

	void OnDisable(){
		MemoryGameTile.OnMemoryTileClicked -= HandleOnMemoryTileClicked;
	}

	void HandleOnMemoryTileClicked (int picValue, int tileIdx)
	{
		if (currTileIdx != tileIdx) {
			pairCounter++;
			if (currPicValue == -1) {
				currPicValue = picValue;
				pairTileIdx [0] = tileIdx;
			} else {
				pairTileIdx [1] = tileIdx;
				if (currPicValue == picValue) {
					isAPair = true;
				} 
			}

			if (pairCounter == 2) {
				pairCounter = 0;
				currPicValue = -1;
				if (isAPair) {
					answerCount++;
					isAPair = false;
					if (answerCount == 3) {
						win = true;
						GameOver ();
					}
					Debug.Log ("answerCount:" + answerCount);
				} else {
					StartCoroutine (WaitForResetTile (2));
				}
			}
			currTileIdx = tileIdx;
		}
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
        List<int> picIndex = new List<int>();
        for (int i = 0; i < tilePictures.Length; i++)
        {
            picIndex.Add(i);
        }

		for (int i = 0; i < 3; i++) {
            int picTemp = Random.Range(0, picIndex.Count);
			int temp = Random.Range (0, slots.Count);
            tileObjects[slots[temp]].GetComponent<MemoryGameTile>().InitTile(picIndex[picTemp], slots[temp], tilePictures[picIndex[picTemp]]);
			slots.RemoveAt(temp);
            temp = Random.Range(0, slots.Count);
            tileObjects[slots[temp]].GetComponent<MemoryGameTile>().InitTile(picIndex[picTemp], slots[temp], tilePictures[picIndex[picTemp]]);
            slots.RemoveAt(temp);
            picIndex.RemoveAt(picTemp);
        }
	}

	void ResetTile (int amount)
	{
		for (int i = 0; i < amount; i++) {
			if (amount == 2) {
				tileObjects [pairTileIdx [i]].GetComponent<MemoryGameTile> ().Reset();
				pairTileIdx [i] = -1;
			} else{
				tileObjects [i].GetComponent<MemoryGameTile> ().Reset();
			}
		}
		overlay.SetActive(false);
		overlayTile.SetActive(false);
	}

	void GameOver(){
		overlay.SetActive(true);
		if (win) {
			overlayLabel.text = LocalizationService.Instance.GetTextByKey ("Game.CONGRATULATIONS");
		} else {
			overlayLabel.text = LocalizationService.Instance.GetTextByKey ("Game.TIMES_UP");
			gameTimer = 6f;
		}

		StopAllCoroutines();

		gameManager.EndGame (gameTimer);
	}

	IEnumerator WaitForResetTile(int amount){
		overlayTile.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		ResetTile(amount);
	}

	IEnumerator Countdown ()
	{
		countdownText.text = "06";
		for (int i = 5; i >= 0; i--) {
			yield return new WaitForSeconds (1);
			countdownText.text = "0" + i.ToString ();
		}
//		SoundManager.Instance.PlaySFX(SFXList.TimeUp);
		win = false;
		GameOver();
	}

	IEnumerator CountPlayTime(){
		while(gameTimer < gameTimeLimit){
			gameTimer += Time.deltaTime;
			yield return null;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGame : BaseGame {
	public MemoryGameTile[] tiles;
	public Sprite[] tilePictures;
	public Sprite tileback;
	public GameObject tileOverlay;

	MemoryGameTile[] pair;
	int openTile;
	int correctPair;

	public override void InitGame (int gameTime, int round)
	{
		List<int> slots = new List<int> ();
		for (int i = 0; i < tiles.Length; i++) {
			slots.Add (i);
		}
		List<int> picPool = new List<int>();
		for (int i = 0; i < tilePictures.Length; i++)
		{
			picPool.Add(i);
		}

		for (int i = 0; i < tiles.Length/2; i++) {
			int ipic = Random.Range(0, picPool.Count);
			int islots = Random.Range (0, slots.Count);
			tiles[slots [islots]].InitTile(tilePictures[picPool [ipic]],tileback);
			slots.RemoveAt(islots);
			islots = Random.Range(0, slots.Count);
			tiles[slots [islots]].InitTile(tilePictures[picPool [ipic]],tileback);
			slots.RemoveAt(islots);
			picPool.RemoveAt(ipic);
		}
		openTile = 0;
		correctPair = 0;
		pair = new MemoryGameTile[2];
		tileOverlay.SetActive (false);
		base.InitGame (gameTime, round);
	}
		
	void CheckTilePair(MemoryGameTile tile) {
		pair [openTile] = tile;
		openTile++;
		if (openTile > 1) {
			if (pair [0].tileImage.sprite == pair [1].tileImage.sprite) {
                SoundManager.Instance.PlaySFX(SFXList.CardMatch);
                correctPair++;
				if (correctPair >= tiles.Length / 2) {
					EndGame (true);
				}
			} else {
                SoundManager.Instance.PlaySFX(SFXList.Incorrect);
                StartCoroutine(DelayFlipTile(0.2f));
			}
			openTile = 0;
		}
	}
	IEnumerator DelayFlipTile(float sec) {
		tileOverlay.SetActive (true);
		yield return new WaitForSeconds (sec);
		tileOverlay.SetActive (false);
		pair [0].FlipTile();
		pair [1].FlipTile();
	}


	new protected void OnEnable() {
		base.OnEnable ();
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].OnFinishFlip += CheckTilePair;
		}
	}
	new protected void OnDisable() {
		base.OnDisable ();
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].OnFinishFlip -= CheckTilePair;
		}
	}



}

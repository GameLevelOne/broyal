using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPairing : BaseGame {

	public ColorPairingTile[] tiles;
//    public GameObject tileOverlay;
	bool onCheckingAll;

	public override void InitGame (int gameTime, int round)
	{
//        tileOverlay.SetActive(false);
		List<int> slots = new List<int> ();
		for (int i = 0; i < tiles.Length; i++) {
			slots.Add (i);
		}
		for (int i = 0; i < tiles.Length/2; i++) {
			int randomSlot = Random.Range (0,slots.Count);
			tiles[slots[randomSlot]].InitTile(0);
			slots.RemoveAt (randomSlot);
		}
		while (slots.Count > 0) {
			int randomSlot = Random.Range (0,slots.Count);
			tiles[slots[randomSlot]].InitTile(1);
			slots.RemoveAt (randomSlot);
		}

		onCheckingAll = false;
		base.InitGame (gameTime, round);
	}

	void CheckTileAll() {
		if (!onCheckingAll) {
			onCheckingAll = true;
			bool sameColor = true;
			for (int i = 1; i < tiles.Length; i++) {
				if (tiles [0].tileImage.color != tiles [i].tileImage.color) {
					sameColor = false;
					break;
				}
			}

			if (sameColor) {
				EndGame (true);
			}

			onCheckingAll = false;
		}
	}
	new protected void OnEnable() {
		base.OnEnable ();
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].OnFinishFlip += CheckTileAll;
		}
	}
	new protected void OnDisable() {
		base.OnDisable ();
		for (int i = 0; i < tiles.Length; i++) {
			tiles [i].OnFinishFlip -= CheckTileAll;
		}
	}

}

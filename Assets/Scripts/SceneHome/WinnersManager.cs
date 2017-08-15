using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnersManager : MonoBehaviour {
	public Transform buttonHighlight;

	float buttonRumbleXPos = -156f;
	float buttonRoyaleXPos = 121f;

	public void OnClickAuctionType (int type)
	{
		if (type == 0) {
			buttonHighlight.localPosition = new Vector3 (buttonRumbleXPos, 0, 0);
		} else {
			buttonHighlight.localPosition = new Vector3 (buttonRoyaleXPos, 0, 0);
		}
	}
}

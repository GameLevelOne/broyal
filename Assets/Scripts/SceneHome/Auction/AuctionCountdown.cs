using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuctionCountdown : MonoBehaviour {
	public Animator anim;

	public void SetState() {
		anim.SetInteger ("State",1);
	}
}

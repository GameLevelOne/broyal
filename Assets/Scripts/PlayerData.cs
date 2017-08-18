using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
	private static PlayerData instance;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance=this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public static PlayerData Instance{ get { return instance; }}
}

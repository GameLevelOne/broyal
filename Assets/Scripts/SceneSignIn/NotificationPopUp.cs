using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopUp : PagesIntroOutro {

	public Text notificationText;

	public void ShowPopUp(string textToShow) {
		notificationText.text = textToShow;
		Activate (true);
	}

}

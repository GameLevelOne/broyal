using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : BasePage {

	public Text titleLabel;
	public ScrollRect scrollView;
	public SignUpManager signUpManager;

	public int showType;
	public string[] titles;
	public GameObject[] showTextEn;
	public GameObject[] showTextId;

	protected override void Init ()
	{
		base.Init ();
		titleLabel.text = LocalizationService.Instance.GetTextByKey (titles [showType]);
		for (int i = 0; i < titles.Length; i++) {
			showTextEn [i].SetActive (false);
			showTextId [i].SetActive (false);
		}
		if (LocalizationService.Instance.Localization == "English") {			
			showTextEn [showType].SetActive (true);
			scrollView.content = showTextEn [showType].GetComponent<RectTransform> ();
			showTextEn [showType].GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		} else {
			showTextId [showType].SetActive (true);
			scrollView.content = showTextId [showType].GetComponent<RectTransform> ();
			showTextId [showType].GetComponent<RectTransform> ().anchoredPosition = Vector2.zero;
		}
	}

	public void CloseText() {
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		NextPage ("SETTINGS");
	}

	public void CloseFromSignIn() {
		SoundManager.Instance.PlaySFX(SFXList.Button02);
		signUpManager.Activate (true);
		Activate (false);
	}
}

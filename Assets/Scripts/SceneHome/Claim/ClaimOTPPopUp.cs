using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class ClaimOTPPopUp : BasePage {
	public ConnectingPanel connectingPanel;
	public NotificationPopUp notifPopUp;

	public InputField inputOtp;
	public Text min1;
	public Text min2;
	public Text sec1;
	public Text sec2;
	public Button enterButton;

	int auctionId;
	float timeLeft;
	public bool successOTP;
	protected override void Init ()
	{
		enterButton.interactable = false;
		successOTP = false;
        inputOtp.text = "";
		StartCoroutine (StartCountDown ());
		base.Init ();
	}

	public void InitTime(int _auctionId,int _time) {
		timeLeft = ((float)_time) / 1000f;
		auctionId = _auctionId;
	}

	IEnumerator StartCountDown() {
		Debug.Log ("OTP Time: "+timeLeft);
		while (timeLeft > 0f) {
			timeLeft -= Time.deltaTime;

			min1.text = (Mathf.Floor(timeLeft / 60f) / 10f).ToString ("0");
			min2.text = (Mathf.Floor(timeLeft / 60f) % 10f).ToString ("0");
			sec1.text = Mathf.Floor((timeLeft % 60f) / 10f).ToString ("0");
			sec2.text = Mathf.Floor((timeLeft % 60f) % 10f).ToString ("0");

//			Debug.Log ("Time: "+timeLeft+", sec1: "+Mathf.Floor((timeLeft % 60f) / 10f)+", sec2: "+((timeLeft % 60f) % 10f));

			yield return null;
		}

		Activate (false);
	}

	public void CheckInput() {
		if (inputOtp.text != "") {
			enterButton.interactable = true;
		}
	}

	public void ClickSendAgain() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting (true);
		DBManager.API.GetClaimAuction (auctionId,
			(response) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse(response);
				timeLeft = jsonData["otpRemainingTime"].AsInt / 1000f;
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error);
				if (jsonData!=null) {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
				} else {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
			}
		);
	}

	public void ClickEnter() {
		SoundManager.Instance.PlaySFX(SFXList.Button01);
		connectingPanel.Connecting (true);
		DBManager.API.SubmitClaimedOtp (auctionId, inputOtp.text,
			(response) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse(response);
				successOTP = true;
				Activate(false);
			}, 
			(error) => {
				connectingPanel.Connecting (false);
				JSONNode jsonData = JSON.Parse (error);
				if (jsonData!=null) {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("Error."+jsonData["errors"]));
				} else {
					notifPopUp.ShowPopUp (LocalizationService.Instance.GetTextByKey("General.SERVER_ERROR"));
				}
			}
		);
	}

}

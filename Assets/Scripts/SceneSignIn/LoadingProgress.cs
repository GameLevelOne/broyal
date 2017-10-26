using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingProgress : MonoBehaviour {
	public Fader fader;
	public Image progressBar;
	public CanvasGroup cg;
	public string nextScene;
	AsyncOperation asyncOp;

	void OnEnable() {
		cg.alpha = 0;
		StartCoroutine(FadeIn());
	}

	IEnumerator FadeIn() {
		while (cg.alpha < 1) {
			cg.alpha += (Time.deltaTime * 2f);
			yield return null;
		}
		cg.alpha = 1;
		yield return null;
		StartCoroutine(LoadNextScene());
	}
		
	IEnumerator LoadNextScene ()
	{
//		Debug.Log("Start loading");
		asyncOp = SceneManager.LoadSceneAsync(nextScene);
		asyncOp.allowSceneActivation = false;
		float delayProgress = 0f;

		while (progressBar.fillAmount<0.9f) {
			progressBar.fillAmount = ( asyncOp.progress > delayProgress ? delayProgress : asyncOp.progress);
			delayProgress += Time.deltaTime;
//			Debug.Log("Progress: "+progressBar.fillAmount);
			yield return null;
		}

//		Debug.Log("Progress: "+asyncOp.progress);
		progressBar.fillAmount = 1;
		yield return null;

		fader.OnFadeOutFinished += OnFinishFadeOut;
		fader.FadeOut ();
	}

	void OnFinishFadeOut() {
		fader.OnFadeOutFinished -= OnFinishFadeOut;
		asyncOp.allowSceneActivation=true;
		Debug.Log("Load: "+nextScene);
	}

}

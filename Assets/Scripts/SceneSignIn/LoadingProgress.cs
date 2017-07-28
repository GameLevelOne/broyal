using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingProgress : MonoBehaviour {
	public static LoadingProgress Instance{ get { return instance;}}
	public Image progressBar;

	private static LoadingProgress instance;
	bool barIsFilled = false;
	string nextScene;

	void Awake ()
	{
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}
	}

	public void ChangeScene(string nextScene){
		this.nextScene = nextScene;
		StartCoroutine(LoadNextScene());
	}

	IEnumerator LoadNextScene ()
	{
		Debug.Log("start loading");
		AsyncOperation asyncOp = SceneManager.LoadSceneAsync(nextScene);
		asyncOp.allowSceneActivation = false;

		while (!barIsFilled) {
			progressBar.fillAmount += 0.05f;
			if (progressBar.fillAmount == 1f) {
				barIsFilled=true;
			}
			yield return null;
		}

		if(barIsFilled){
			Debug.Log("load scene");
			asyncOp.allowSceneActivation=true;
		}
	}
}

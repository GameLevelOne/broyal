using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImageAndVideoPicker;
using System.IO;

public class TestImagePicker : MonoBehaviour {
	public Text locText;
    public ImageLoader image;
	Texture2D texture;

	public void UploadPicture() {
		#if UNITY_ANDROID
		AndroidPicker.BrowseImage(false);
		#elif UNITY_IPHONE
		IOSPicker.BrowseImage(false); // true for pick and crop
		#endif
	}

	void OnImageLoad(string imgPath, Texture2D tex, ImageAndVideoPicker.ImageOrientation imgOrientation)
	{
		string filePath = "file:/" + imgPath;
		bool existed = File.Exists (imgPath);
		locText.text = filePath + (existed ? "\n\nFile Exists!" : "\n\nFile not found!");
		locText.text += "\n\nTexture Size: " + tex.width + " x " + tex.height;
		texture = tex;
		Sprite spr = image.SetSpriteFromTexture(tex);
		if (spr != null) {
			locText.text += "\n\nSprite Created!";
		} else {
			locText.text += "\n\nSprite Not Created!";
		}
	}

	void Start() {
		PickerEventListener.onImageLoad += OnImageLoad;
		texture = null;
	}
	void OnDestroy() {
		PickerEventListener.onImageLoad -= OnImageLoad;
	}


}

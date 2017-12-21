using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ImageAndVideoPicker;
using System.IO;

public class TestImagePicker : MonoBehaviour {
	public Text locText;
	public ImageLoader imageLoader;

	public void UploadPicture() {
		#if UNITY_ANDROID
		AndroidPicker.BrowseImage(false);
		#elif UNITY_IPHONE
		IOSPicker.BrowseImage(false); // true for pick and crop
		#endif
		PickerEventListener.onImageSelect += OnImageSelect;
	}

	void OnImageSelect(string imgPath, ImageAndVideoPicker.ImageOrientation imgOrientation)
	{
		string filePath = "file:/" + imgPath;
		bool existed = File.Exists (filePath);
		locText.text = filePath + (existed ? "\n\nFile Exists!" : "\n\nFile not found!");
		imageLoader.LoadImageFromUrl (filePath, false);
	}

	void Start() {
		PickerEventListener.onImageSelect += OnImageSelect;
	}
	void OnDestroy() {
		PickerEventListener.onImageSelect -= OnImageSelect;
	}

}

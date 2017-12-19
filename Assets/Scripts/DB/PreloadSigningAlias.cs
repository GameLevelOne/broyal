#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class PreloadSigningAlias
{

	static PreloadSigningAlias ()
	{
		PlayerSettings.Android.keystorePass = "BidRoyaleKeystore";
		PlayerSettings.Android.keyaliasName = "bidroyalekey";
		PlayerSettings.Android.keyaliasPass = "BidRoyaleKeystore";
	}

}

#endif
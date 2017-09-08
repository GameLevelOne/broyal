using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour {
	public delegate void InputChar();
	public static event InputChar OnInputChar;

	public char currInput;

	public void OnClickKey(string input){
		currInput = input[0];
		OnInputChar();
	}
}

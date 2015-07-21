using UnityEngine;
using System.Collections;
using System;

//Inputs: WASD,QE,Maus1,Shift,leert
//INputs: JoyS,L1R1,R2,JoyS2,L2

public class PlayerJoystickClass : MonoBehaviour {
	
	private string currentButton;
	private float[] axisInput = new float[4];

	void Start () {
		for(int i = 0; i < axisInput.Length; i++)
			axisInput[i] = 0.0f;
		
	}
	
	void Update () {
		// Get the currently pressed Gamepad Button name
		var values = Enum.GetValues(typeof(KeyCode));
		for(int x = 0; x < values.Length; x++) {
			if(Input.GetKeyDown((KeyCode)values.GetValue(x))){
				currentButton = values.GetValue(x).ToString();
			}
		}
	}

	void OnGUI(){
		GUI.TextArea(new Rect(0, 0, 250, 40), "Current Button : " + currentButton);
	}
}
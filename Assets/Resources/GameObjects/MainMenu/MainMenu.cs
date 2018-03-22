using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenu : MonoBehaviour {

	private bool inStart = false;
	TextMeshProUGUI startText;
	// Use this for initialization
	void Start () {
//		startText = transform.Find ("StartButton/StartText").GetComponent<TextMeshProUGUI> ();
//		startText.color = new Color (0xFF, 0x78, 0x78);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MouseEnterStart(){
		inStart = true;
		print (transform);
		startText.color = new Color (0xFF, 0x78, 0x78);
	}

	public void MouseExitStart(){
		inStart = false;
		print (transform);
		startText.color = new Color (0xFF, 0xFF, 0xFF);
	}

	public void MouseClick(){
		if (inStart) {
			SceneManager.LoadScene (1);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private bool pointerStart = false;
	public GameObject levelSelectScreen;
	public GameObject mainMenuScreen;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0) && pointerStart) {
			if (GameData.Level1Rating == 0) {
				SceneManager.LoadScene (1);
			} else {
				levelSelectScreen.SetActive (true);
				mainMenuScreen.SetActive (false);
			}
		}
	}

	public void OnPointerEnter( PointerEventData eventData )
	{
		//print ("POINTER ENTER");
		//startText.faceColor = new Color32 (0xFF, 0x78, 0x78, 0xFF);
		pointerStart = true;
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 1.08f, "y", 1.08f, "time", 0.5f));
		//startText.color = new Color (0xFF, 0x78, 0x78);
	}

	public void OnPointerExit( PointerEventData eventData )
	{
		//print ("POINTER EXIT");
		//startText.faceColor = new Color32 (0xFF, 0xFF, 0xFF, 0xFF);
		pointerStart = false;
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 1f, "y", 1f, "time", 0.5f));
		//startText.color = new Color (0xFF, 0xFF, 0xFF);
	}
}

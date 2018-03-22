using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class ExtrasButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private bool pointerStart = false;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0) && pointerStart) {
			SceneManager.LoadScene (1);
		}
	}

	public void OnPointerEnter( PointerEventData eventData )
	{
		pointerStart = true;
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 1.05f, "y", 1.05f, "time", 0.5f));
	}

	public void OnPointerExit( PointerEventData eventData )
	{
		pointerStart = false;
		iTween.ScaleTo (gameObject, iTween.Hash ("x", 1f, "y", 1f, "time", 0.5f));
	}
}

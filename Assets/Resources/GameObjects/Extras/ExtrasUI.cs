using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ExtrasUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		int xCount = 0;
		xCount += GameData.Level1Rating;
		xCount += GameData.Level2Rating;
		xCount += GameData.Level3Rating;
		xCount += GameData.Level4Rating;
		transform.Find ("XCount/XCountText").GetComponent<TextMeshProUGUI>().SetText(xCount+"/12");
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.Escape)) {
			SceneManager.LoadScene (0);
		}
	}
}

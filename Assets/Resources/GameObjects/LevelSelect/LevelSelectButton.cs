using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private int levelIndex = 1;
	private bool pointerStart = false;
	// Use this for initialization
	void Start () {
		ChangeLevel (1);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0) && pointerStart) {
			SceneManager.LoadScene (levelIndex);
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			if (levelIndex < 4 && GetLevelRating(levelIndex) != 0) {
				levelIndex += 1;
				ChangeLevel (levelIndex);
			}
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			if (levelIndex > 1) {
				levelIndex -= 1;
				ChangeLevel (levelIndex);
			}
		}
	}

	int GetLevelRating(int level){
		if (level == 1) {
			return GameData.Level1Rating;
		} else if (level == 2) {
			return GameData.Level2Rating;
		} else if (level == 3) {
			return GameData.Level3Rating;
		} else if (level == 4) {
			return GameData.Level4Rating;
		}
		return -100;
	}

	void ChangeLevel(int level){
		transform.Find ("LevelText").GetComponent<TextMeshProUGUI> ().SetText ("LEVEL " + level);
		if (GetLevelRating (level) == 0) {
			transform.Find ("RatingText").GetComponent<TextMeshProUGUI> ().SetText ("RATING: NONE");
		} else {
			string ratingString = "RATING: ";
			for(int a = 0; a < GetLevelRating (level); a++){
				ratingString += "X";
			}
			transform.Find ("RatingText").GetComponent<TextMeshProUGUI> ().SetText (ratingString);
		}
	}

	void ChangeRating(int rating){
		transform.Find ("RatingText").GetComponent<TextMeshProUGUI> ().SetText ("" + rating);
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

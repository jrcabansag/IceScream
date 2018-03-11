using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour {

	public int monsterCount = 5;
	private RawImage lives;
	private TextMeshProUGUI livesText;
	private RawImage death;
	private TextMeshProUGUI deathText;
	private RawImage combo;
	private TextMeshProUGUI comboText;
	private TextMeshProUGUI damageText;
	private int comboCount = 0;
	private float damageMultiplier = 1;
	private float myLives = 3;

	// Use this for initialization
	void Start () {
		lives = transform.Find ("Lives").GetComponent<RawImage>();
		livesText = transform.Find ("Lives/Text").GetComponent<TextMeshProUGUI> ();
		death = transform.Find ("Death").GetComponent<RawImage>();
		deathText = transform.Find ("Death/DeathText").GetComponent<TextMeshProUGUI> ();
		death.CrossFadeAlpha (0f, 0f, true);
		deathText.CrossFadeAlpha (0f, 0f, true);
		combo = transform.Find ("Combo").GetComponent<RawImage>();
		comboText = transform.Find ("Combo/ComboText").GetComponent<TextMeshProUGUI> ();
		damageText = transform.Find ("Combo/DamageText").GetComponent<TextMeshProUGUI> ();
		combo.CrossFadeAlpha (0f, 0f, true);
		comboText.CrossFadeAlpha (0f, 0f, true);
		damageText.CrossFadeAlpha (0f, 0f, true);
		iTween.ScaleTo(combo.gameObject, iTween.Hash("x", 0f, "y", 0f, "time", 0f));

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateCombo(){
		monsterCount -= 1;
		comboCount += 1;
		if (monsterCount <= 0) {
			ClearedStage ();
		}
		else if (comboCount >= 1) {
			UpdateComboDisplay ();
			UpdateDamage ();
		}
	}

	public void UpdateDamage(){
		if (comboCount == 1) {
			damageMultiplier = 1f;
			damageText.SetText ("1x DAMAGE");
		}
		else if (comboCount == 3) {
			damageMultiplier = 2f;
			damageText.SetText ("2x DAMAGE");
		}
		else if (comboCount == 8) {
			damageMultiplier = 3f;
			damageText.SetText ("3x DAMAGE");
		}
		else if (comboCount == 15) {
			damageMultiplier = 4f;
			damageText.SetText ("4x DAMAGE");
		}
		else if (comboCount == 25) {
			damageMultiplier = 5f;
			damageText.SetText ("5x DAMAGE");
		}
	}

	void UpdateComboDisplay(){
		comboText.SetText (comboCount + " KILL COMBO!");
		combo.CrossFadeAlpha (1f, 0f, true);
		comboText.CrossFadeAlpha (1f, 0f, true);
		damageText.CrossFadeAlpha (1f, 0f, true);
		CancelInvoke ("ComboFade");
		CancelInvoke ("KillCombo");
		Invoke ("ComboFade", 7f);
		Invoke ("KillCombo", 10f);
//		combo.CrossFadeAlpha (0f, 10f, true);
//		comboText.CrossFadeAlpha (0f, 10f, true);
//		damageText.CrossFadeAlpha (0f, 10f, true);
		iTween.ScaleTo(combo.gameObject, iTween.Hash("x", 1f, "y", 1f, "time", 0.5f));
		//iTween.PunchPosition(combo.gameObject, iTween.Hash("amount", new Vector3(0.3f, 0.3f, 1f), "time", 2f));
		//iTween.PunchScale(combo.gameObject, iTween.Hash("amount", new Vector3(0.3f, 0.3f, 1f), "time", 1f, "delay"));
		iTween.PunchScale(comboText.gameObject, iTween.Hash("amount", new Vector3(0.15f, 0.15f, 1f), "time", 1f));
		iTween.PunchScale(damageText.gameObject, iTween.Hash("amount", new Vector3(0.1f, 0.1f, 1f), "time", 1f));
		iTween.ScaleTo(combo.gameObject, iTween.Hash("x", 0.7f, "y", 0.7f, "time", 9f, "delay", 1f, "easeType", iTween.EaseType.easeInOutSine));
	}

	void ClearedStage(){
		comboText.SetText ("STAGE CLEARED!");
		string ratingString = "RATING: ";
		for(int a = 0; a < myLives; a++){
			ratingString += "X";
		}
		damageText.SetText (ratingString);
		combo.CrossFadeAlpha (1f, 0f, true);
		comboText.CrossFadeAlpha (1f, 0f, true);
		damageText.CrossFadeAlpha (1f, 0f, true);
		CancelInvoke ("ComboFade");
		CancelInvoke ("KillCombo");
		iTween.ScaleTo(combo.gameObject, iTween.Hash("x", 1f, "y", 1f, "time", 0.5f));
		iTween.PunchScale(comboText.gameObject, iTween.Hash("amount", new Vector3(0.15f, 0.15f, 1f), "time", 1f));
		iTween.PunchScale(damageText.gameObject, iTween.Hash("amount", new Vector3(0.1f, 0.1f, 1f), "time", 1f));
	}

	void KillCombo(){
		comboCount = 0;
		damageMultiplier = 1f;
		iTween.ScaleTo(combo.gameObject, iTween.Hash("x", 0f, "y", 0f, "time", 0f));
	}

	public void UpdateLives(int livesCount){
		myLives = livesCount;
		livesText.SetText ("LIVES: " + livesCount);
	}

	public void Died(){
//		death.enabled = true;
//		deathText.enabled = true;
		lives.CrossFadeAlpha (0f, 0.2f, true);
		livesText.CrossFadeAlpha (0f, 0.2f, true);
		combo.CrossFadeAlpha (0f, 0.2f, true);
		comboText.CrossFadeAlpha (0f, 0.2f, true);
		damageText.CrossFadeAlpha (0f, 0.2f, true);
//		lives.enabled = false;
//		livesText.enabled = false;
//		combo.enabled = false;
//		comboText.enabled = false;
//		damageText.enabled = false;
		Invoke ("ShowDeath", 3.5f);
	}

	void ComboFade(){
		combo.CrossFadeAlpha (0f, 3f, true);
		comboText.CrossFadeAlpha (0f, 3f, true);
		damageText.CrossFadeAlpha (0f, 3f, true);
	}

	void ShowDeath(){
		death.CrossFadeAlpha (1f, 0.2f, true);
		deathText.CrossFadeAlpha (1f, 0.2f, true);
	}

	public float GetDamageMultiplier(){
		return damageMultiplier;
	}
}

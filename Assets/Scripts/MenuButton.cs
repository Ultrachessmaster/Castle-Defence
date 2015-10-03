using UnityEngine;
using System.Collections;

public class MenuButton : MonoBehaviour {

	public GameObject panel;
	public GameObject light1;
	public GameObject light2;

	public void LoadLevel (string level) {
		Application.LoadLevel (level);
	}

	public void Quit () {
		Application.Quit ();
	}

	public void LoadWarningScreen () {
		panel.SetActive (true);
		light1.SetActive (false);
		light2.SetActive (false);
	}

	public void ResetAllProgress () {
		PlayerPrefs.DeleteAll ();
		panel.SetActive (false);
		light1.SetActive (true);
		light2.SetActive (true);
	}

	public void GoBack () {
		panel.SetActive (false);
		light1.SetActive (true);
		light2.SetActive (true);
	}

}

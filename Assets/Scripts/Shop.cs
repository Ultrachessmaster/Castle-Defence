using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviour {

	public int coins = 0;
	public int healthCost;
	public int swordLevel2;
	public int bowLevel2;
	public GameObject canvas;

	public void showShop () {
		canvas.SetActive (true);
		coins += 10;
		PlayerPrefs.SetInt ("coins", coins);
		GameObject.Find ("Coins").GetComponent <Text> ().text = ": " + coins.ToString ();
	}

	void Awake () {
		coins = PlayerPrefs.GetInt ("coins");
	}

	public void buyHealth () {
		if (coins >= healthCost) {
			coins -= healthCost;
			PlayerPrefs.SetInt ("health", PlayerPrefs.GetInt ("health") + 1);
			PlayerPrefs.SetInt ("coins", coins);
			GameObject.Find ("Coins").GetComponent <Text> ().text = ": " + coins.ToString ();
		}
	}

	public void buyBow2 () {
		if (coins >= bowLevel2) {
			coins -= bowLevel2;
			PlayerPrefs.SetInt ("bowlevel", 2);
			PlayerPrefs.SetInt ("coins", coins);
			GameObject.Find ("Coins").GetComponent <Text> ().text = ": " + coins.ToString ();
		}
	}

	public void buySword2 () {
		if (coins >= swordLevel2) {
			coins -= swordLevel2;
			PlayerPrefs.SetInt ("swordlevel", 2);
			PlayerPrefs.SetInt ("coins", coins);
			GameObject.Find ("Coins").GetComponent <Text> ().text = ": " + coins.ToString ();
		}
	}
}

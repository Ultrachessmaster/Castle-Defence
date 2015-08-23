using UnityEngine;
using System.Collections;

public class Shop : MonoBehaviour {

	public int coins = 0;
	public int healthCost;
	public int swordLevel2;
	public int bowLevel2;
	public GameObject canvas;

	public void showShop () {
		canvas.SetActive (true);
	}

	public void buyHealth () {
		if (coins >= healthCost) {
			coins -= healthCost;
			PlayerPrefs.SetInt ("health", PlayerPrefs.GetInt ("health") + 1);
		}
	}

	public void buyBow2 () {
		if (coins >= bowLevel2) {
			coins -= bowLevel2;
			PlayerPrefs.SetInt ("bowlevel", 2);
		}
	}

	public void buySword2 () {
		if (coins >= swordLevel2) {
			coins -= swordLevel2;
			PlayerPrefs.SetInt ("swordlevel", 2);
		}
	}
}

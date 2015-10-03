using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shop : MonoBehaviour {

	public int coins = 0;
	public int healthCost;
	public int swordLevel2Cost;
	public int bowLevel2Cost;
	private bool hasMegaBow = false;
	private bool hasMegaSword = false;
	public GameObject canvas;
	public GameObject sword2;
	public GameObject bow2;

	public void showShop () {
		canvas.SetActive (true);
		coins += 10;
		PlayerPrefs.SetInt ("coins", coins);
		GameObject.Find ("Coins").GetComponent <Text> ().text = ": " + coins.ToString ();
	}

	void Awake () {
		if (coins == 0)
			coins = PlayerPrefs.GetInt ("coins");
		if (PlayerPrefs.GetInt ("swordlevel") > 1) {
			GameObject sb = GameObject.Find ("Sword Button");
			Transform parent = sb.transform.parent;
			GameObject sword2Instance = (GameObject)Instantiate (sword2, sb.transform.position, Quaternion.identity);
			sword2Instance.transform.SetParent (parent, false);
			sword2Instance.GetComponent <RectTransform> ().localPosition = sb.GetComponent <RectTransform> ().localPosition;
			Destroy (sb);
			hasMegaSword = true;
		}
		if (PlayerPrefs.GetInt ("bowlevel") > 1) {
			GameObject bb = GameObject.Find ("Bow Button");
			Transform parent = bb.transform.parent;
			GameObject bow2Instance = (GameObject)Instantiate (bow2, bb.transform.position, Quaternion.identity);
			bow2Instance.transform.SetParent (parent, false);
			bow2Instance.GetComponent <RectTransform> ().localPosition = bb.GetComponent <RectTransform> ().localPosition;
			Destroy (bb);
			hasMegaBow = true;
		}
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
		if (coins >= bowLevel2Cost && !hasMegaBow) {
			coins -= bowLevel2Cost;
			PlayerPrefs.SetInt ("bowlevel", 2);
			PlayerPrefs.SetInt ("coins", coins);
			GameObject.Find ("Coins").GetComponent <Text> ().text = ": " + coins.ToString ();
		}
	}

	public void buySword2 () {
		if (coins >= swordLevel2Cost && !hasMegaSword) {
			coins -= swordLevel2Cost;
			PlayerPrefs.SetInt ("swordlevel", 2);
			PlayerPrefs.SetInt ("coins", coins);
			GameObject.Find ("Coins").GetComponent <Text> ().text = ": " + coins.ToString ();
		}
	}
}

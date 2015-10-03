using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {
	public int damage = 2;
	private float timer;
	private PlayerMovement pm;

	void Awake () {
		pm = GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerMovement> ();
	}

	void Update () {
		timer += Time.deltaTime;
		if (timer >= 0.8f) {
			pm.turnDone = false;
			GameManager.instance.isPlayerTurn = false;
			gameObject.SetActive (false);
			timer = 0;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Enemy")) {
			col.GetComponent <Enemy> ().lowerHealth (damage);
		}
	}
}
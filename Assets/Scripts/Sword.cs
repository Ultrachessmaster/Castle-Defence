using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {
	public int damage = 2;
	private float timer;
	private bool hitEnemy;
	private PlayerMovement pm;

	void Awake () {
		pm = GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerMovement> ();
	}

	void Update () {
		timer += Time.deltaTime;
		if (timer >= 0.8f && (!hitEnemy)) {
			pm.turnDone = false;
			GameManager.instance.isPlayerTurn = false;
			gameObject.SetActive (false);
		} else if (timer >= 0.8f)
			gameObject.SetActive (false);
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Enemy")) {
			col.GetComponent <Enemy> ().lowerHealth (damage);
			pm.turnDone = false;
			GameManager.instance.isPlayerTurn = false;
			hitEnemy = true;
		}
	}
}
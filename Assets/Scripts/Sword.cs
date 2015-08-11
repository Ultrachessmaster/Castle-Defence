using UnityEngine;
using System.Collections;

public class Sword : MonoBehaviour {
	public int damage = 2;
	private PlayerMovement pm;

	void Start () {
		pm = GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerMovement> ();
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Enemy")) {
			col.GetComponent <Enemy> ().lowerHealth (damage);
			pm.turnDone = false;
			GameManager.instance.isPlayerTurn = false;
		}
	}
}
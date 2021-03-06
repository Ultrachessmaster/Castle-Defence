﻿using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public float distance;
	public int damage = 1;

	private PlayerMovement pm;

	Vector2 playerPos;

	void Start () {
		playerPos = transform.position;
		pm = GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerMovement> ();
		GetComponent <AudioSource> ().Play ();
	}

	void Update () {
		if (Vector2.Distance (playerPos, transform.position) > distance) {
			gameObject.SetActive (false);
			//Set pm.turnDone to false because it's already false, and the player needs their turn making abilities back
			pm.turnDone = false;
			GameManager.instance.isPlayerTurn = false;

		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Enemy")) {
			col.GetComponent <Enemy> ().lowerHealth (damage);
			gameObject.SetActive (false);
			//Set pm.turnDone to false because it's already false, and the player needs their turn making abilities back
			pm.turnDone = false;
			GameManager.instance.isPlayerTurn = false;
		}
	}
}

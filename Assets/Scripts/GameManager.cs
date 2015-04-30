using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public bool isPlayerTurn = true;

	public readonly bool playerTurn;

	public static GameManager instance;

	void Awake () {
		instance = this;
	}

	void Start () {

	}

	void Update () {
		if (!isPlayerTurn) {
			bool enemiesAreDone = true;
			foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
				if (!g.GetComponent <Enemy> ().turnDone) {
					enemiesAreDone = false;
				}
			}
			if (enemiesAreDone) {
				isPlayerTurn = true;
				foreach (GameObject g in GameObject.FindGameObjectsWithTag ("Enemy")) {
					g.GetComponent <Enemy> ().resetTurn ();
				}
			}
		}
	}
}

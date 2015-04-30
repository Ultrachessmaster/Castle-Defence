using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public float distance;

	Vector2 playerPos;

	void Start () {
		playerPos = transform.position;
	}

	void Update () {
		if (Vector2.Distance (playerPos, transform.position) > distance) {
			gameObject.SetActive (false);
			GameManager.instance.isPlayerTurn = false;
		}
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Enemy")) {
			col.GetComponent <Enemy> ().lowerHealth (1);
			gameObject.SetActive (false); 
			GameManager.instance.isPlayerTurn = false;
		}
	}
}

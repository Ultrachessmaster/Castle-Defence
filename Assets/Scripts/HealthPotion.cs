using UnityEngine;
using System.Collections;

public class HealthPotion : MonoBehaviour {

	public int healthRegeneration;

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Player")) {
			col.GetComponent <PlayerMovement> ().gainHealth (1);
			Destroy (gameObject);
		}
	}
}

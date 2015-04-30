using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public TiledMap tm;

	public Vector2 goal;
	public float speed;
	public int blocksToTravel;
	public float accuracy;
	public int health = 2;
	private bool dead = false;

	private int tilesTraveled = 0;

	public GameObject bloodParticles;

	private bool playerIsNear;

	private Pathfinding2D pf2;
	private bool pathNotFound = true;

	public bool turnDone {
		get; private set;
	}

	private bool isTurnCalculated;

	void Start () { pf2 = GetComponent <Pathfinding2D> (); }

	void FixedUpdate () {
		if(!GameManager.instance.isPlayerTurn && !dead) {
			if (pathNotFound) {
				pf2.FindPath (transform.position, goal);
				pathNotFound = false;
			}
			playerIsNear = false;
			if (!isTurnCalculated) {
				playerIsNear = isPlayerNear ();
				isTurnCalculated = true;
			}

			if (pf2.Path.Count > 0 && tilesTraveled < blocksToTravel && !playerIsNear) {
				move ();
			} else if (tilesTraveled == blocksToTravel) 
				turnDone = true;

			if (playerIsNear && !turnDone) {
				GameObject.FindGameObjectWithTag ("Player").GetComponent <PlayerMovement> ().loseHealth (1);
				turnDone = true;
			}
		}
	}

	bool isPlayerNear () {
		playerIsNear = false;
		if (Vector2.Distance (GameObject.FindGameObjectWithTag ("Player").transform.position, transform.position) <= 1)
			playerIsNear = true;
		return playerIsNear;
	}

	void move()
	{
		Vector3 p = pf2.Path[0];
		p = new Vector3 (Mathf.Floor (p.x), Mathf.Floor (p.y), -4);
		transform.position = Vector3.MoveTowards(transform.position, p, speed);
		if (Vector2.Distance(transform.position, p) < accuracy) {
			transform.position = p;
			pf2.Path.RemoveAt(0);
			tilesTraveled++;
		}
	}

	public void lowerHealth (int healthLost) {
		health -= healthLost;
		if (health == 0) {
			dead = true;
			GetComponent <Animator> ().SetBool ("isDead", true);
			GetComponent <Collider2D> ().enabled = false;
			Destroy (gameObject, 5);
		}
		GameObject partcles = (GameObject)Instantiate (bloodParticles, new Vector3 (transform.position.x + 0.5f, transform.position.y + 0.5f, -5), Quaternion.identity);
		Destroy (partcles, 5);
	}

	public void resetTurn () {
		turnDone = false;
		isTurnCalculated = false;
		tilesTraveled = 0;
	}
}

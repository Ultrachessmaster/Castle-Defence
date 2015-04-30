using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Transform selectionCube;
	public GameObject arrow;
	public float arrowSpeed;

	public int health;
	private bool dead = false;

	public GameObject bloodParticles;

	Vector2 mousePosition;
	Vector2 oldMousePos;
	Vector2 sCubePosition;
	Vector2 velocity;
	bool firstPath;
	bool secondPath;
	bool xposdone = false;
	bool yposdone = false;

	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		if(Input.GetMouseButtonDown (0)) {
			calculateVelocity ();
			if (Vector2.Distance (mousePosition, oldMousePos) == 0) {
				if (calculateFirstPath (oldMousePos)) {
					firstPath = true;
				} else if (calculateSecondPath (oldMousePos)) {
					secondPath = true;
				}
			}
			else {
				ShootArrow (velocity);
			}
		}
	}

	void FixedUpdate () {

		sCubePosition = new Vector2 (MouseOver.mouseCoor.x + 0.5f, MouseOver.mouseCoor.y + 0.5f);
		sCubePosition = new Vector3 (Mathf.Clamp (sCubePosition.x, transform.position.x - 1.5f, transform.position.x + 2.5f),
			Mathf.Clamp (sCubePosition.y, transform.position.y - 1.5f, transform.position.y + 2.5f), -3f);

		selectionCube.position = sCubePosition;

		mousePosition = MouseOver.mouseCoor;
		if (GameManager.instance.isPlayerTurn && !dead) {
			if (firstPath) {
				if (!xposdone) {
					MoveXPlayer (oldMousePos);
				}
				else if (!yposdone) {
					MoveYPlayer (oldMousePos);
				} 
				else {
					firstPath = false;
					xposdone = false;
					yposdone = false;
					GameManager.instance.isPlayerTurn = false;
				}
			}

			if (secondPath) {
				if (!yposdone) {
					MoveYPlayer (oldMousePos);
				}
				else if (!xposdone) {
					MoveXPlayer (oldMousePos);
				} 
				else {
					secondPath = false;
					xposdone = false;
					yposdone = false;
					GameManager.instance.isPlayerTurn = false;
				}
			}
		}
	}

	void ShootArrow (Vector2 veloctity) {
		GameObject arrow = (GameObject) Instantiate (this.arrow, transform.position, Quaternion.identity);
		arrow.GetComponent <Rigidbody2D> ().velocity = Mathf.Abs (velocity.x) > Mathf.Abs (velocity.y) ? new Vector2 (velocity.x * arrowSpeed, 0) : new Vector2 (0, velocity.y * arrowSpeed);
	}

	void calculateVelocity () {
		oldMousePos = new Vector2 (sCubePosition.x - 0.5f, sCubePosition.y - 0.5f);
		velocity = oldMousePos - new Vector2(transform.position.x, transform.position.y);
		if(velocity.x > 0) {
			velocity.x = 1;
		}
		else if (velocity.x < 0)
			velocity.x = -1;
		if(velocity.y > 0) {
			velocity.y = 1;
		}
		else if (velocity.y < 0)
			velocity.y = -1;
	}

	bool calculateFirstPath (Vector2 pos) {
		RaycastHit2D rchx = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2 (velocity.x, 0), Mathf.Abs (transform.position.x - pos.x), 8, -100, 100);
		RaycastHit2D rchy = Physics2D.Raycast (new Vector2 (pos.x + 0.5f, transform.position.y + 0.5f), new Vector2 (0, velocity.y), Mathf.Abs (transform.position.x - pos.x), 8, -100, 100);
		
		if (rchx.collider != null)
			return false;
		if (rchy.collider != null)
			return false;

		//Debug.Log ("First Path workes, rchx collider is " + rchx.collider + " and rchy collider is " + rchy.collider + " . ");

		return true;
	}

	bool calculateSecondPath (Vector2 pos) {
		RaycastHit2D rchy = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2 (0, velocity.y), Mathf.Abs (transform.position.y - pos.y), 8, -100, 100);
		RaycastHit2D rchx = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, pos.y + 0.5f), new Vector2 (velocity.x, 0), Mathf.Abs (transform.position.y - pos.y), 8, -100, 100);
		if (rchy.collider != null)
			return false;
		if (rchx.collider != null)
			return false;

		//Debug.Log ("Second Path workes, rchx collider is " + rchx.collider + " and rchy collider is " + rchy.collider + " . ");

		return true;
	}
		
	void MoveXPlayer (Vector2 position) {
		Rigidbody2D rb2D = GetComponent <Rigidbody2D> ();
		rb2D.velocity = new Vector2 (velocity.x * 2, 0);
		if(Mathf.Abs (transform.position.x - position.x) < 0.02f) {
			rb2D.MovePosition (new Vector2 (position.x, transform.position.y));
			rb2D.velocity = Vector2.zero;
			xposdone = true;
		}
	}

	void MoveYPlayer (Vector2 position) {
		Rigidbody2D rb2D = GetComponent <Rigidbody2D> ();
		rb2D.velocity = new Vector2 (0, velocity.y * 2);
		if(Mathf.Abs (transform.position.y - position.y) < 0.02f) {
			rb2D.MovePosition (new Vector2 (transform.position.x, position.y));
			rb2D.velocity = Vector2.zero;
			yposdone = true;
		}
	}

	public void loseHealth (int healthLost) {
		health -= healthLost;
		GameObject partcles = (GameObject)Instantiate (bloodParticles, new Vector3 (transform.position.x + 0.5f, transform.position.y + 0.5f, -5), Quaternion.identity);
		Destroy (partcles, 5);
		if (health == 0) {
			dead = true;
			Debug.Log ("Player is deaded.");
		}
	}
}

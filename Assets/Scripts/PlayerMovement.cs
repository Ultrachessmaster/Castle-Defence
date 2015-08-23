using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Transform selectionCube;
	public GameObject arrow;
	public GameObject sword;
	public float arrowSpeed;

	public int health;
	private bool dead = false;

	public GameObject bloodParticles;

	public LayerMask terrain;

	bool arrowSelected;
	Vector2 cubePos;
	Vector3 sCubePosition;
	public float speed;
	public float accuracy;
	bool firstPath;
	bool secondPath;
	bool xposdone = false;
	bool yposdone = false;
	bool touchOffMovement;

	bool android;

	public bool turnDone = false;

	void Start () {
		android = Application.platform == RuntimePlatform.Android;
		health = PlayerPrefs.GetInt ("health");
	}

	void Update () {
		if (!android) {
			if(GameManager.instance.isPlayerTurn && !turnDone && !dead) {
				Vector2 velocity = calculateVelocity ();
				if (Input.GetAxis ("Fire1") > 0 && MouseOver.mouseCoor.x > 0) {
					firstPath = false;
					secondPath = false;
					if (calculateFirstPath (cubePos)) {
						firstPath = true;
						turnDone = true;
					} else if (calculateSecondPath (cubePos)) {
						secondPath = true;
						turnDone = true;
					}

				}
				else if (Input.GetAxis ("Fire2") > 0) {
					if (arrowSelected) {
						ShootArrow (velocity * arrowSpeed);
						turnDone = true;
					}
					else {
						SwingSword (velocity);
						turnDone = true;
					}
				}
			
			}
		} else {
			Vector2 pos = (Vector2)Camera.main.ScreenPointToRay (Input.touches[0].position).origin;
			if(GameManager.instance.isPlayerTurn && !turnDone && !dead && !touchOffMovement) {
				Vector2 velocity = calculateVelocity ();
				if (Input.touchCount > 0 && pos.x > 0) {
					Debug.Log (cubePos);
					if (calculateFirstPath (cubePos)) {
						firstPath = true;
						turnDone = true;
					} else if (calculateSecondPath (cubePos)) {
						secondPath = true;
						turnDone = true;
					}

				} else if (Input.touchCount > 0) {
					if (arrowSelected)
						ShootArrow (velocity);
					else
						SwingSword (velocity);
					turnDone = true;
				}

			}
		}
	}

	void FixedUpdate () {
		if (!android) {
			sCubePosition = new Vector3 (MouseOver.mouseCoor.x + 0.5f, MouseOver.mouseCoor.y + 0.5f, -3f);
			sCubePosition = new Vector3 (Mathf.Clamp (sCubePosition.x, transform.position.x - 1.5f, transform.position.x + 2.5f),
				Mathf.Clamp (sCubePosition.y, transform.position.y - 1.5f, transform.position.y + 2.5f), -3f);

			selectionCube.position = sCubePosition;
		} else {
			if (Input.touchCount > 0) {
				Vector2 touchPos = (Vector2)Camera.main.ScreenPointToRay(Input.touches[0].position).origin;
				if (Vector2.Distance (touchPos, (Vector2)transform.position + Vector2.one/2) > 2.5f) {
					sCubePosition = (Vector2)Camera.main.ScreenPointToRay(Input.touches[0].position).origin;
					sCubePosition = new Vector3 (Mathf.Floor (Mathf.Clamp (sCubePosition.x, transform.position.x - 2, transform.position.x + 2)) + 0.5f,
						Mathf.Floor (Mathf.Clamp (sCubePosition.y, transform.position.y - 2f, transform.position.y + 2f)) + 0.5f, -3f);
					selectionCube.position = sCubePosition;
					touchOffMovement = false;
				} else {
					touchOffMovement = true;
				}
			}
		}

		if (GameManager.instance.isPlayerTurn && !dead) {
			if (firstPath) {
				if (!xposdone) {
					MoveXPlayer (cubePos);
				}
				else if (!yposdone) {
					MoveYPlayer (cubePos);
				} 
				else {
					firstPath = false;
					xposdone = false;
					yposdone = false;
					turnDone = false;
					GameManager.instance.isPlayerTurn = false;
				}
			}

			if (secondPath) {
				if (!yposdone) {
					MoveYPlayer (cubePos);
				}
				else if (!xposdone) {
					MoveXPlayer (cubePos);
				} 
				else {
					secondPath = false;
					xposdone = false;
					yposdone = false;
					turnDone = false;
					GameManager.instance.isPlayerTurn = false;
				}
			}
		}
	}

	void ShootArrow (Vector2 velocity) {
		GameObject arrow = (GameObject) Instantiate (this.arrow, new Vector3 (transform.position.x + 0.5f, transform.position.y + 0.5f, transform.position.z - 1f), Quaternion.identity);
		arrow.GetComponent <Rigidbody2D> ().velocity = velocity;
		Vector3 angles = arrow.transform.eulerAngles;
		if (velocity.x > 0 && velocity.y > 0) angles.z = -135f;
		if (velocity.x < 0 && velocity.y > 0) angles.z = -45f;
		if (velocity.x < 0 && velocity.y < 0) angles.z = 45f;
		if (velocity.x > 0 && velocity.y < 0) angles.z = 135f;
		if (velocity.x == 0 && velocity.y > 0) angles.z = -90f;
		if (velocity.x == 0 && velocity.y < 0) angles.z = 90f;
		if (velocity.x < 0 && velocity.y == 0) angles.z = 0f;
		if (velocity.x > 0 && velocity.y == 0) angles.z = -180f;
		arrow.transform.eulerAngles = angles;
	}

	void SwingSword (Vector2 direction) {
		sword.transform.position = transform.position + (Vector3)direction + Vector3.one/2;
		sword.gameObject.SetActive (true);
		Vector3 angles = sword.transform.eulerAngles;
		Vector2 velocity = calculateVelocity ();
		if (velocity.x > 0 && velocity.y > 0) angles.z = -45f;
		if (velocity.x < 0 && velocity.y > 0) angles.z = 45f;
		if (velocity.x < 0 && velocity.y < 0) angles.z = 135f;
		if (velocity.x > 0 && velocity.y < 0) angles.z = -135f;
		if (velocity.x == 0 && velocity.y > 0) angles.z = 0f;
		if (velocity.x == 0 && velocity.y < 0) angles.z = 180f;
		if (velocity.x < 0 && velocity.y == 0) angles.z = 90f;
		if (velocity.x > 0 && velocity.y == 0) angles.z = -90f;
		sword.transform.eulerAngles = angles;
	}


	Vector2 calculateVelocity () {
		cubePos = new Vector2 (sCubePosition.x - 0.5f, sCubePosition.y - 0.5f);
		Vector2 velocity = cubePos - new Vector2(transform.position.x, transform.position.y);
		velocity.Normalize ();
		return velocity;
	}

	bool calculateFirstPath (Vector2 pos) {
		Vector2 velocity = calculateVelocity ();
		RaycastHit2D rchx = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2 (velocity.x, 0), Mathf.Abs (transform.position.x - pos.x), terrain);
		RaycastHit2D rchy = Physics2D.Raycast (new Vector2 (pos.x + 0.5f, transform.position.y + 0.5f), new Vector2 (0, velocity.y), Mathf.Abs (transform.position.y - pos.y), terrain);
		
		if (rchx)
			return false;
		if (rchy)
			return false;

		return true;
	}

	bool calculateSecondPath (Vector2 pos) {
		Vector2 velocity = calculateVelocity ();
		RaycastHit2D rchy = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2 (0, velocity.y), Mathf.Abs (transform.position.y - pos.y), terrain);
		RaycastHit2D rchx = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, pos.y + 0.5f), new Vector2 (velocity.x, 0), Mathf.Abs (transform.position.x - pos.x), terrain);
		if (rchy)
			return false;
		if (rchx)
			return false;

		return true;
	}
		
	void MoveXPlayer (Vector2 position) {
		Rigidbody2D rb2D = GetComponent <Rigidbody2D> ();
		Vector2 velocity = calculateVelocity ();
		velocity.Set (Mathf.Sign (velocity.x) * Mathf.Ceil (Mathf.Abs (velocity.x)), Mathf.Sign (velocity.y) * Mathf.Ceil (Mathf.Abs (velocity.y)));
		rb2D.velocity = new Vector2 (velocity.x * speed, 0);
		if(Mathf.Abs (transform.position.x - position.x) < accuracy) {
			rb2D.MovePosition (new Vector2 (position.x, transform.position.y));
			rb2D.velocity = Vector2.zero;
			xposdone = true;
		}
	}

	void MoveYPlayer (Vector2 position) {
		Rigidbody2D rb2D = GetComponent <Rigidbody2D> ();
		Vector2 velocity = calculateVelocity ();
		velocity.Set (Mathf.Sign (velocity.x) * Mathf.Ceil (Mathf.Abs (velocity.x)), Mathf.Sign (velocity.y) * Mathf.Ceil (Mathf.Abs (velocity.y)));
		rb2D.velocity = new Vector2 (0, velocity.y * speed);
		if(Mathf.Abs (transform.position.y - position.y) < accuracy) {
			rb2D.MovePosition (new Vector2 (transform.position.x, position.y));
			rb2D.velocity = Vector2.zero;
			yposdone = true;
		}
	}

	public void loseHealth (int healthLost) {
		health -= healthLost;
		GameObject.Find ("Health Count").GetComponent <Text> ().text = "x " + health.ToString ();
		GameObject partcles = (GameObject)Instantiate (bloodParticles, new Vector3 (transform.position.x + 0.5f, transform.position.y + 0.5f, -5), Quaternion.identity);
		Destroy (partcles, 5);
		if (health == 0) {
			dead = true;
			GameManager.instance.gameOver ();
			Debug.Log ("Player is deaded.");
		}
	}

	public void gainHealth (int healthGained) {
		health += healthGained;
		GameObject.Find ("Health Count").GetComponent <Text> ().text = "x " + health.ToString ();
	}

	public void setIsArrow (bool value) {
		arrowSelected = value;
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

	public Transform selectionCube;
	public GameObject arrow;
	public float arrowSpeed;

	public int health;
	private bool dead = false;

	public GameObject bloodParticles;

	public LayerMask terrain;

	Vector2 cubePos;
	Vector3 sCubePosition;
	public float speed;
	public float accuracy;
	Vector2 velocity;
	bool firstPath;
	bool secondPath;
	bool xposdone = false;
	bool yposdone = false;

	bool android;

	public bool turnDone = false;

	void Start () {
		android = Application.platform == RuntimePlatform.Android;
	}

	void Update () {
		if (!android) {
			if(GameManager.instance.isPlayerTurn && !turnDone && !dead) {
				calculateVelocity ();
				if (Input.GetAxis ("Fire1") > 0) {
					firstPath = false;
					secondPath = false;
					if (calculateFirstPath (cubePos)) {
						firstPath = true;
					} else if (calculateSecondPath (cubePos)) {
						secondPath = true;
					}
					turnDone = true;
				}
				else if (Input.GetAxis ("Fire2") > 0) {
					ShootArrow (new Vector2 (velocity.x * arrowSpeed, velocity.y * arrowSpeed));
					turnDone = true;
				}
			
			}
		} else {
			if(GameManager.instance.isPlayerTurn && !turnDone && !dead) {
				if (Input.touchCount > 0) {
					calculateVelocity ();
					Debug.Log (cubePos);
					if (calculateFirstPath (cubePos)) {
						firstPath = true;
					} else if (calculateSecondPath (cubePos)) {
						secondPath = true;
					}
					turnDone = true;
				} else if (Input.touchCount > 0) {
					ShootArrow (Camera.main.ScreenPointToRay (Input.touches[0].position).origin);
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
		Debug.Log (velocity);
		arrow.GetComponent <Arrow> ().pm = this;

	}

	void calculateVelocity () {
		if (!android) {
			cubePos = new Vector2 (sCubePosition.x - 0.5f, sCubePosition.y - 0.5f);
			velocity = cubePos - new Vector2(transform.position.x, transform.position.y);
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
		} else {
			cubePos = new Vector2 (sCubePosition.x - 0.5f, sCubePosition.y - 0.5f);
			velocity = cubePos - new Vector2(transform.position.x, transform.position.y);
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
	}

	bool calculateFirstPath (Vector2 pos) {
		RaycastHit2D rchx = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2 (velocity.x, 0), Mathf.Abs (transform.position.x - pos.x), terrain);
		RaycastHit2D rchy = Physics2D.Raycast (new Vector2 (pos.x + 0.5f, transform.position.y + 0.5f), new Vector2 (0, velocity.y), Mathf.Abs (transform.position.y - pos.y), terrain);
		
		if (rchx)
			return false;
		if (rchy)
			return false;

		return true;
	}

	bool calculateSecondPath (Vector2 pos) {
		RaycastHit2D rchy = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, transform.position.y + 0.5f), new Vector2 (0, velocity.y), Mathf.Abs (transform.position.y - pos.y), terrain);
		RaycastHit2D rchx = Physics2D.Raycast (new Vector2 (transform.position.x + 0.5f, pos.y + 0.5f), new Vector2 (velocity.x, 0), Mathf.Abs (transform.position.x - pos.x), terrain);
		if (rchy)
			return false;
		if (rchx)
			return false;

		//Debug.Log ("Second Path workes, rchx collider is " + rchx.collider + " and rchy collider is " + rchy.collider + " . ");

		return true;
	}
		
	void MoveXPlayer (Vector2 position) {
		Rigidbody2D rb2D = GetComponent <Rigidbody2D> ();
		rb2D.velocity = new Vector2 (velocity.x * speed, 0);
		if(Mathf.Abs (transform.position.x - position.x) < accuracy) {
			rb2D.MovePosition (new Vector2 (position.x, transform.position.y));
			rb2D.velocity = Vector2.zero;
			xposdone = true;
		}
	}

	void MoveYPlayer (Vector2 position) {
		Rigidbody2D rb2D = GetComponent <Rigidbody2D> ();
		rb2D.velocity = new Vector2 (0, velocity.y * speed);
		if(Mathf.Abs (transform.position.y - position.y) < accuracy) {
			rb2D.MovePosition (new Vector2 (transform.position.x, position.y));
			rb2D.velocity = Vector2.zero;
			yposdone = true;
		}
	}

	public void loseHealth (int healthLost) {
		health -= healthLost;
		GameObject.Find ("Health").GetComponent <Text> ().text = "x " + health.ToString ();
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
		GameObject.Find ("Health").GetComponent <Text> ().text = "x " + health.ToString ();
	}
}

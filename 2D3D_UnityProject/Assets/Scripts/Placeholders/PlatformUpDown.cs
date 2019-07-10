using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUpDown : MonoBehaviour {

	public float uplimit;
	public float downlimit;
	public float speed = 0.1f;
	public float direction = 1f;
	public float delay = 1f;
	public bool isMoving = true;

	// Use this for initialization
	void Start () {

	}

	void FixedUpdate() {
		if (transform.position.y >= uplimit && isMoving) {
			direction = -1f;
			isMoving = false;
			StartCoroutine ("Wait");
		} else if (transform.position.y <= downlimit && isMoving) {
			isMoving = false;
			direction = 1f;
			StartCoroutine ("Wait");
		} else if (isMoving) {
			transform.Translate (Vector3.up * direction * speed);
		}
	}

	IEnumerator Wait() {
		yield return new WaitForSeconds (delay);
		isMoving = true;
		transform.Translate (Vector3.up * direction * speed);
	}
}

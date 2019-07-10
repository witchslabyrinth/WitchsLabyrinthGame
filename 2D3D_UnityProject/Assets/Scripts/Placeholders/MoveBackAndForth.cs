using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackAndForth : MonoBehaviour {

	public float leftlimit = 23f;
	public float rightlimit = 3f;
	public float speed = 0.1f;
	public float direction = 1;

	// Use this for initialization
	void Start () {
		
	}

	void FixedUpdate() {
		if (transform.position.z > rightlimit) {
			direction = -1f;
		} else if (transform.position.z < leftlimit) {
			direction = 1;
		}
			
		transform.Translate (Vector3.forward * direction * speed);
	}
}

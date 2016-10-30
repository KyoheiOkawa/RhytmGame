using UnityEngine;
using System.Collections;

public class PlayerFollowCamera : MonoBehaviour {
	public GameObject target;

	private Vector3 startPos;
	private Vector3 offSet;
	// Use this for initialization
	void Start () {
		startPos = transform.position;
		offSet = startPos - target.transform.position;
	}

	// Update is called once per frame
	void Update () {
		//transform.position = target.transform.position + offSet;
        transform.position = new Vector3(target.transform.position.x + offSet.x, transform.position.y,transform.position.z);
	}
}

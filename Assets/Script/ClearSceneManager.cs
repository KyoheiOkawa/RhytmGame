using UnityEngine;
using System.Collections;

public class ClearSceneManager : MonoBehaviour {
    public float wait = 3.0f;

    private float now = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        now += Time.deltaTime;
        if (now >= wait)
            FadeManager.Instance.LoadLevel("MasterScene/Title",0.5f);
	}
}

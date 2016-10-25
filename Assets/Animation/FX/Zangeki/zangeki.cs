using UnityEngine;
using System.Collections;

public class zangeki : MonoBehaviour {
    private float time;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        time += Time.deltaTime;

        if(time >= 0.7f/2)
        {
            Color c = GetComponent<SpriteRenderer>().color;
            c.a -= 0.1f;
            GetComponent<SpriteRenderer>().color = new Color(c.r, c.g, c.b, c.a);

            if (c.a <= 0)
                Destroy(this.gameObject);
        }
	}
}

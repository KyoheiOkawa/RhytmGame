using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Title : MonoBehaviour {
    public GameObject light;
    public GameObject press;
    public float fadeInterval = 0.5f;

    private float f = 1.0f;
    private bool lightFlg = true;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.anyKeyDown)
        {
            lightFlg = false;
            Destroy(light.gameObject);
        }
        if (lightFlg)
        {
            Color c = light.GetComponent<Image>().color;
            c.a -= f * Time.deltaTime;
            light.GetComponent<Image>().color = c;
            if (c.a <= 0 || c.a >= 1)
                f *= -1;
        }else
        {
            Color c = press.GetComponent<Image>().color;
            c.a -= Time.deltaTime;
            press.GetComponent<Image>().color = c;

            if(c.a <= 0)
            {
                FadeManager.Instance.LoadLevel("main2", 0.5f);
            }
        }
	}
}

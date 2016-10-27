using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Title : MonoBehaviour {
    public GameObject lightef;
    public GameObject press;
    public AudioClip start;
    public float fadeInterval = 0.5f;

    private AudioSource audioSource;
    private float f = 1.0f;
    private bool lightFlg = true;
    private bool change = false;
	// Use this for initialization
	void Start () {
        audioSource = GameObject.Find("Sound").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.anyKeyDown && !change)
        {
            change = true;
            lightFlg = false;
            audioSource.PlayOneShot(start);
            Destroy(lightef.gameObject);
        }
        if (lightFlg)
        {
            Color c = lightef.GetComponent<Image>().color;
            c.a -= f * Time.deltaTime;
            lightef.GetComponent<Image>().color = c;
            if (c.a <= 0 || c.a >= 1)
                f *= -1;
        }else
        {
            Color c = press.GetComponent<Image>().color;
            c.a -= Time.deltaTime;
            press.GetComponent<Image>().color = c;

            if(c.a <= 0 && !audioSource.isPlaying)
            {
                FadeManager.Instance.LoadLevel("main2", fadeInterval);
            }
        }
	}
}

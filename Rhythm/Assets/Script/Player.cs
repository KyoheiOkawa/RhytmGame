using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public AudioClip yha;
    public AudioClip sore;
    public AudioClip ah;
    public GameObject[] Enemy = new GameObject[3];

    private int eneCount = 0;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Move()
    {
        if (transform.position.x + 1 == Enemy[eneCount].transform.position.x)
        {
            GetComponent<AudioSource>().PlayOneShot(ah);
            GetComponent<Animator>().SetTrigger("Damage");
        }
        else
        {
            GetComponent<AudioSource>().PlayOneShot(yha);
            transform.position = new Vector2(transform.position.x + 1, transform.position.y);
        }
    }

    public void Attack()
    {
        if (transform.position.x + 1 == Enemy[eneCount].transform.position.x)
        {
            Destroy(Enemy[eneCount].gameObject);
            if(eneCount != 2)
                eneCount++;
        }
        GetComponent<AudioSource>().PlayOneShot(sore);
    }
}

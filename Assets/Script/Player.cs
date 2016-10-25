using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    public AudioClip yha;
    public AudioClip sore;
    public AudioClip ah;
    public GameObject[] Enemy = new GameObject[3];
    private float minimum;
    private float maximum;
    public float duration = 5.0f;
    private float startTime;

    public GameObject zangeki;

    private int eneCount = 0;
    private bool isMoving = false;
    // Use this for initialization
    void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
	    if(isMoving)
        {
            float t = (Time.time - startTime) / duration;
            transform.position = new Vector3(Mathf.SmoothStep(minimum, maximum, t), transform.position.y, 0);

            if (transform.position.x == maximum)
            {
                isMoving = false;
                GetComponent<Animator>().SetBool("isMove", isMoving);
            }
        }
	}

    public void Move()
    {
        if (transform.position.x + 1 == Enemy[eneCount].transform.position.x)
        {
            GetComponent<AudioSource>().PlayOneShot(ah);
            GetComponent<Animator>().SetTrigger("Damage");
        }
        else if(!isMoving)
        {
            GetComponent<AudioSource>().PlayOneShot(yha);
            //transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            isMoving = true;
            GetComponent<Animator>().SetBool("isMove", isMoving);
            startTime = Time.time;
            minimum = transform.position.x;
            maximum = transform.position.x + 1;
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
        Instantiate(zangeki, new Vector2(transform.position.x+1,transform.position.y-0.6f), Quaternion.identity);
    }
}

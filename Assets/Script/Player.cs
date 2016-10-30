using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
    public AudioClip yha;
    public AudioClip sore;
    public AudioClip ah;
    private GameObject[] L_enemy;//エネミーを全部入れとく配列
    public float jumpPower = 1.0f;
    private float minimum; //移動開始場所
    private float maximum;//移動終了場所
    public float duration = 5.0f;//移動の速さ
    private float startTime;//移動の際に使う変数

    public GameObject zangeki;//斬撃エフェクト

    private int eneCount = 0;//敵の数
    private bool isMoving = false;//今移動中かどうか
    private Vector2 startPos; //プレイヤーのスタート時のポジション
    private float acceleration = 1.0f;//ジャンプするときに使う加速度
    private const int MOVESIZE = 1;
    // Use this for initialization
    void Start () {
        startTime = Time.time;
        startPos = transform.position;

        L_enemy = GameObject.FindGameObjectsWithTag("Enemy");
        int min = 0;
        for(int i = 0; i < L_enemy.Length-1; i++)
        {
            min = i;
            for (int j = i + 1; j < L_enemy.Length; j++)
            {
                if (L_enemy[j].transform.position.x < L_enemy[min].transform.position.x)
                {
                    min = j;
                }
            }
                GameObject tmp = L_enemy[min];
                L_enemy[min] = L_enemy[i];
                L_enemy[i] = tmp;
        }
        //for(int i = 0; i < L_enemy.Length; i++)
        //{
        //    Debug.Log(L_enemy[i].gameObject.name);
        //}
	}
	
	// Update is called once per frame
	void Update () {
        Moving();
		EnemyAttackDamageCheck ();
	}

    public void Moving()
    {
        if (isMoving)
        {
            float t = (Time.time - startTime) / duration;

            acceleration -= 9.8f * Time.deltaTime;
            float y = transform.position.y + jumpPower * acceleration;

            if (y <= startPos.y)
                y = startPos.y;

            transform.position = new Vector3(Mathf.SmoothStep(minimum, maximum, t), y, 0);

            if (transform.position.x == maximum)
            {
                isMoving = false;
                acceleration = 1;
                //GetComponent<Animator>().SetBool("isMove", isMoving);
            }
        }
    }

    public void Move()
    {
        if (eneCount != L_enemy.Length)
        {
            if (transform.position.x + MOVESIZE == L_enemy[eneCount].transform.position.x)
            {
                Damage();
                return;
            }
 
        }
        if (!isMoving)
        {
            GetComponent<AudioSource>().PlayOneShot(yha);
            //transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            isMoving = true;
            //GetComponent<Animator>().SetBool("isMove", isMoving);
            startTime = Time.time;
            minimum = transform.position.x;
            maximum = transform.position.x + MOVESIZE;
        }
    }

    public void Attack()
    {
        if (eneCount != L_enemy.Length)
        {
            if (transform.position.x + MOVESIZE == L_enemy[eneCount].transform.position.x)
            {
                if (L_enemy[eneCount].GetComponent<Enemy>().Damage())
                {
                    if (eneCount != L_enemy.Length)
                        eneCount++;
                }
            }
        }
        GetComponent<AudioSource>().PlayOneShot(sore);
        Instantiate(zangeki, new Vector2(transform.position.x+1,transform.position.y-0.6f), Quaternion.identity);
    }

	private void Damage()
	{
		GetComponent<AudioSource>().PlayOneShot(ah);
		//GetComponent<Animator>().SetTrigger("Damage");
	}

	private void EnemyAttackDamageCheck()
	{
        if (eneCount != L_enemy.Length)
        {
            if (transform.position.x + MOVESIZE == L_enemy[eneCount].transform.position.x)
            {
                if (L_enemy[eneCount].GetComponent<Enemy>().GetIsAtttack())
                {
                    Damage();
                }
            }
        }
	}
}

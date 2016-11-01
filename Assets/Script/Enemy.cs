using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public bool[] attack = new bool[8];
    public GameObject zangekiE;
	public int HP = 1;

	private bool isAttack = false;
    private bool[] attackTmp = new bool[8];
    private int counter = 0;
	// Use this for initialization
	void Start () {
        TraceAttack();
	}
	
	// Update is called once per frame
	void Update () {
        SetCounter();

		if (attackTmp [counter]) {
			isAttack = true;
			attackTmp [counter] = false;
			Instantiate (zangekiE, transform.position, Quaternion.identity);
		} else {
			isAttack = false;
		}

        transform.FindChild("HP").GetComponent<TextMesh>().text = HP.ToString();
		//Move ();
		//Debug.Log (isAttack);
	}

    private void SetCounter()
    {
        float count = GameObject.FindWithTag("Bar").GetComponent<TimingBar>().CalBMSCount() % 9600;

        const int ONECOUNT = 1200;
        for (int i = 0; i < 8; i++)
        {
            if (count >= ONECOUNT * i && count < ONECOUNT * i + ONECOUNT)
            {
                counter = i;

                int tmp = i;
                if (counter == 0)
                    tmp = 7;
                else
                    tmp = i - 1;

                if (attack[tmp])
                    attackTmp[tmp] = true;
            }
        }
    }

    private void TraceAttack()
    {
        for(int i = 0; i < 8; i++)
        {
            attackTmp[i] = attack[i];
        }
    }

	public bool GetIsAtttack()
	{
		return isAttack;
	}

	private bool M_Flg = true;
	private int M_counter = 0;
	private void Move()
	{
		if (M_counter != counter)
			M_Flg = true;
		if (counter % 2 == 0) {
			M_counter = counter;
			if (M_Flg) {
				M_Flg = false;

				transform.position = new Vector3 (transform.position.x - 1.0f, transform.position.y,0);
			}
		}
	}

	public bool Damage()
	{
		HP--;
		if (HP <= 0) {
			Destroy (this.gameObject);
			return true;
		}

		return false;
	}
}

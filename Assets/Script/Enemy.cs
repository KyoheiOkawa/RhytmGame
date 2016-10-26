using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public bool[] attack = new bool[8];
    public GameObject zangekiE;

    private bool[] attackTmp = new bool[8];
    private int counter = 0;
	// Use this for initialization
	void Start () {
        TraceAttack();
	}
	
	// Update is called once per frame
	void Update () {
        SetCounter();

        if(attackTmp[counter])
        {
            attackTmp[counter] = false;
            Instantiate(zangekiE, transform.position, Quaternion.identity);
        }
	}

    private void SetCounter()
    {
        float count = GameObject.FindWithTag("Bar").GetComponent<UITimingBar>().CalBMSCount() % 9600;

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
}

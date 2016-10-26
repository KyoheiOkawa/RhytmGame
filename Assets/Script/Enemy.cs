using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    public bool[] attack = new bool[8];

    private int counter = 0;
    private float value;
    private bool[] counterFlg = new bool[8];
	// Use this for initialization
	void Start () {
        InitCounterFlg();
	}
	
	// Update is called once per frame
	void Update () {
        value = GameObject.FindWithTag("Bar").GetComponent<UITimingBar>().GetSinValue();

        if (value > -0.05f && value < 0.05f)
        {
            if(counter == 7)
            {
                counter = 0;
                InitCounterFlg();
            }
            if (counterFlg[0])
            {
                counterFlg[0] = false;
                counter = 0;
            }
            else if (counterFlg[2])
            {
                counterFlg[2] = false;
                counter = 2;
            }
            else if (counterFlg[4])
            {
                counterFlg[4] = false;
                counter = 4;
            }
            else if (counterFlg[6])
            {
                counterFlg[6] = false;
                counter = 6;
            }
        }
        else if(value > 0.9f && value <= 1.0f)
        {
            if(counterFlg[1])
            {
                counterFlg[1] = false;
                counter = 1;
            }
            else if (counterFlg[5])
            {
                counterFlg[5] = false;
                counter = 5;
            }
        }
        else if(value >= -1.0f && value < -0.9f)
        {
            if(counterFlg[3])
            {
                counterFlg[3] = false;
                counter = 3;
            }
            else if(counterFlg[7])
            {
                counterFlg[7] = false;
                counter = 7;
            }
        }
        Debug.Log(counter);
	}

    void InitCounterFlg()
    {
        for (int i = 0; i < 8; i++)
        {
            counterFlg[i] = true;
        }
    }
}

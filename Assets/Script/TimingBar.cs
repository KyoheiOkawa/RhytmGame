using UnityEngine;
using System.Collections;

public class TimingBar : MonoBehaviour {
    public long BPM = 120;
    public float Side; //中心からの距離（左右対称）
    public GameObject Player;
    
    private AudioSource audioSource;//曲を流すオーディオソース
    private float value; //求めたsinの値を保管しておく
	// Use this for initialization
	void Start () {
        audioSource = GameObject.Find("music").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        DesidePosition();
        int i = Judgment();
        if(i == 1)
        {
            Player.GetComponent<Player>().Move();
            //Debug.Log("Move");
        }
        else if (i == 2)
        {
            Player.GetComponent<Player>().Attack();
            //Debug.Log("Attack");
        }
    }

    //use in Update Func
    private void DesidePosition()
    {
        long count = CalBMSCount(audioSource.time) % BMSConstants.BMS_RESOLUTION; //現在のカウント値から1小節のカウント値（０～９６００）に変換

        value = Mathf.Sin((Mathf.PI / 2400) * CalBMSCount(audioSource.time));
        transform.position = new Vector2(value * Side, transform.position.y);
    }

    private long CalBMSCount(float time)
    {
        return (long)(time * ((float)BPM / 60) * (BMSConstants.BMS_RESOLUTION / 4));

    }

    //表拍、裏拍でタイミングよくスペースキーを押せたか判定する関数
    //戻り値　０失敗　１前進（表）　２攻撃（裏）
    private int Judgment()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(value <= 0.3f && value >= -0.3f)
            {
                return 1;
            }

            if((value >= 0.9f && value <= 1) || (value <= -0.9f && value >= -1))
            {
                return 2;
            }
        }

        return 0;
    }
}

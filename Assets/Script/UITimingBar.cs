using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITimingBar : MonoBehaviour {
	public long BPM = 120;
	public float Side; //中心からの距離（左右対称）
	public GameObject Player;

	private Vector3 startPos;
	private AudioSource audioSource;//曲を流すオーディオソース
	private float value; //求めたsinの値を保管しておく
	// Use this for initialization
	void Start () {
		audioSource = GameObject.Find("music").GetComponent<AudioSource>();
		startPos = transform.position;
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
		value = Mathf.Sin((Mathf.PI / 2400) * CalBMSCount());
		//transform.position = new Vector2(value * Side, transform.position.y);
		gameObject.GetComponent<RectTransform>().position = new Vector3(startPos.x + value * Side, transform.position.y,0);
	}

	public long CalBMSCount()
	{
		return (long)(audioSource.time * ((float)BPM / 60) * (BMSConstants.BMS_RESOLUTION / 4));

	}

	//表拍、裏拍でタイミングよくスペースキーを押せたか判定する関数
	//戻り値　０失敗　１前進（表）　２攻撃（裏）
	private int Judgment()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			if(value <= 0.6f && value >= -0.6f)
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

    public float GetSinValue()
    {
        return value;
    }
}

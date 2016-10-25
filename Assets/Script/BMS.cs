//Copyright © 2016 KyoheiOkawa All Rights Reserved.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


//BMSクラスで使う定数の定義
static class BMSConstants
{
	public const int BMS_RESOLUTION = 9600;//１小節のカウント値
	public const int BMS_MAXBUFFER = 16 * 16;
}

//BMSファイルの基本情報
public struct BMSHEADER
{
	public string mTitle; //曲のタイトル
	public string mArtist;//曲のアーティスト
	public long lBpm;//曲のテンポ
	public long lPlaylevel;//曲の難易度
	public long lRank;//判定のランク
	public long lWavVol;//音量を元の何パーセントにするか
	public float[] fBpmIndex;//テンポインデックス

	public long lEndBar;//終了小節
	public long lendCount;//終了小節の次の小節の始まりのカウント
}

//音符１つの情報
public struct BMSDATA
{
	public long lTime;//音符のカウント値
    public bool bFlag;//ゲーム側で任意に使用できる変数（falseで初期化される）
}

//小節情報
public struct BMSBAR
{
	public float fScale; //この小節の長さの倍率
	public long lTime;	  //この小節の始まりのカウント
	public long lLength; //この小節の長さ（カウント）
}

public class BMS : MonoBehaviour 
{
	public BMSHEADER hBH;
	public BMSBAR[] mBmsBar = new BMSBAR[1000 + 1];
    public List<BMSDATA>[] LMainData = new List<BMSDATA>[10];//配列の要素数は１０から１９のチャンネルに対応

    //コンストラクタ
	public BMS()
	{
		Clear();
	}

    //初期化関数
	public bool Clear()
	{
		hBH.mTitle = null;
		hBH.mArtist = null;
		hBH.lBpm = 120;
		hBH.lPlaylevel = 0;
		hBH.lRank = 0;
		hBH.lWavVol = 1;

        for (int i = 0; i < 10; i++)
            LMainData[i] = new List<BMSDATA>();

		hBH.fBpmIndex = new float[BMSConstants.BMS_MAXBUFFER];
		for (int i = 0; i < BMSConstants.BMS_MAXBUFFER; i++)
		{
			hBH.fBpmIndex [i] = 120;
		}

		for (int i = 0; i < 1000 + 1; i++) 
		{
			mBmsBar [i].fScale = 1.0f;
			mBmsBar [i].lTime = i * BMSConstants.BMS_RESOLUTION;
			mBmsBar [i].lLength = BMSConstants.BMS_RESOLUTION;
		}

		return true;

	}
    //引数FILEはファイルパスを指定
    //以下の関数の引数FILEについての説明は省略
    public void LoadALL(string FILE)
    {
        LoadHeader(FILE);
        LoadBarInfo(FILE);
        BMSMainDataLoad(FILE);
    }

    //BMSファイルの基本情報を読み込む関数
	public void LoadHeader(string FILE)
	{
		string filePass = Path.Combine (Application.streamingAssetsPath, FILE);
		StreamReader file = new StreamReader (filePass);

		while (file.Peek() > -1) 
		{
			string line = file.ReadLine ();
			char[] buf = line.ToCharArray();

			if (buf.Length <= 0)
				continue;
			if (buf [0] != '#')
				continue;

			int cmdNum = GetCommand (line);

			switch (cmdNum) 
			{
			case 0:
				string tmp = CharArrayCopy (7, buf);
				hBH.mTitle = tmp;
				//Debug.Log (hBH.mTitle);
				break;
			case 1:
				tmp = CharArrayCopy(8,buf);
				hBH.mArtist = tmp;
				//Debug.Log(hBH.mArtist);
				break;
			case 2:
				tmp = CharArrayCopy (5, buf);
			    long lb = long.Parse (tmp);
				hBH.lBpm = lb;
				//Debug.Log ("BPM:" + hBH.lBpm);
				break;
			case 3:
				tmp = CharArrayCopy (11, buf);
				long pl = long.Parse (tmp);
				hBH.lPlaylevel = pl;
				//Debug.Log ("PLAYLEVEL:" + hBH.lPlaylevel);
				break;
			case 4:
				tmp = CharArrayCopy (6, buf);
				pl = long.Parse (tmp);
				hBH.lRank = pl;
				//Debug.Log ("RANK:" + hBH.lRank);
				break;
			case 5:
				tmp = CharArrayCopy (5, buf);
				pl = long.Parse (tmp);
				hBH.lWavVol = pl;
				Debug.Log ("WAV:" + hBH.lWavVol);
				break;
			}
		}
		file.Close ();
	}

    //小節情報を読み込む関数
	public void LoadBarInfo(string FILE)
	{
		string filePass = Path.Combine (Application.streamingAssetsPath, FILE);
		StreamReader file = new StreamReader (filePass);

		int measure = 0;
		long maxCount = 0;

		while (file.Peek () > -1) 
		{
			string line = file.ReadLine ();
			char[] buf = line.ToCharArray();

			if (buf.Length <= 0)
				continue;
			if (buf [0] != '#')
				continue;

            if (isDigitChar(1, 3, buf))
            {
                if (GetMeasure(buf) == string.Format("{0:D3}", measure + 1))
                {
                    //Debug.Log (measure + "scale is" +mBmsBar [measure].fScale);
                    //Debug.Log (measure +"start count is" + mBmsBar [measure].lTime);
                    //Debug.Log (measure +"length is" + mBmsBar [measure].lLength);
                    measure++;
                }
                if (GetMeasure(buf) == string.Format("{0:D3}", measure))
                {
                    if (measure == 0)
                    {
                        mBmsBar[measure].lTime = BMSConstants.BMS_RESOLUTION * measure;
                    }
                    else
                    {
                        mBmsBar[measure].lTime = mBmsBar[measure-1].lTime + (long)(mBmsBar[measure-1].fScale * BMSConstants.BMS_RESOLUTION);
                    }

                    if (GetChannel (buf) == "02")
                    {
                        string mag = CharArrayCopy (7, buf);
                        mBmsBar [measure].fScale = float.Parse (mag);
                    }

                    mBmsBar[measure].lLength = (long)(mBmsBar[measure].fScale * BMSConstants.BMS_RESOLUTION);
                }
            }


		}
        hBH.lEndBar = measure;
        hBH.lendCount = mBmsBar[measure].lTime + mBmsBar[measure].lLength;
		file.Close ();
	}

    //音符情報を全部読み込む関数
    public void BMSMainDataLoad(string FILE)
    {
        string filePass = Path.Combine (Application.streamingAssetsPath, FILE);
        StreamReader file = new StreamReader (filePass);

        int measure = 0;

        while (file.Peek() > -1)
        {
            string line = file.ReadLine ();
            char[] buf = line.ToCharArray();

            if (buf.Length <= 0)
                continue;
            if (buf [0] != '#')
                continue;

            if (isDigitChar(1, 3, buf))
            {
                int mes = int.Parse(GetMeasure(buf));
                int cha = int.Parse(GetChannel(buf));

                if (mes == measure + 1)
                {
                    measure++;
                }
                if (measure == mes)
                {
                    if(cha >= 10 && cha <= 19)
                    {
                        string s_data = CharArrayCopy(7, buf);
                        char[] c_data = s_data.ToCharArray();
                        int length = CountArrayLength(c_data);
                        //Debug.Log(length);
                        int oneCount = (int)((BMSConstants.BMS_RESOLUTION * mBmsBar[mes].fScale) / length);

                        for (int i = 0; i < length; i++)
                        {
                            if (c_data[i] == '1')
                            {
                                BMSDATA tmp = new BMSDATA();
                                tmp.lTime = mBmsBar[mes].lTime + i * oneCount;
                                tmp.bFlag = false;

                                LMainData[cha-10].Add(tmp);
                            }
                        }
                    }
                }
            }
        }
        //for (int i = 0; i < LMainData[0].Count; i++)
        //{
        //    BMSDATA tmp = LMainData[0][i];
        //    Debug.Log(tmp.lTime);
        //}
        file.Close();
    }

    //時間（秒）からBMSカウント値を求める関数
    //注意！！　この関数はテンポの途中変更に対応していない
    public long CalBMSCount(float sec)
    {
        return (long)(sec * ((float)hBH.lBpm / 60) * (BMSConstants.BMS_RESOLUTION / 4));
    }

    //BMSファイルの基本情報を読み込む際どの情報か判定する関数
    //戻り値にはcmdのインデックス
	private int GetCommand(string str)
	{
		string[] cmd =
		{
			"TITLE",
			"ARTIST",
			"BPM",
			"PLAYLEVEL",
			"RANK",
			"WAV"
		};

		for(int i = 0; i < 6; i++)
		{
			if(str.Contains(cmd[i]))
			{
				return i;
			}
		}

		bool obj = true;
		char[] ch = str.ToCharArray();
		for(int i = 1; i <= 5; i++)
		{
			if(ch[i] < '0' || ch[i] > '9')
			{
				obj = false;
			}
		}

		if(obj)
		{
			return -1;
		}

		return -2;
	}

	//始まりを指定して改行するまでbufにコピー
	private string CharArrayCopy(int start,char[] origin)
	{
		char[] buf = new char[1024];

		int j = 0;
		for (int i = start; i < origin.Length; i++) {
			buf [j++] = origin [i];
		}

		string str = new string (buf);

		return str;
	}

    //小節番号を取得
	private string GetMeasure(char[] origin)
	{
		char[] ch = new char[3];
		for (int i = 0; i < 3; i++) 
		{
			ch [i] = origin [i + 1];
		}

		string result = new string (ch);
		return result;
	}

    //チャンネル番号を取得
	private string GetChannel(char[] origin)
	{
		char[] ch = new char[2];
		for (int i = 0; i < 2; i++) 
		{
			ch [i] = origin [i + 4];
		}

		string result = new string (ch);
		return result;
	}

    //文字型配列の要素が全て数字かどうか判断する関数
    //ch 判定したい文字列型配列
    //start 判定したい要素の最初のインデックスを指定
    //end startからどこまで判定するかを指定するインデックス
    //戻り値　全部が数字だったらtrueそれ以外false
	private bool isDigitChar(int start,int end,char[] ch)
	{
		for (int i = start; i < end; i++)
		{
			if (!char.IsDigit (ch [i])) 
			{
				return false;
			}
		}

		return true;
	}

    //要素全てが数字である文字列型配列限定の関数
    //引数originには文字列型配列
    //戻り値はoriginは要素数０から何個連続して数字が入ってるか返す
    private int CountArrayLength(char[] origin)
    {
        int result = 0;
        while (char.IsDigit(origin[result]))
        {
            result++;
        }
        return result;
    }
}

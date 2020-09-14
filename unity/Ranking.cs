using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ranking : MonoBehaviour
{

    string url = "http://localhost:3000/rank/";

    // ランク１
    public Text rank1;
    // ランク２
    public Text rank2;
    // ランク３
    public Text rank3;

    void Start()
    {
        StartCoroutine("GetRanking");
    }

    [System.Obsolete]
    IEnumerator GetRanking()
    {
        var www = new WWW(url);
        //wwwの値が返ってくるまで処理を止める。
        yield return www;

        //Jsonデジリアライズ
        List<Rank> deserialized = JsonConvert.DeserializeObject<System.Collections.Generic.List<Rank>>(www.text);

        //Scoreソート処理
        List<Rank> rankingList = new List<Rank>();
        rankingList.Add(new Rank());

        foreach (Rank rank in deserialized)
        {
            int ichi = 0;

            for (int i = 0; i < rankingList.Count; i++)
            {
                if (rankingList[i].score < rank.score) {
                    ichi = i;
                    break;
                }
                ichi = i + 1;
            }
            rankingList.Insert(ichi, rank);
        }

        //表示
        rank1.text = "No1　" + rankingList[0].name + "  " + rankingList[0].score;
        rank2.text = "No2　" + rankingList[1].name + "  " + rankingList[1].score;
        rank3.text = "No3　" + rankingList[2].name + "  " + rankingList[2].score;
    }

    public void Back()
    {
        SceneManager.LoadScene("Ranking");
    }
}

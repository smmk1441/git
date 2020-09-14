using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // スコアを表示するGUIText
    public Text scoreGUIText;

    // ハイスコアを表示するGUIText
    public Text highScoreGUIText;

    // スコア
    private int score;

    // ハイスコア
    private int highScore;

    // PlayerPrefsで保存するためのキー
    private string highScoreKey = "highScore";

    // API呼び出しURL
    string url = "http://localhost:3000/rank/";
    int idcnt;
    private string name = "";

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        // スコアがハイスコアより大きければ
        if (highScore < score)
        {
            highScore = score;
        }

        // スコア・ハイスコアを表示する
        scoreGUIText.text = score.ToString();
        highScoreGUIText.text = "HighScore : " + highScore.ToString();
    }

    // ゲーム開始前の状態に戻す
    private void Initialize()
    {
        // スコアを0に戻す
        score = 0;

        // ハイスコアを取得する。保存されてなければ0を取得する。
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    // ポイントの追加
    public void AddPoint(int point)
    {
        score = score + point;
    }

    // ハイスコアの保存
    public void Save()
    {
        // ハイスコアを保存する
        PlayerPrefs.SetInt(highScoreKey, highScore);
        PlayerPrefs.Save();
    }


    // スコアを登録する
    [Obsolete]
    IEnumerator ScoreRegistByName()
    {

        var www = new WWW(url);
        //wwwの値が返ってくるまで処理を止める。
        yield return www;

        //Jsonデジリアライズ
        List<Rank> deserialized = JsonConvert.DeserializeObject<System.Collections.Generic.List<Rank>>(www.text);

        //IDの最大を取得
        foreach (Rank rankk in deserialized)
        {
            if (this.idcnt < rankk.id)
            {
                Debug.Log(idcnt);

                this.idcnt = rankk.id;
            }
        }


        Rank rank = new Rank();
        rank.name = name;
        rank.score = score;
        rank.id = idcnt + 1;

        var json = JsonConvert.SerializeObject(rank);

        // HEADERはHashtableで記述
        Hashtable header = new Hashtable();
        // jsonでリクエストを送るのへッダ例
        header.Add("Content-Type", "application/json; charset=UTF-8");

        // シリアライズする(LitJson.JsonData→JSONテキスト)
        byte[] postBytes = Encoding.Default.GetBytes(json);

        // 送信開始
        www = new WWW(url, postBytes, header);
        yield return www;

        // 成功
        if (www.error == null)
        {
            Debug.Log("Post Success");
        }
        // 失敗
        else
        {
            Debug.Log("Post Failure");
        }
        // ゲーム開始前の状態に戻す
        Initialize();

        SceneManager.LoadScene("Menu");
    }

    internal void ScoreRegist(string name)
    {
        this.name = name;
        StartCoroutine("ScoreRegistByName");
    }
}

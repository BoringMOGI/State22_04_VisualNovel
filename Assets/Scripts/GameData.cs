using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "GameData", menuName = "DB/GameData")]
public class GameData : ScriptableObject
{
    public Data data;    

    
    public void Load(Data data)
    {
        this.data = data;
    }
    
}

[System.Serializable]
public class Data
{
    public string userName;     // 사용자 지정 이름은 무엇인가?
    public int currnetStory;    // 어떤 스토리를 보고 있었는가?
    public int storyRow;        // 스토리를 몇번째 라인까지 읽고 있었는가?

    // etc
    // 스테이터스.
    public int money;           // 소지금.

    public string GetJson()
    {
        return JsonUtility.ToJson(this);
    }
    public void Load(Data loadData)
    {
        userName = loadData.userName;
        currnetStory = loadData.currnetStory;
        storyRow = loadData.storyRow;

        money = loadData.money;
    }
}
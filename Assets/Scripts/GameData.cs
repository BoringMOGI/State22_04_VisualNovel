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
    public string userName;     // ����� ���� �̸��� �����ΰ�?
    public int currnetStory;    // � ���丮�� ���� �־��°�?
    public int storyRow;        // ���丮�� ���° ���α��� �а� �־��°�?

    // etc
    // �������ͽ�.
    public int money;           // ������.

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
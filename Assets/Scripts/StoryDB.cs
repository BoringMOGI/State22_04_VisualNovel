using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WHO
{
    A,
    B,
    C,
}

[System.Serializable]
public class CharacterInfo
{
    public WHO who;         // 누구인가?
    public int position;    // 캐릭터의 위치.
    public int image;       // 이미지 넘버.
    public int face;        // 표정 넘버.
    public float size;      // 사이즈.
}
[System.Serializable]
public class StoryRow
{
    // 이름과 대사.
    public string name;
    public string text;

    // 백그라운드 이미지.
    public int background;

    // 캐릭터 등장 정보.
    public CharacterInfo[] characterInfos;

    // 이펙트(Effect)
    public string backEffect;
    public float backEffectValue;
    public string eventName;
    public float eventValue;

    // 오디오(Audio)
    public string bgm;
    public string se;
    public string voice;

    public StoryRow(string row)
    {
        string[] values = row.Split(',');

        name = values[0].Trim();
        text = values[1].Trim().Replace('@', ',');

        characterInfos = new CharacterInfo[3];
        for (int i = 0; i < 3; i++)
        {
            int position = ParseInt(values[2 + (i * 4)]);
            int image    = ParseInt(values[3 + (i * 4)]);
            int face     = ParseInt(values[4 + (i * 4)]);
            float size   = ParseFloat(values[5 + (i * 4)]);
            characterInfos[i] = new CharacterInfo()
            {
                who = (WHO)i,
                position = position,
                image = image,
                face = face,
                size = size,
            };
        }

        background = ParseInt(values[14].Trim());

        backEffect = values[15].Trim();
        backEffectValue = ParseFloat(values[16].Trim());

        eventName = values[17].Trim();
        eventValue = ParseFloat(values[18].Trim());

        bgm = values[19].Trim();
        se = values[20].Trim();
        voice = values[21].Trim();
    }

    public CharacterInfo GetCharacter(WHO who)
    {
        return characterInfos[(int)who];
    }

    private static int ParseInt(string str)
    {
        str.Trim();
        if (string.IsNullOrEmpty(str))
            return -100;

        return int.Parse(str);
    }
    private static float ParseFloat(string str)
    {
        str.Trim();
        if (string.IsNullOrEmpty(str))
            return -100f;
        
        return float.Parse(str);
    }
}

public class StoryDB : MonoBehaviour
{
    [SerializeField] TextAsset[] csvTexts;

    // 파일 이름, row데이터 배열.
    Dictionary<string, StoryRow[]> db;

    [SerializeField] StoryRow[] test;

    private void Start()
    {
        db = new Dictionary<string, StoryRow[]>();
        foreach(TextAsset csv in csvTexts)
        {
            string[] csvRows = csv.text.Trim().Split('\n');             // 띄어쓰기로 자른다.

            // 각 열 문자열 데이터를 StoryRow객체로 생성.
            List<StoryRow> list = new List<StoryRow>();
            for (int i = 1; i < csvRows.Length; i++)
                list.Add(new StoryRow(csvRows[i]));

            // 실제 DB에 딕셔너리 형태(이름, 데이터 배열)로 전달.
            db.Add(csv.name, list.ToArray());

            // 테스트를 위한 더미.
            test = list.ToArray();
        }
    }

    public StoryRow[] GetStory(string fileName)
    {
        return db[fileName];

    }
}

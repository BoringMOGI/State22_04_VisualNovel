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
    public WHO who;         // �����ΰ�?
    public int position;    // ĳ������ ��ġ.
    public int image;       // �̹��� �ѹ�.
    public int face;        // ǥ�� �ѹ�.
    public float size;      // ������.
}
[System.Serializable]
public class StoryRow
{
    // �̸��� ���.
    public string name;
    public string text;

    // ��׶��� �̹���.
    public int background;

    // ĳ���� ���� ����.
    public CharacterInfo[] characterInfos;

    // ����Ʈ(Effect)
    public string backEffect;
    public float backEffectValue;
    public string eventName;
    public float eventValue;

    // �����(Audio)
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

    // ���� �̸�, row������ �迭.
    Dictionary<string, StoryRow[]> db;

    [SerializeField] StoryRow[] test;

    private void Start()
    {
        db = new Dictionary<string, StoryRow[]>();
        foreach(TextAsset csv in csvTexts)
        {
            string[] csvRows = csv.text.Trim().Split('\n');             // ����� �ڸ���.

            // �� �� ���ڿ� �����͸� StoryRow��ü�� ����.
            List<StoryRow> list = new List<StoryRow>();
            for (int i = 1; i < csvRows.Length; i++)
                list.Add(new StoryRow(csvRows[i]));

            // ���� DB�� ��ųʸ� ����(�̸�, ������ �迭)�� ����.
            db.Add(csv.name, list.ToArray());

            // �׽�Ʈ�� ���� ����.
            test = list.ToArray();
        }
    }

    public StoryRow[] GetStory(string fileName)
    {
        return db[fileName];

    }
}

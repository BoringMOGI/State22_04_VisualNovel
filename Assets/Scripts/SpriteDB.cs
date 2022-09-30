using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterCG
{
    [SerializeField] WHO who;
    [SerializeField] Sprite[] bodySprites;
    [SerializeField] Sprite[] faceSprites;

    public Sprite GetBody(int index)
    {
        return bodySprites[index];
    }
    public Sprite GetFace(int index)
    {
        return faceSprites[index];
    }
}

public class SpriteDB : MonoBehaviour
{
    public static SpriteDB Instance { get; private set; }

    [SerializeField] CharacterCG[] characterCgs;
    [SerializeField] Sprite[] backgroundSprites;

    Dictionary<int, Sprite> backgounds;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // ��׶��� �̹��� �̸��� �����ε� ��� ���ڰ� �ִ°� �ƴϱ� ������
        // ����(�̸�)�� key������ �������.
        backgounds = new Dictionary<int, Sprite>();
        foreach (Sprite back in backgroundSprites)
            backgounds.Add(int.Parse(back.name.Trim()), back);
    }

    public Sprite GetBodySprite(WHO who, int index)
    {
        return characterCgs[(int)who].GetBody(index);
    }
    public Sprite GetFaceSprite(WHO who, int index)
    {
        return characterCgs[(int)who].GetFace(index);
    }

    public Sprite GetBackground(int index)
    {
        return backgounds[index];
    }
}

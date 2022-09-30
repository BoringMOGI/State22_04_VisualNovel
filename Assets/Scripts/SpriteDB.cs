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
        // 백그라운드 이미지 이름이 숫자인데 모든 숫자가 있는게 아니기 때문에
        // 숫자(이름)을 key값으로 만들었다.
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

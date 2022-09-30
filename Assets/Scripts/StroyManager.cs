using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StroyManager : MonoBehaviour
{
    [Header("Story File Name")]
    [SerializeField] string fileName;

    [Header("Parameter")]
    [SerializeField] StoryDB db;
    [SerializeField] Image backgroundImage;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text talkText;

    [Header("Text")]
    [SerializeField] RectTransform talkWindow;
    [SerializeField] RectTransform nextIcon;

    [Header("Player")]
    [SerializeField] Image bodyImage;
    [SerializeField] Image faceImage;

    [Header("Audio")]
    [SerializeField] AudioSource bgmSouce;
    [SerializeField] AudioSource seSource;
    [SerializeField] AudioSource voiceSouce;

    VoiceGroup voiceGroup;
    SpriteDB spriteDB;

    
    [ContextMenu("Show")]
    public void StoryStart()
    {
        bodyImage.enabled = false;
        faceImage.enabled = false;

        StoryRow[] rows = db.GetStory(fileName);

        voiceGroup = AudioDB.instance.GetVoiceGroup(fileName);
        spriteDB = SpriteDB.Instance;
        
        NextIconSetting(false);
        StartCoroutine(Story(rows));
    }
    
    void CharacterSetting(StoryRow row)
    {
        // B ĳ���� ����.
        CharacterInfo b = row.GetCharacter(WHO.B);
        bodyImage.enabled = (b.image >= 0);
        faceImage.enabled = (b.face >= 0);

        if (b.image >= 0)
            bodyImage.sprite = spriteDB.GetBodySprite(WHO.B, b.image);
        if (b.face >= 0)
            faceImage.sprite = spriteDB.GetFaceSprite(WHO.B, b.face);
    }
    void AudioSetting(StoryRow row)
    {
        // �������
        if (row.bgm.Equals("StopBGM"))
        {
            bgmSouce.Stop();
        }
        else if(!string.IsNullOrEmpty(row.bgm))
        {
            bgmSouce.clip = AudioDB.instance.GetBGM(row.bgm);
            bgmSouce.Play();
        }

        // ȿ����
        if(!string.IsNullOrEmpty(row.se))
        {
            seSource.clip = AudioDB.instance.GetSE(row.se);
            seSource.Play();
        }

        // ����
        voiceSouce.Stop();
        if (!string.IsNullOrEmpty(row.voice))
        {
            voiceSouce.clip = voiceGroup.GetVoice(row.voice);
            voiceSouce.Play();
        }
    }
    void NextIconSetting(bool isOn)
    {
        nextIcon.gameObject.SetActive(isOn);
        if (isOn)
        {
            int charCount = talkText.textInfo.characterCount;                   // ���� ����.
            if (charCount > 0)
            {
                // ������ ������ ���� �߾ӿ� ����Ʈ�� �����.
                TMP_CharacterInfo[] charInfos = talkText.textInfo.characterInfo;            // ���� ���� �迭.
                TMP_CharacterInfo lastChar = charInfos[charCount - 1];

                Vector3 endPoint = talkText.transform.TransformPoint(lastChar.bottomRight); // ���� > ��������Ʈ.
                endPoint.x += talkText.fontSize / 2;                                        // �ʺ� ����.
                endPoint.y += talkText.fontSize / 2;                                        // ���� ����.

                nextIcon.position = endPoint;
            }
            else
            {
                // �ؽ�Ʈ �������� ���� ��ܿ� ����Ʈ�� �����.
                Vector3[] corners = new Vector3[4];
                talkWindow.GetWorldCorners(corners);

                Vector3 endPoint = corners[1];
                endPoint.y -= talkText.fontSize / 2;

                nextIcon.position = endPoint;
            }
        }
    }

    IEnumerator Story(StoryRow[] rows)
    {
        // ���پ� ��ɾ ����.
        foreach(StoryRow row in rows)
        {
            // ��׶��� �̹��� ����.
            if(row.background >= 0)
                backgroundImage.sprite = spriteDB.GetBackground(row.background);

            CharacterSetting(row);      // ĳ���� ���� ����.
            AudioSetting(row);          // ����� ���� ����.

            // ��� �ؽ���..
            nameText.text = row.name;   // �̸� ����.
            yield return StartCoroutine(Textting(row.text));


            // �÷��̾� �Է� ���.
            while (!Input.GetMouseButtonDown(0))
                yield return null;

            // ������ ���� ����.
            yield return null;
        }
    }
    IEnumerator Textting(string text)
    {
        NextIconSetting(false);

        bool isSkip = false;
        Coroutine skip = StartCoroutine(TextSkip(() => { isSkip = true; }));

        StringBuilder builder = new StringBuilder();
        foreach(char c in text)
        {
            if (isSkip)
                break;

            builder.Append(c);
            talkText.text = builder.ToString();
            yield return new WaitForSeconds(0.05f);
        }

        talkText.text = text;       // ��ü �ؽ�Ʈ ����.
        yield return null;          // ������ ���� ����.

        NextIconSetting(true);      // ���� Ŭ�� ��ư on.
        StopCoroutine(skip);        // �ڷ�ƾ �ߴ�.

        yield return null;          // ������ ���� ����.
    }
    IEnumerator TextSkip(System.Action callback)
    {
        while (true)
        {
            if(Input.GetMouseButtonDown(0))
                callback?.Invoke();

            yield return null;
        }
    }
}

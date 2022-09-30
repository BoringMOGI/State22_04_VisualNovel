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
        // B 캐릭터 세팅.
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
        // 배경음악
        if (row.bgm.Equals("StopBGM"))
        {
            bgmSouce.Stop();
        }
        else if(!string.IsNullOrEmpty(row.bgm))
        {
            bgmSouce.clip = AudioDB.instance.GetBGM(row.bgm);
            bgmSouce.Play();
        }

        // 효과음
        if(!string.IsNullOrEmpty(row.se))
        {
            seSource.clip = AudioDB.instance.GetSE(row.se);
            seSource.Play();
        }

        // 음성
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
            int charCount = talkText.textInfo.characterCount;                   // 문자 개수.
            if (charCount > 0)
            {
                // 마지막 문자의 우측 중앙에 포인트를 맞춘다.
                TMP_CharacterInfo[] charInfos = talkText.textInfo.characterInfo;            // 문자 정보 배열.
                TMP_CharacterInfo lastChar = charInfos[charCount - 1];

                Vector3 endPoint = talkText.transform.TransformPoint(lastChar.bottomRight); // 로컬 > 월드포인트.
                endPoint.x += talkText.fontSize / 2;                                        // 너비 차이.
                endPoint.y += talkText.fontSize / 2;                                        // 높이 차이.

                nextIcon.position = endPoint;
            }
            else
            {
                // 텍스트 윈도우의 좌측 상단에 포인트를 맞춘다.
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
        // 한줄씩 명령어를 실행.
        foreach(StoryRow row in rows)
        {
            // 백그라운드 이미지 세팅.
            if(row.background >= 0)
                backgroundImage.sprite = spriteDB.GetBackground(row.background);

            CharacterSetting(row);      // 캐릭터 관련 설정.
            AudioSetting(row);          // 오디오 관련 설정.

            // 대사 텍스팅..
            nameText.text = row.name;   // 이름 세팅.
            yield return StartCoroutine(Textting(row.text));


            // 플레이어 입력 대기.
            while (!Input.GetMouseButtonDown(0))
                yield return null;

            // 프레임 간격 조정.
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

        talkText.text = text;       // 전체 텍스트 대입.
        yield return null;          // 프레임 간격 조정.

        NextIconSetting(true);      // 다음 클릭 버튼 on.
        StopCoroutine(skip);        // 코루틴 중단.

        yield return null;          // 프레임 간격 조정.
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] GameData gameData;     // ���� ���Ǵ� ���� ������.
    [SerializeField] SaveSlot[] slots;      // ������ 8�� �����̴�.

    [Header("Position")]
    [SerializeField] Transform openPivot;
    [SerializeField] Transform closePivot;

    const int MAX_SLOT = 8;
    Coroutine move;
    bool isOpen = false;

    private void Start()
    {
        transform.position = closePivot.position;

        SaveManager.SaveData(new Data() { userName = "�׽�Ʈ������A", currnetStory = 1, storyRow = 4, money = 1000}, 1);
        SaveManager.SaveData(new Data() { userName = "�׽�Ʈ������B", currnetStory = 4, storyRow = 2, money = 0}, 3);
        SaveManager.SaveData(new Data() { userName = "�׽�Ʈ������C", currnetStory = 5, storyRow = 16, money = 9999}, 4);

        Data gd = new Data() { currnetStory = 1, storyRow = 4, money = 1000, userName = "�𺧷���" };
        string json = JsonUtility.ToJson(gd);

        Data gd2 = JsonUtility.FromJson<Data>(json);
    }

    public void OnClickSaveLoadUI()
    {
        // UI ��ư ���� �̺�Ʈ : �ݴ�� �����Ѵ�.
        isOpen = !isOpen;   
        if (isOpen)
            Open();
        else
            Close();
    }

    void Open()
    {
        if (move != null)
            StopCoroutine(move);
        move = StartCoroutine(Move(openPivot));

        // ���� ä���.
        for (int i = 0; i < MAX_SLOT; i++)
        {
            string json = string.Empty;

            // ���̺�Ŵ������Լ� ������ �ҷ�����.
            if (SaveManager.LoadData(i, out json))
            {
                // ���� ������ ����.
                Data loadData = JsonUtility.FromJson<Data>(json);
                slots[i].Setup(i, loadData, OnClickLoad);
            }
            else
                slots[i].Setup(i, null, null);
        }
    }
    void Close()
    {
        if (move != null)
            StopCoroutine(move);

        move = StartCoroutine(Move(closePivot));
    }


    private void OnClickLoad(Data gameData)
    {
        this.gameData.Load(gameData);
    }


    IEnumerator Move(Transform destination)
    {
        float moveSpeed = Screen.width * 1.8f;

        while (Vector3.Distance(transform.position, destination.position) != 0f)
        {
            Vector3 current = transform.position;
            Vector3 end = destination.position;
            transform.position = Vector3.MoveTowards(current, end, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}

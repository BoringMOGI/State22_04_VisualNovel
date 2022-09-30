using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveSlot : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;

    System.Action<Data> onClick;
    Data gameData;

    public void Setup(int slotIndex, Data gameData, System.Action<Data> onClick)
    {
        this.gameData = gameData;
        this.onClick = onClick;

        nameText.text = (gameData == null) ? $"Empty[{slotIndex}]Slot" : gameData.userName;
    }

    public void OnClick()
    {
        // ���� �����Ͱ� �־�� Ŭ���� �����ϴ�.
        if(gameData != null)
            onClick?.Invoke(gameData);
    }
}

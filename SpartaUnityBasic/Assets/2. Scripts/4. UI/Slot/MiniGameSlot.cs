using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameSlot : MonoBehaviour
{
    [SerializeField] private Image minigameIconImage;
    [SerializeField] private TextMeshProUGUI minigameName;
    [SerializeField] private Button minigameSelectButton;

    MiniGameType minigameType = MiniGameType.None;

    void Start()
    {
        minigameSelectButton.onClick.AddListener(() => OnClickSlot());
    }

    public void SetMiniGameSlot(MiniGameData data)
    {
        minigameIconImage.sprite = data.MiniGameIcon;
        minigameName.text = data.MiniGameName;
        minigameType = data.MiniGameType;
    }

    private void OnClickSlot()
    {
        UIMinigamePanel.Instance.SelectMiniGame(minigameType);
    }
}
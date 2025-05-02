using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHUD : MonoBehaviour
{
    public static UIHUD Instance;

    [SerializeField]
    private Sprite[] interfactorSprite;

    [SerializeField]
    private Image interfactorImage;

    private Coroutine swapCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public void StartSwapInterfactorSprite()
    {
        swapCoroutine = StartCoroutine(SwapInterfactorSprite());
    }

    public void StopSwapInterfactorSprite()
    {
        if (swapCoroutine != null)
        {
            StopCoroutine(swapCoroutine);
            swapCoroutine = null;
            interfactorImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator SwapInterfactorSprite()
    {
        int currentIndex = 0;
        interfactorImage.gameObject.SetActive(true);
        while (true)
        {
            interfactorImage.sprite = interfactorSprite[currentIndex];
            currentIndex = (currentIndex + 1) % interfactorSprite.Length;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
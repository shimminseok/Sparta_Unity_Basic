using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    void Start()
    {
    }

    void Update()
    {
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine(LoadSceneAsyncRoutione(sceneName));
    }

    IEnumerator LoadSceneAsyncRoutione(string sceneName)
    {
        //TODO: 로딩 UI 활성화
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        while (!async.isDone)
        {
            if (async.progress >= 0.9f)
            {
                //TODO: 페이드 아웃이 끝났으면
                async.allowSceneActivation = true;
            }

            yield return null;
        }
        //TODO: 로딩 UI 비활성화
    }
}
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance;

    public void LoadSceneAsync(int sceneNumber, Action action)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneNumber);
        StartCoroutine(Loading(operation, action));
    }

    public void LoadSceneAsync(string sceneName, Action action)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        StartCoroutine(Loading(operation, action));
    }

    private IEnumerator Loading(AsyncOperation operation, Action action)
    {
        while(!operation.isDone)
        {
            yield return null;
        }

        action?.Invoke();
    }
}

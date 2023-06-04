using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    public event UnityAction OnLoadProcessStarted;
    public event UnityAction OnLoadProcessFinished;

    public event Func<IEnumerator> OnBeforeLoadStarted;
    public event Func<IEnumerator> OnBeforeLoadFinished;

    [SerializeField] private bool debugLoadingTime;

    private DateTime _lastLoadStartTime;
    private IEnumerator _sceneLoadingCoroutine;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }

        OnLoadProcessFinished += ResetSceneLoadingCoroutine;
    }

    public void LoadScene(int sceneIndex)
    {
        if (_sceneLoadingCoroutine == null)
        {
            _sceneLoadingCoroutine = LoadSceneCoroutine(sceneIndex);
            StartCoroutine(_sceneLoadingCoroutine);
        }
        else
        {
            Debug.LogWarning(
                $"Multiple scenes loading attempt detected! Only one scene may be loaded at a time. (Scene |{sceneIndex}| load request)");
        }
    }

    private void ResetSceneLoadingCoroutine()
    {
        _sceneLoadingCoroutine = null;
    }

    private IEnumerator LoadSceneCoroutine(int sceneIndex)
    {
        OnLoadProcessStarted?.Invoke();
        
        if(OnBeforeLoadStarted != null)
            yield return StartCoroutine(OnBeforeLoadStarted?.Invoke());
        
        StartCountingTime();
        yield return SceneManager.LoadSceneAsync(sceneIndex);
        FinishCountingTime();

        if(OnBeforeLoadFinished != null)
            yield return StartCoroutine(OnBeforeLoadFinished?.Invoke());
        
        OnLoadProcessFinished?.Invoke();
    }

    private void StartCountingTime()
    {
        _lastLoadStartTime = DateTime.Now;
    }

    private void FinishCountingTime()
    {
        if(debugLoadingTime)
            Debug.Log($"Loading finished after > {(DateTime.Now - _lastLoadStartTime).Milliseconds} milliseconds");
    }
}

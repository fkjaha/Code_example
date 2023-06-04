using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevHelper : MonoBehaviour
{
    public static DevHelper Instance;
    
    [SerializeField] private KeyCode sceneReloadKey;
    [SerializeField] private KeyCode sceneLoadKey;
    [SerializeField] private int sceneToLoadIndex;

    [SerializeField] private DevHelperSettings settings;

    private void Awake()
    {
#if !UNITY_EDITOR
        if(settings.destroyOutOfEditor) Destroy(this);
#endif

        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(sceneReloadKey))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(sceneLoadKey))
        {
            SceneManager.LoadScene(sceneToLoadIndex);
        }
    }
}

[Serializable]
public struct DevHelperSettings
{
    public bool destroyOutOfEditor;
}

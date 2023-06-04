using System.Collections;
using EasyTransition;
using UnityEngine;

public class SceneLoadingVisualizer : MonoBehaviour
{
    public static SceneLoadingVisualizer Instance;

    [Header("Setup")]
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private int transitionIndex;
    [SerializeField] private TransitionManagerSettings transitionManagerSettings;
    [SerializeField] private GameObject transitionTemplate;

    [Header("Settings")]
    [SerializeField] private bool inTransitionEnabled;
    [SerializeField] private bool outTransitionEnabled;

    private Transition _transition;

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

        sceneLoader.OnBeforeLoadStarted += PlayTransitionIn;
        sceneLoader.OnBeforeLoadFinished += PlayTransitionOut;
    }

    private IEnumerator PlayTransitionIn()
    {
        if(!inTransitionEnabled) yield break;
        
        yield return StartCoroutine(TransitionCoroutine(transitionManagerSettings.transitions[transitionIndex]));
    }
    
    private IEnumerator PlayTransitionOut()
    {
        if(!outTransitionEnabled) yield break;

        yield return StartCoroutine(TransitionCoroutine(transitionManagerSettings.transitions[transitionIndex], true));
    }
    
    private IEnumerator TransitionCoroutine(TransitionSettings transitionSettings, bool isTransitionOut = false)
    {
        Transition template = Instantiate(transitionTemplate, Vector3.zero, Quaternion.identity).GetComponent<Transition>();
        template.transitionSettings = transitionSettings;
        template.fullSettings = transitionManagerSettings;
        
        float transitionTime = transitionSettings.transitionTime;
        if (transitionSettings.autoAdjustTransitionTime)
            transitionTime /= transitionSettings.transitionSpeed;

        if (_transition != null)
        {
            Destroy(_transition.gameObject);
        }
        _transition = template;
        
        if(isTransitionOut)
            _transition.PlayFadeOutTransition();
        else
            _transition.PlayFadeInTransition();
            
        yield return new WaitForSecondsRealtime(transitionTime);
    }
}

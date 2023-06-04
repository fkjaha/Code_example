using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIStatsVisualizer : MonoBehaviour
{
    public event UnityAction OnVisualizationTargetChanged;
    
    [SerializeField] private string gameInputName;
    [SerializeField] private AIMindBase currentBase;
    [Space(20f)] 
    [SerializeField] private List<AIStatVisualizer> visualizers;

    private IAIMindBaseProvider _currentProvider;
    
    private void Start()
    {
        SceneInputDetector.Instance.AddSceneInputListener<IAIMindBaseProvider>(gameInputName, ChangeVisualizationTarget);
    }

    private void ChangeVisualizationTarget(IAIMindBaseProvider target)
    {
        _currentProvider = target;
        AIMindBase targetMindBase = _currentProvider.GetMind();
        
        if (currentBase != null)
            currentBase.OnDestroyEvent -= ResetCurrentBase;

        currentBase = currentBase == targetMindBase ? null : targetMindBase;
        
        if (currentBase != null)
            currentBase.OnDestroyEvent += ResetCurrentBase;
        
        OnTargetChanged();
    }

    private void ResetCurrentBase()
    {
        ChangeVisualizationTarget(_currentProvider);
    }

    private void OnTargetChanged()
    {
        foreach (AIStatVisualizer statVisualizer in visualizers)
        {
            statVisualizer.ChangeVisualizationTarget(currentBase);
        }
        OnVisualizationTargetChanged?.Invoke();
    }
}

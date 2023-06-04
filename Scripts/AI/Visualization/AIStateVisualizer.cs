using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIStateVisualizer : AIStatVisualizer
{
    [SerializeField] private Transform moveTarget;
    [SerializeField] private Image image;
    [SerializeField] private int idleStateIndex;
    [SerializeField] private int suspectStateIndex;
    [SerializeField] private int concernedStateIndex;
    [SerializeField] private List<GameObject> statesGameObjects;

    private int _currentIndex;

    private void Update()
    {
        if(currentTarget == null) return;

        moveTarget.position = _targetTransform.position;
        _currentIndex = currentTarget.IsConcerned ? concernedStateIndex : (currentTarget.IsSuspected ? suspectStateIndex : idleStateIndex);
        
        for (int i = 0; i < statesGameObjects.Count; i++)
        {
            if(statesGameObjects[i] != null)
                statesGameObjects[i].SetActive(i == _currentIndex);
        }
    }
}

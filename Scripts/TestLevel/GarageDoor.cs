using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GarageDoor : MonoBehaviour
{
    [SerializeField] private float moveTime;
    [SerializeField] private List<EventTask> requiredTasks;
    [SerializeField] private Transform doorTransform;
    [SerializeField] private Vector3 closedLocalPosition;
    [SerializeField] private Vector3 openedLocalPosition;

    private int _tasksCompleted;

    private void Start()
    {
        foreach (EventTask requiredTask in requiredTasks)
        {
            requiredTask.OnComplete += () =>
            {
                _tasksCompleted++;
                UpdateState();
            };
        }
        UpdateState();
    }

    private void UpdateState()
    {
        doorTransform.DOLocalMove(_tasksCompleted >= requiredTasks.Count ? openedLocalPosition : closedLocalPosition, moveTime);
    }
}

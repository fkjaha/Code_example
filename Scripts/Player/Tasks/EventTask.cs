using UnityEngine;
using UnityEngine.Events;

public class EventTask : PlayerTask
{
    public event UnityAction OnComplete;

    [SerializeField] private UnityEvent onComplete;

    public override void CompleteTask()
    {
        base.CompleteTask();
        OnComplete?.Invoke();
        onComplete?.Invoke();
    }
}

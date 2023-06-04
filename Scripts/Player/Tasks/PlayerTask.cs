using UnityEngine;

public class PlayerTask : InteractiveObject
{
    public Vector3 GetOriginPosition => taskOriginTransform.position;
    public float GetPerformRadius => performRadius;
    public TaskInfo GetTaskInfo => taskInfo;
    
    [Space(20f)]
    [SerializeField] private TaskInfo taskInfo;
    [SerializeField] private Transform taskOriginTransform;
    [SerializeField] private float performRadius;
    [Space(20f)] 
    [SerializeField] private bool disableOnComplete;

    public override void Interact()
    {
        PlayerTaskPerformer.Instance.TryPerform(this);
    }

    public virtual void CompleteTask()
    {
        if(disableOnComplete)
            DisableInteractions();
    }

    private void OnDrawGizmosSelected()
    {
        #if UNITY_EDITOR
        if(taskOriginTransform == null) return;
        
        UnityEditor.Handles.color = Color.green;
        UnityEditor.Handles.DrawWireDisc(taskOriginTransform.position, Vector3.up, performRadius);
        #endif
    }
}

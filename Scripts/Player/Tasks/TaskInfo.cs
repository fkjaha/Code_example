using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/New Task Info", fileName = "TaskInfo_")]
public class TaskInfo : ScriptableObject
{
    public string GetName => taskName;
    public float GetPerformTime => performTime;

    [SerializeField] private string taskName;
    [SerializeField] private float performTime;
}

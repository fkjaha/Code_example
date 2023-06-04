using System.Collections.Generic;
using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    public List<PathAction> GetPathActions => pathActions;
    public bool LoopActions => loopActions;
    public int GetRepeatCount => repeatCount;

    [SerializeField] private List<PathAction> pathActions;
    [SerializeField] private bool loopActions;
    [SerializeField] private int repeatCount = 1;
}
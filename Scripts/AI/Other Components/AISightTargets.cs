using System.Collections.Generic;
using UnityEngine;

public class AISightTargets : MonoBehaviour
{
    public static AISightTargets Instance;

    public Transform GetMainTarget => mainTarget;
    public List<AdditionalTarget> GetAdditionalTargets => additionalTargets;

    [SerializeField] private Transform mainTarget;
    [SerializeField] private List<AdditionalTarget> additionalTargets = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddAdditionalTarget(AdditionalTarget target)
    {
        additionalTargets.Add(target);
    }

    public void RemoveAdditionalTarget(AdditionalTarget target)
    {
        additionalTargets.Remove(target);
    }
}

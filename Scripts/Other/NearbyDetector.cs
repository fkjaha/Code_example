using System.Collections.Generic;
using UnityEngine;

public class NearbyDetector : MonoBehaviour
{
    [SerializeField] private int maxDetectTargetsCount;
    [SerializeField] private LayerMask layerMask;

    public List<T> GetNearbyInRadius<T>(Vector3 origin, float radius, float allowedHeightDelta, T exclude = null) where T: class
    {
        Collider[] rawResults = new Collider[maxDetectTargetsCount];
        Physics.OverlapBoxNonAlloc(origin, new Vector3(radius, allowedHeightDelta, radius), rawResults,
            Quaternion.identity, layerMask);
        List<T> filteredResults = new();

        foreach (var rawResult in rawResults)
        {
            if (rawResult == null) continue;
            if(!rawResult.gameObject.TryGetComponent(out T component) || component == exclude) continue;
            if(Vector2.Distance(rawResult.transform.position.Vector3XZToVector2(), origin.Vector3XZToVector2()) > radius) continue;
            filteredResults.Add(component);
        }

        // Debug.Log(filteredResults.Count + " | " + typeof(T) + " | " + radius);
        
        return filteredResults;
    }
}

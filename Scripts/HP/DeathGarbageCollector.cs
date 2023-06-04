using System.Collections.Generic;
using UnityEngine;

public class DeathGarbageCollector : MonoBehaviour
{
    [SerializeField] private HPContainer hpContainer;  
    [SerializeField] private List<GameObject> gameObjectsToDestroyOnDeath;
    [SerializeField] private List<Component> componentsToDestroyOnDeath;

    private void Awake()
    {
        hpContainer.OnDeath += DestroyGarbage;
    }

    private void DestroyGarbage()
    {
        foreach (GameObject destroyObject in gameObjectsToDestroyOnDeath)
        {
            Destroy(destroyObject);
        }
        
        foreach (Component destroyComponent in componentsToDestroyOnDeath)
        {
            Destroy(destroyComponent);
        }
    }
}

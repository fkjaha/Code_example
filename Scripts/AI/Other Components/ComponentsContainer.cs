using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentsContainer : MonoBehaviour
{
    private List<IComponent> components;

    public bool TryGetContainerComponent<T>(out T result) where T: Object
    {
        result = null;
        foreach (IComponent component in components)
        {
            if (component.GetComponent<T>())
            {
                result = component as T;
                return true;
            }
        }

        return false;
    }
}

public interface IComponent
{
    public T GetComponent<T>();
}

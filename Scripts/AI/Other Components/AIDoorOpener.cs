using System;
using UnityEngine;

public class AIDoorOpener : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Door door))
        {
            if (!door.IsOpen)
            {
                door.Interact(gameObject);
            }
        }
    }

    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.gameObject.TryGetComponent(out Door door))
    //     {
    //         if (door.IsOpen)
    //         {
    //             door.Interact(gameObject);
    //         }
    //     }
    // }
}

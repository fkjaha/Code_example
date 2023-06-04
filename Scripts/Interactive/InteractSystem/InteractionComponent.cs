using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionComponent : MonoBehaviour
{
    public abstract void CallInteraction(InteractiveObject interactiveObject);
}

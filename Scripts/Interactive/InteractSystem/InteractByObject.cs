using UnityEngine;

public class InteractByObject : MonoBehaviour
{
    public string GetInteractionTag => interactionTag;

    [SerializeField] private string interactionTag = "";
}

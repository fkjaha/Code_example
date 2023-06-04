using UnityEngine;

public class AIColliderBase : MonoBehaviour, IAIMindBaseProvider
{
    [Header("Base")]
    [SerializeField] private AIMindBase aiMind;

    public AIMindBase GetMind()
    {
        return aiMind;
    }
}

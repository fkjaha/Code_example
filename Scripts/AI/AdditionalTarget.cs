using UnityEngine;

public class AdditionalTarget : MonoBehaviour
{
    private void Start()
    {
        AISightTargets.Instance.AddAdditionalTarget(this);
    }

    public void GetNoticed()
    {
        AISightTargets.Instance.RemoveAdditionalTarget(this);
        Destroy(this);
    }
}

using UnityEngine;
using UnityEngine.Events;

public class Ear : MonoBehaviour
{
    public UnityAction<Vector3> OnHear;
    public UnityAction<Vector3> OnHearAndAlerted;

    public void GetSound(Vector3 position, bool isAlert = false)
    {
        if(!isAlert)
            OnHear?.Invoke(position);
        else 
            OnHearAndAlerted?.Invoke(position);
    }
}

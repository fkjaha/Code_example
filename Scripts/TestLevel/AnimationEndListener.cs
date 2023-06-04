using UnityEngine;
using UnityEngine.Events;

public class AnimationEndListener : MonoBehaviour
{
    public event UnityAction OnAnimationEnded;
    
    public void AnimationEnded()
    {
        OnAnimationEnded?.Invoke();
    }
}

using UnityEngine;

public class AnimationSetter : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationName;

    private void Start()
    {
        animator.Play(animationName);
    }
}

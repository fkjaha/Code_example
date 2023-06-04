using UnityEngine;

public class TestAnimations : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationName;
    [SerializeField] private KeyCode keyToActivate;

    private void Update()
    {
        if (Input.GetKey(keyToActivate))
        {
            animator.Play(animationName, default, 0);
        }
    }
}

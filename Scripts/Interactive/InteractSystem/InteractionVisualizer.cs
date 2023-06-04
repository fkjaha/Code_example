using System;
using UnityEngine;
using UnityEngine.UI;

public class InteractionVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject interactionPossibilityDisplayGameObject;
    [SerializeField] private Image holdingIndicatorImage;

    [SerializeField] private Camera raycastCamera;
    [SerializeField] private LayerMask raycastMask;

    private void Start()
    {
        UpdateHoldingIndicatorImage(false);
    }

    void FixedUpdate()
    {
        UpdateInteractionPossibilityIcon();
    }


    private void UpdateInteractionPossibilityIcon()
    {
        interactionPossibilityDisplayGameObject.SetActive(
            Physics.Raycast(raycastCamera.ScreenPointToRay(InputDetector.Instance.GetMousePosition), out RaycastHit hit,
                Mathf.Infinity, raycastMask) && hit.collider.gameObject.TryGetComponent(out InteractiveObject _));
    }

    public void UpdateHoldingIndicatorImage(bool isHolding)
    {
        holdingIndicatorImage.gameObject.SetActive(isHolding);
    }

    public void UpdateHoldingIndicatorImage(float fillAmount)
    {
        holdingIndicatorImage.fillAmount = fillAmount;
    }
}

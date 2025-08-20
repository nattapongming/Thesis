using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnchantmentSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Enchantment enchantment;
    public Image icon;

    private CanvasGroup canvasGroup;
    private Transform originalParent;
    private Vector2 originalPosition;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        transform.SetParent(transform.root); // Move to top layer
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        canvasGroup.blocksRaycasts = true;
    }
}


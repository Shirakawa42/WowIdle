using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BehaviorSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public Image border;
    private Behavior behavior;

    public void OnPointerEnter(PointerEventData eventData)
    {
        behavior?.OnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        behavior?.OnPointerExit();
    }

    public void SetBehavior(Behavior behavior, Vector3 position)
    {
        this.behavior = behavior;
        background.sprite = Resources.Load<Sprite>("Textures/BehaviorIcons/" + behavior.Icon);
        border.color = ColorUtils.GetColorFromHex(behavior.Color);
        behavior.SetTooltipPosition(position);
    }
}

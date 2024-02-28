using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image background;
    public Image border;
    private Ability ability;

    public void OnPointerEnter(PointerEventData eventData)
    {
        ability?.OnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ability?.OnPointerExit();
    }

    public void SetAbility(Ability ability, Vector3 position)
    {
        this.ability = ability;
        background.sprite = Resources.Load<Sprite>("Textures/AbilityIcons/" + ability.Icon);
        border.color = ColorUtils.GetColorFromHex(ability.Color);
        ability.SetTooltipPosition(position);
    }
}


using System.Collections.Generic;
using UnityEngine;

public abstract class Hoverable
{
    public abstract Vector3 TooltipPosition { get; set; }
    public abstract string Color { get; set; }
    public bool IsHovered { get; set; }

    public abstract List<TooltipValue> GetTooltipValues();
    public void OnPointerEnter()
    {
        Globals.itemTooltipManager.ShowTooltip(GetTooltipValues(), Color, TooltipPosition);
        IsHovered = true;
    }

    public void OnPointerExit()
    {
        Globals.itemTooltipManager.HideTooltip();
        IsHovered = false;
    }

    public void OnRemove()
    {
        if (IsHovered)
        {
            OnPointerExit();
            IsHovered = false;
        }
    }
}
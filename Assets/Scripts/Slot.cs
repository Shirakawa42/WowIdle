using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Slot : MonoBehaviour, IDropHandler, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject draggableItem;
    public List<GameObject> extras = new();
    public SlotType slotType;
    public bool equippedSlot = true;

    private Slotable slotable = null;

    public Slotable SetSlotable(Slotable slotable)
    {
        Slotable oldSlotable = this.slotable;

        if (equippedSlot)
        {
            this.slotable?.OnUnequip(this);
            slotable?.OnEquip(Globals.selectedHero, this);
        }
        this.slotable = slotable;
        draggableItem.SetActive(false);
        if (slotable != null)
        {
            draggableItem.GetComponent<DraggedObject>().icon.sprite = slotable.Icon;
            draggableItem.GetComponent<DraggedObject>().border.color = slotable.Color;
            draggableItem.GetComponent<DraggedObject>().slotable = slotable;
            draggableItem.SetActive(true);
        }
        return oldSlotable;
    }

    public void EnableExtras(bool enable)
    {
        foreach (GameObject extra in extras)
        {
            extra.SetActive(enable);
        }
    }

    public Slotable GetSlotable()
    {
        return slotable;
    }

    public bool IsEmpty()
    {
        return slotable == null;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (slotable != null && slotType != SlotType.Enemy)
        {
            draggableItem.transform.SetParent(transform.root);
            draggableItem.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (slotable != null && slotType != SlotType.Enemy)
        {
            draggableItem.transform.position = eventData.position;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        Slot droppedSlot = droppedItem.GetComponent<Slot>();

        if (droppedSlot.slotable == null || !IsSlotCompatible(droppedSlot.slotable.SlotType))
            return;

        Slotable newSlotable = droppedSlot.SetSlotable(slotable);
        SetSlotable(newSlotable);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggableItem.transform.position = transform.position;
        draggableItem.transform.SetParent(transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotable?.OnPointerEnter(transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slotable?.OnPointerExit();
    }

    public bool IsSlotCompatible(SlotType slot)
    {
        if (slotType == slot)
            return true;

        if (slotType == SlotType.AnyGear && !IsUnitSlot(slot))
            return true;

        return false;
    }

    public bool IsUnitSlot(SlotType slotType)
    {
        return slotType == SlotType.Hero || slotType == SlotType.Enemy;
    }
}

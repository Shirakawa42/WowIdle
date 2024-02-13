using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine.AI;

public class Slot : MonoBehaviour, IDropHandler, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject draggableItem;
    public List<GameObject> extras = new();
    public SlotType slotType;
    public bool equippedSlot = true;
    public int id;
    public GameObject selectedBorder;

    private Slotable slotable = null;

    public Slotable SetSlotable(Slotable newslotable, bool dontUnequip = false, bool dontEquip = false)
    {
        Slotable oldSlotable = slotable;

        newslotable?.SetCurrentSlot(this);
        if (equippedSlot)
        {
            if (!dontUnequip)
                slotable?.OnUnequip();
            if (!dontEquip)
                newslotable?.OnEquip();
        }
        slotable = newslotable;
        draggableItem.SetActive(false);
        if (newslotable != null)
        {
            draggableItem.GetComponent<DraggedObject>().icon.sprite = newslotable.Icon;
            draggableItem.GetComponent<DraggedObject>().border.color = newslotable.Color;
            draggableItem.GetComponent<DraggedObject>().slotable = newslotable;
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

    public void SetSelected()
    {
        if (Globals.selectedSlot == this)
            return;
        if (Globals.selectedSlot != null)
            Globals.selectedSlot.selectedBorder.SetActive(false);
        
        Globals.selectedSlot = this;
        selectedBorder.SetActive(true);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        Slot droppedSlot = droppedItem.GetComponent<Slot>();

        if (droppedSlot.slotable == null || !IsSlotCompatible(droppedSlot.slotable.SlotType))
            return;

        if (Globals.selectedSlot == droppedSlot) {
            SetSelected();
        }

        Slotable newSlotable = droppedSlot.SetSlotable(slotable);
        SetSlotable(newSlotable, true);
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

    private readonly Stopwatch clickTimer = new();
    private Vector2 clickPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (slotable == null || slotType != SlotType.Hero)
            return;

        clickTimer.Stop();
        clickTimer.Reset();
        clickTimer.Start();
        clickPosition = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (slotable == null || slotType != SlotType.Hero)
            return;

        clickTimer.Stop();
        if (clickTimer.ElapsedMilliseconds < 350 && (clickPosition - eventData.position).magnitude < 10)
        {
            SetSelected();
            slotable.OnClick();
        }
        clickTimer.Reset();
    }

    public bool IsSlotCompatible(SlotType slot)
    {
        if (slotType == slot)
            return true;

        if (slotType == SlotType.AnyGear && !IsUnitSlot(slot))
            return true;

        if ((slotType == SlotType.MainHand || slotType == SlotType.OffHand)
            && (slot == SlotType.MainHand || slot == SlotType.OffHand))
            return true;

        return false;
    }

    public bool IsUnitSlot(SlotType slotType)
    {
        return slotType == SlotType.Hero || slotType == SlotType.Enemy;
    }
}

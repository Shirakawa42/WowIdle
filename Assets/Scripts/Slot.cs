using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public GameObject draggableItem;
    public List<SlotExtra> extras = new();
    public List<Image> coloredImages = new();
    public SlotType slotType;
    public bool equippedSlot = true;
    public int id;
    public GameObject selectedBorder;
    public TMP_Text levelText = null;
    public GameObject[] disabledOnEnemy = new GameObject[0];

    private Slotable slotable = null;
    private bool dragging = false;


    void Start()
    {
        if (slotType == SlotType.Enemy)
        {
            foreach (GameObject obj in disabledOnEnemy)
                obj.SetActive(false);
        }
    }

    public Slotable SetSlotable(Slotable newslotable, bool dontEquip = false)
    {
        Slotable oldSlotable = slotable;
        if (newslotable == null)
        {
            EmptySlot();
            return oldSlotable;
        }

        newslotable.SetCurrentSlot(this);
        EnableExtras(equippedSlot);

        slotable = newslotable;
        if (equippedSlot && !dontEquip)
            newslotable.OnEquip();

        draggableItem.SetActive(false);
        if (newslotable != null)
        {
            UpdateLevel();
            draggableItem.GetComponent<DraggedObject>().icon.sprite = newslotable.Icon;
            draggableItem.GetComponent<DraggedObject>().slotable = newslotable;
            draggableItem.SetActive(true);
            foreach (Image image in coloredImages)
                image.color = ColorUtils.GetColorFromHex(newslotable.Color);
            slotable.UpdateBars();
        }
        return oldSlotable;
    }

    public void EmptySlot(bool dontUnequip = false)
    {
        if (equippedSlot && !dontUnequip)
            slotable?.OnUnequip();
        slotable = null;
        draggableItem.SetActive(false);
        EnableExtras(false);
    }

    public Vector2 GetTopLeftCorner()
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        return new Vector2(rectTransform.position.x - rectTransform.rect.width / 2 * Globals.gameCanvas.scaleFactor, rectTransform.position.y + rectTransform.rect.height / 2 * Globals.gameCanvas.scaleFactor);
    }

    public void EnableExtras(bool enable)
    {
        foreach (SlotExtra extra in extras)
        {
            if (extra.heroOnly && slotType != SlotType.Hero)
                continue;
            extra.gameObject.SetActive(enable);
        }
    }

    public void UpdateLevel()
    {
        levelText.text = slotable.Level.ToString();
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
        dragging = true;
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

        if (droppedSlot.slotable == null || !IsSlotCompatible(droppedSlot.slotable.SlotType) || droppedSlot == this)
            return;

        if (Globals.selectedSlot == droppedSlot)
        {
            SetSelected();
        }

        Slotable newSlotable = droppedSlot.slotable;
        Slotable oldSlotable = slotable;

        bool dontUnequip = false;
        bool dontEquip = false;
        if (equippedSlot && droppedSlot.equippedSlot && slotType == SlotType.Hero)
            dontUnequip = dontEquip = true;

        droppedSlot.EmptySlot(dontUnequip);
        EmptySlot(dontUnequip);

        droppedSlot.SetSlotable(oldSlotable, dontEquip);
        SetSlotable(newSlotable, dontEquip);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggableItem.transform.position = transform.position;
        draggableItem.transform.SetParent(transform);
        dragging = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        slotable?.OnPointerEnter();
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

        if ((slotType == SlotType.OneHand || slotType == SlotType.TwoHands)
            && (slot == SlotType.OneHand || slot == SlotType.TwoHands))
            return true;

        return false;
    }

    public bool IsUnitSlot(SlotType slotType)
    {
        return slotType == SlotType.Hero || slotType == SlotType.Enemy;
    }

    public void PlayAttackAnimation(Vector3 targetPosition)
    {
        if (!dragging)
            StartCoroutine(MoveTo(targetPosition));
    }

    private System.Collections.IEnumerator MoveTo(Vector3 targetPosition)
    {
        float time = 0;
        Vector3 startPosition = draggableItem.transform.position;
        targetPosition = startPosition + (targetPosition - startPosition).normalized * 10f;
        while (time < 0.2f)
        {
            if (dragging)
                break;
            time += Time.deltaTime;
            draggableItem.transform.position = Vector3.Lerp(startPosition, targetPosition, time / 0.1f);
            yield return null;
        }
        time = 0;
        while (time < 0.2f)
        {
            if (dragging)
                break;
            time += Time.deltaTime;
            draggableItem.transform.position = Vector3.Lerp(targetPosition, startPosition, time / 0.1f);
            yield return null;
        }
        draggableItem.transform.position = transform.position;
    }
}

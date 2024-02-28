using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Stopwatch = System.Diagnostics.Stopwatch;
using TMPro;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler, IEndDragHandler, IDragHandler, IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
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
    public GameObject buffParent;
    public GameObject debuffParent;
    public GameObject abilityParent;
    public GameObject behaviorSlotPrefab;
    public GameObject abilitySlotPrefab;

    private Slotable slotable = null;
    private bool dragging = false;


    void Awake()
    {
        if (slotType == SlotType.Enemy)
        {
            foreach (GameObject obj in disabledOnEnemy)
                obj.SetActive(false);
        }
        if (IsUnitSlot(slotType) && equippedSlot)
        {
            for (int i = 0; i < 16; i++)
            {
                GameObject buff = Instantiate(behaviorSlotPrefab, buffParent.transform);
                GameObject debuff = Instantiate(behaviorSlotPrefab, debuffParent.transform);
                buff.SetActive(false);
                debuff.SetActive(false);
            }
            for (int i = 0; i < 8; i++)
            {
                GameObject ability = Instantiate(abilitySlotPrefab, abilityParent.transform);
                ability.SetActive(false);
            }
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
        if (eventData.pointerEnter == gameObject)
            slotable?.OnPointerEnter();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
            slotable?.OnPointerEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerEnter == gameObject)
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

    private void RefreshBehaviors()
    {
        int buffCount = 0;
        int debuffCount = 0;
        foreach (Behavior behavior in (slotable as Unit).behaviors)
        {
            if (behavior.IsBuff)
            {
                Transform child = buffParent.transform.GetChild(buffCount);
                child.gameObject.SetActive(true);
                child.localPosition = new Vector3(0, -buffCount * 25 - 10, 0);
                child.GetComponent<BehaviorSlot>().SetBehavior(behavior, child.position);
                buffCount++;
            }
            else if (!behavior.IsBuff)
            {
                Transform child = debuffParent.transform.GetChild(debuffCount);
                child.gameObject.SetActive(true);
                child.localPosition = new Vector3(0, -debuffCount * 25 - 10, 0);
                child.GetComponent<BehaviorSlot>().SetBehavior(behavior, child.position);
                debuffCount++;
            }
        }
        for (int i = buffCount; i < 16; i++)
            buffParent.transform.GetChild(i).gameObject.SetActive(false);
        for (int i = debuffCount; i < 16; i++)
            debuffParent.transform.GetChild(i).gameObject.SetActive(false);
    }

    private void RefreshAbilities()
    {
        int abilityCount = 0;
        int totalAbilities = (slotable as Unit).abilities.Count;
        foreach (Ability ability in (slotable as Unit).abilities)
        {
            Transform child = abilityParent.transform.GetChild(abilityCount);
            child.gameObject.SetActive(true);
            child.localPosition = new Vector3(-25 * (totalAbilities - 1) / 2 + 25 * abilityCount, 0, 0);
            child.GetComponent<AbilitySlot>().SetAbility(ability, child.position);
            abilityCount++;
        }
        for (int i = abilityCount; i < 8; i++)
            abilityParent.transform.GetChild(i).gameObject.SetActive(false);
    }

    public void Refresh()
    {
        RefreshBehaviors();
        RefreshAbilities();
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

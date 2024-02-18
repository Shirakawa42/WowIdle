using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourcesManager : MonoBehaviour
{
    public TextMeshProUGUI resourcesText;

    private int money = 0;

    void Awake()
    {
        Globals.resourcesManager = this;
        UpdateText();
    }

    private void UpdateText()
    {
        int copper = money % 100;
        int silver = Mathf.FloorToInt(money / 100) % 100;
        int gold = Mathf.FloorToInt(money / 10000) % 1000;
        int platinum = Mathf.FloorToInt(money / 10000000);

        string text = "";
        if (money > 10000000)
            text += $"<color=#E5E4E2>{platinum}p</color> ";
        if (money > 10000)
            text += $"<color=#FFD700>{gold}g</color> ";
        if (money > 100)
            text += $"<color=#C0C0C0>{silver}s</color> ";
        text += $"<color=#B87333>{copper}c</color>";
        resourcesText.SetText(text);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("money: " + money);
        UpdateText();
    }
}
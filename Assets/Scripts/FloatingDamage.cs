using System.Collections;
using TMPro;
using UnityEngine;

//melee damages are white
//ability damages are yellow
//misses are white and write "miss"
//melee ability and misses move slowly up and fade out in 2 seconds
//crits are bigger, vibrate and does not move up and fade out in 3 second

public class FloatingDamage : MonoBehaviour
{
    private TextMeshProUGUI text;
    private const float critVibration = 0.2f;
    private const float critDuration = 3f;
    private const float nonCritDuration = 2f;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void Play(FloatingDamageType type, bool isCrit, int value, Vector3 position)
    {
        text.text = value.ToString();
        text.color = GetColorFromType(type);
        transform.position = position + new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), 0);

        if (isCrit)
        {
            text.fontSize = 50;
            StartCoroutine(Crit());
        }
        else
        {
            text.fontSize = 30;
            StartCoroutine(NotCrit());
        }
    }

    private Color GetColorFromType(FloatingDamageType type)
    {
        switch (type)
        {
            case FloatingDamageType.AutoDamage:
                return Color.white;
            case FloatingDamageType.AbilityDamage:
                return Color.yellow;
            case FloatingDamageType.Miss:
                return Color.white;
            default:
                return Color.white;
        }
    }

    private IEnumerator NotCrit()
    {
        float time = 0;
        while (time < nonCritDuration)
        {
            time += Time.deltaTime;
            transform.position += new Vector3(0, 5f, 0) * Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1, 0, time / nonCritDuration));
            yield return null;
        }
        Destroy(gameObject);
    }

    private IEnumerator Crit()
    {
        float time = 0;
        while (time < critDuration)
        {
            time += Time.deltaTime;
            transform.position = new Vector3(transform.position.x + Random.Range(-critVibration, critVibration), transform.position.y + Random.Range(-critVibration, critVibration), transform.position.z);
            text.color = new Color(text.color.r, text.color.g, text.color.b, Mathf.Lerp(1, 0, time / critDuration));
            yield return null;
        }
        Destroy(gameObject);
    }

}

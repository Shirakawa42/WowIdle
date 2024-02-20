using UnityEngine;

public class FloatingDamagesManager : MonoBehaviour
{
    [SerializeField] private GameObject floatingDamagePrefab;
    [SerializeField] private Transform floatingDamagesParent;

    private void Awake()
    {
        Globals.floatingDamagesManager = this;
    }

    public void CreateFloatingDamage(Vector3 position, int damage, FloatingDamageType type, bool isCrit)
    {
        GameObject floatingDamage = Instantiate(floatingDamagePrefab, position, Quaternion.identity, floatingDamagesParent);
        floatingDamage.GetComponent<FloatingDamage>().Play(type, isCrit, damage, position);
    }
}

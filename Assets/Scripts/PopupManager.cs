using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [SerializeField] DamagePopup damagePopupPrefab;
    [SerializeField] Transform canvas;
    private void OnEnable()
    {
        Enemy.OnEnemyDamaged += ShowDamagePopup;
    }


    private void OnDisable()
    {
        Enemy.OnEnemyDamaged -= ShowDamagePopup;
    }


    DamagePopup CreatePopup()
    {
        return Instantiate(damagePopupPrefab, canvas);
    }

    public void ShowDamagePopup(DamageData dd)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(dd.attacked.transform.position + (Vector3)Random.insideUnitCircle);
        DamagePopup popup = CreatePopup();
        popup.Setup(dd.damage, screenPos);
    }
}

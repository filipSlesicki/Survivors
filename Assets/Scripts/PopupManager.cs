using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PopupManager : MonoBehaviour
{
    [SerializeField] DamagePopup damagePopupPrefab;
    [SerializeField] Transform canvas;
    [SerializeField] int initialPoolSize = 50;
    public static ObjectPool<DamagePopup> popupPool;


    private void Start()
    {
        popupPool = new ObjectPool<DamagePopup>(CreatePopup,
            popup => popup.gameObject.SetActive(true),
             popup => popup.gameObject.SetActive(false),
             null, false, 20);

        for (int i = 0; i < initialPoolSize; i++)
        {
            popupPool.Release(CreatePopup());
        }
    }


    DamagePopup CreatePopup()
    {
        return Instantiate(damagePopupPrefab, canvas);
    }

    public void ShowDamagePopup(DamageInfo di)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(di.attacked.transform.position + (Vector3)Random.insideUnitCircle);
        DamagePopup popup = popupPool.Get();
        popup.Setup(di.damage, screenPos);
    }
}

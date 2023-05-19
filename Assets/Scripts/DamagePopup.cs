using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamagePopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float moveSpeed = 3;
    float t = 1;

    public void Setup(float damage,Vector2 screenPos)
    {
        t = 1;
        text.SetText (damage.ToString(),false);
        transform.position = screenPos;
    }

    public void Update()
    {
        t -= Time.deltaTime;
        transform.position += moveSpeed * Time.deltaTime * Vector3.up ;
        text.alpha = t;
        if (t <= 0)
        {
            PopupManager.popupPool.Release(this);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RandomEnemyChance))]
public class RandomChanceInspector : Editor
{
    bool draging;
    public List<ChanceSliderInfo> sliders = new List<ChanceSliderInfo>();

    ChanceSliderInfo left;
    ChanceSliderInfo right;
    public static Rect totalSliderRect;
    int selectedIndex;
    public override void OnInspectorGUI()
    {
        RandomEnemyChance owner = target as RandomEnemyChance;
        base.OnInspectorGUI();

        totalSliderRect = GUILayoutUtility.GetRect(1000, 30, GUILayout.ExpandWidth(true));
        DrawSliders();

        EditorGUI.BeginChangeCheck();

        HandleRightClick();
        ResizeSlidersWithDrag();
        HandleLeftClick();

        EditorGUILayout.Space();

        EditorGUILayout.BeginHorizontal();

        owner.enemies[selectedIndex].enemy = (Enemy)EditorGUILayout.ObjectField(owner.enemies[selectedIndex].enemy, typeof(Enemy), true,
        GUILayout.Width(100), GUILayout.Height(100), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
        EditorGUILayout.EndHorizontal();

        if (EditorGUI.EndChangeCheck())
        {
            Debug.Log("Changed");
            // Handle property changes here
        }

    }

    void ResizeSlidersWithDrag()
    {
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.MouseDown:
                draging = IsOverInteractableArea(e.mousePosition, out left, out right);
                if(draging)
                e.Use();
                break;
            case EventType.MouseDrag:
                if (!draging)
                    break;

                float mouseX = e.mousePosition.x;
                float minX = left != null ? left.rect.xMin : totalSliderRect.xMin;
                float maxX = right != null ? right.rect.xMax : totalSliderRect.xMax;
                mouseX = Mathf.Clamp(mouseX, minX, maxX);
                if (left != null)
                {

                    left.rect.xMax = mouseX;
                }
                if (right != null)
                {
                    right.rect.xMin = mouseX;
                }
                UpdateOwnerDropChances(left, right);

                e.Use();
                break;
            case EventType.MouseUp:
                if (draging)
                {
                    draging = false;
                }
                e.Use();
                break;
            default:
                break;
        }
    }

    void HandleRightClick()
    {
        Event e = Event.current;

        if(e.type == EventType.MouseDown && e.button == 1)
        {
            if (!totalSliderRect.Contains(e.mousePosition))
                return;

            ChanceSliderInfo clicked = ClickedSlider(e.mousePosition);
            if (clicked == null)
                return;


            GenericMenu menu = new GenericMenu();
            RandomEnemyChance owner = target as RandomEnemyChance;
            if (sliders.Count < owner.colors.Length)
            {
                menu.AddItem(new GUIContent("Add before"), false, AddBar, clicked);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Add before"));
            }

            if(sliders.Count == 1)
            {
                menu.AddDisabledItem(new GUIContent("Delete"));
            }
            else
            {
                menu.AddItem(new GUIContent("Delete"), false, RemoveBar, clicked);
            }

            menu.ShowAsContext();
            e.Use();
        }
    }

    void HandleLeftClick()
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown && e.button == 0)
        {
            if (!totalSliderRect.Contains(e.mousePosition))
                return;

            ChanceSliderInfo clicked = ClickedSlider(e.mousePosition);
            if (clicked == null)
                return;

            selectedIndex = clicked.index;

            e.Use();
        }

    }

    ChanceSliderInfo ClickedSlider(Vector2 mousePos)
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            if (sliders[i].rect.Contains(mousePos))
                return sliders[i];
        }
        return null;
    }

    void AddBar(object clickedSlider)
    {
        ChanceSliderInfo slider = clickedSlider as ChanceSliderInfo;
        ChanceSliderInfo newLeft;
        slider.Split(out newLeft);
        RandomEnemyChance owner = target as RandomEnemyChance;
        float lastDropChance = owner.enemies[slider.index].dropChance;
        owner.enemies[slider.index].dropChance = lastDropChance / 2;
        EnemyChance drop = new EnemyChance();
        drop.dropChance = lastDropChance / 2;
        owner.enemies.Insert(slider.index, drop);
    }

    void RemoveBar(object clickedSlider)
    {
        ChanceSliderInfo slider = clickedSlider as ChanceSliderInfo;
        RandomEnemyChance owner = target as RandomEnemyChance;
     
        if(slider.index == 0)
        {
            owner.enemies[slider.index + 1].dropChance += slider.PercentageChance;
            selectedIndex = slider.index + 1;
        }
        else
        {
            owner.enemies[slider.index - 1].dropChance += slider.PercentageChance;
            selectedIndex = slider.index - 1;
        }
        owner.enemies.RemoveAt(slider.index);
    }

    void DrawSliders()
    {
        sliders.Clear();
        RandomEnemyChance owner = target as RandomEnemyChance;
        var items = owner.enemies;
        float totalPercentage = 0;
        if(owner.enemies.Count == 0)
        {
            owner.enemies.Add(new EnemyChance() { dropChance = 1 });
        }
        for (int i = 0; i < items.Count; i++)
        {
            //Calculate size
            float startX = totalSliderRect.xMin + totalPercentage * totalSliderRect.width;
            totalPercentage += items[i].dropChance;
            if(totalPercentage > 1)
            {
                Debug.Log(totalPercentage-1);
                items[i].dropChance -= totalPercentage - 1;
                totalPercentage = 1;
            }
            if(i == items.Count -1)
            {
                if(totalPercentage < 1)
                {
                    items[i].dropChance += 1 - totalPercentage;
                    totalPercentage = 1;
                }
            }
            float endX = totalSliderRect.xMin + totalPercentage * totalSliderRect.width;
            float currentWidth = endX - startX;
            Rect currentRect = new Rect(totalSliderRect.x + startX, totalSliderRect.y, currentWidth, totalSliderRect.height);
            ChanceSliderInfo sliderInfo = new ChanceSliderInfo(currentRect,i);
            EditorGUI.DrawRect(currentRect, owner.colors[i]);

            string itemName = items[i].enemy ? items[i].enemy.name : "No item";
            itemName += "\n" + (items[i].dropChance*100).ToString("0.0") + "%";
            EditorGUI.LabelField(currentRect, itemName);
            sliders.Add(sliderInfo);
            //Mark interactable area
            if (i != 0)
            {
                sliderInfo.SetInteractableAreaLeft();

                sliderInfo.leftNeighbour = sliders[i - 1];
            }
        }

    }
    bool IsOverInteractableArea (Vector2 mousePos, out ChanceSliderInfo left, out ChanceSliderInfo right)
    {
        foreach (var slider in sliders)
        {
            if(slider.resizableRectLeft.Contains(mousePos))
            {
                left = slider.leftNeighbour;
                right = slider;
                return true;
            }
        }
        left = null;
        right = null;
        return false;
        
    }

    void UpdateOwnerDropChances(ChanceSliderInfo left, ChanceSliderInfo right)
    {
        RandomEnemyChance owner = target as RandomEnemyChance;
        if(left != null)
        {
            owner.enemies[left.index].dropChance = left.PercentageChance;
        }
        if(right != null)
        {
            owner.enemies[right.index].dropChance = right.PercentageChance;
        }

    }
}

public class ChanceSliderInfo
{
    public Rect rect;
    public Rect resizableRectLeft;
    public ChanceSliderInfo leftNeighbour;
    public int index;
    
    public float PercentagePosition
    {
        get { return rect.xMax / RandomChanceInspector.totalSliderRect.width; }
    }

    public float PercentageChance { get { return rect.width/ RandomChanceInspector.totalSliderRect.width; } }

    public ChanceSliderInfo(Rect rect, int index)
    {
        this.rect = rect;
        this.index = index;
    }

    public void Split(out ChanceSliderInfo newSliderLeft)
    {
        Rect newRectLeft = new Rect(rect);
        newRectLeft.width /= 2;
        newSliderLeft = new ChanceSliderInfo(newRectLeft,index);
        newSliderLeft.leftNeighbour = this.leftNeighbour;

        this.leftNeighbour = newSliderLeft;
        rect.xMin += rect.width / 2;
        rect.width /= 2;

    }

    public void SetInteractableAreaLeft()
    {
        resizableRectLeft = new Rect(rect.xMin, rect.y, 10, rect.height);
        EditorGUIUtility.AddCursorRect(resizableRectLeft, MouseCursor.ResizeHorizontal);
    }
}

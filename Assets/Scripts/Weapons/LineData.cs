using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu]
public class LineData : ScriptableObject
{
    public LineRenderer linePrefab;

    public Dictionary<Weapon, LineRenderer[]> weaponLines = new Dictionary<Weapon, LineRenderer[]>();
    ObjectPool<LineRenderer> linePool;

    private void OnEnable()
    {
        linePool = new ObjectPool<LineRenderer>(
            () => Instantiate(linePrefab),
            l => l.gameObject.SetActive(true),
            l => l.gameObject.SetActive(false)
            );
    }

    public void MakeLines(Weapon weapon, bool parent = true)
    {
        if(!weaponLines.ContainsKey(weapon))
        {
            weaponLines.Add(weapon, new LineRenderer[weapon.bulletCount]);
        }
        var lineList = weaponLines[weapon];
        if (lineList.Length != weapon.bulletCount)
        {
            lineList = new LineRenderer[weapon.bulletCount];
        }
     

        for (int i = 0; i < weapon.bulletCount; i++)
        {
            LineRenderer currentLine = linePool.Get();
            currentLine.startWidth = weapon.size;
            currentLine.endWidth = weapon.size;
            if (parent)
            {
                currentLine.transform.SetParent(weapon.shootPositions[i]);
                currentLine.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                currentLine.SetPosition(1, new Vector3(weapon.range, 0, 0));
                lineList[i] = currentLine;
            }
        }
    }

    public void DestroyLines(Weapon weapon)
    {
        var lineList = weaponLines[weapon];
        for (int i = lineList.Length - 1; i >= 0; i--)
        {
            linePool.Release(lineList[i]);
        }
    }

    public void UpdateLine(Weapon weapon, int shotIndex, Vector3 endPos, bool parent = true)
    {
        weaponLines[weapon][shotIndex].SetPosition(1, endPos);
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
    }
}

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
        if(linePool == null)
        {
            linePool = new ObjectPool<LineRenderer>(
                CreateLine,
                OnGet,
                OnRelease,
                OnDestroyLine,
                false, 10, 10000
                );
        }

    }

    public void MakeLines(Weapon weapon, bool parent = true)
    {
        if(!weaponLines.ContainsKey(weapon))
        {
            weaponLines.Add(weapon, new LineRenderer[weapon.bulletCount]);
        }
        if (weaponLines[weapon].Length != weapon.bulletCount)
        {
            //DestroyLines(weapon);
            weaponLines[weapon] = new LineRenderer[weapon.bulletCount];
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
                weaponLines[weapon][i] = currentLine;
            }
        }
    }

    public void DestroyLines(Weapon weapon)
    {
        for (int i = weaponLines[weapon].Length - 1; i >= 0; i--)
        {
            linePool.Release(weaponLines[weapon][i]);
        }
    }

    public void UpdateLine(Weapon weapon, int shotIndex, Vector3 endPos, bool parent = true)
    {
        weaponLines[weapon][shotIndex].SetPosition(1, endPos);
    }

    LineRenderer CreateLine()
    {
        return Instantiate(linePrefab);
    }

    void OnGet(LineRenderer line)
    {
        line.gameObject.SetActive(true);
    }

    void OnRelease(LineRenderer line)
    {
        line.gameObject.SetActive(false);
    }

    void OnDestroyLine(LineRenderer line)
    {
        Destroy(line.gameObject);
    }
}

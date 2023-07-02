using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move Costs")]
public class LayerCosts : ScriptableObject
{
    public LayerMoveCost[] moveCosts;

    public int GetCombinedMask()
    {
        int combined = 0;
        for (int i = 0; i < moveCosts.Length; i++)
        {
            combined |= moveCosts[i].layer;
        }
        return combined;
    }

    public byte GetTerrainCost(int objectLayer)
    {
        int layerAsLayerMask = (1 << objectLayer);
        foreach (var moveCost in moveCosts)
        {
            if((moveCost.layer & layerAsLayerMask) != 0)
            {
                return moveCost.cost;
            }
        }

        return 1;
    }
    [ContextMenu("Test cost")]
    public void TerrainCostTest()
    {
        Debug.Log(GetTerrainCost(15));
    }

}

[System.Serializable]
public class LayerMoveCost
{
    public LayerMask layer;
    public byte cost;

}


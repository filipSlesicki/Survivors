using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public struct Cell
{
	public float2 worldPos;
	
	public int2 gridIndex;
	public int index;
	public byte cost;
	public ushort bestCost;
	public int bestDirectionIndex;
	public Vector3 WorldPos3D { get { return new Vector3(worldPos.x, worldPos.y, 0); } }
	public int2 BestDirection
	{
		get
		{
			if (bestDirectionIndex == -1)
				return new int2(0, 0);

			return FlowField.directions[bestDirectionIndex];
		}
	}

	public Vector2 BestNormalizedDirection
	{
		get
		{
			if (bestDirectionIndex == -1)
				return new Vector2(0, 0);

			return FlowField.normalizedDirections[bestDirectionIndex];
		}
	}

	public Cell(float2 _worldPos, int2 _gridIndex, int flatIndex)
	{
		worldPos = _worldPos;
		gridIndex = _gridIndex;
		cost = 1;
		bestCost = ushort.MaxValue;
		bestDirectionIndex = -1;
		index = flatIndex;
	}

	public Cell(Cell cell)
	{
		this.worldPos = cell.worldPos;
		this.gridIndex = cell.gridIndex;
		this.cost = cell.cost;
		bestCost = ushort.MaxValue;
		bestDirectionIndex = -1;
		this.index = cell.index;
	}

	public void Reset()
	{
		bestCost = ushort.MaxValue;
		bestDirectionIndex = -1;
	}

	public void IncreaseCost(int amnt)
	{
		if (cost == byte.MaxValue) { return; }
		if (amnt + cost >= 255) { cost = byte.MaxValue; }
		else { cost += (byte)amnt; }
	}

	public override string ToString()
	{
		return gridIndex.ToString();
	}
}

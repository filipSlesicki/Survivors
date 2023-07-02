using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Burst.CompilerServices;

[BurstCompile]
public struct CalculateFlowFieldJob : IJob
{
	public NativeArray<Cell> grid;
	[ReadOnly]
	public Cell target;
	[ReadOnly]
	public int width;
	[ReadOnly]
	public int height;
	[ReadOnly]
	public NativeArray<int2> directions;
	public void Execute()
	{
		ResetCells();
		CreateIntegrationField(target);
		CreateFlowField();
	}

	public void ResetCells()
	{
		int cellCount = grid.Length;
		for (int i = 0; i < cellCount; i++)
		{
			grid[i] = new Cell(grid[i]);
		}
	}

	public void CreateIntegrationField(Cell destinationCell)
	{
		destinationCell.bestCost = 0;
		grid[destinationCell.index] = destinationCell;
		NativeQueue<Cell> cellsToCheck = new NativeQueue<Cell>(Allocator.Temp);
		cellsToCheck.Enqueue(destinationCell);

		NativeArray<Cell> neighbors = new NativeArray<Cell>(4, Allocator.Temp);
		while (cellsToCheck.Count > 0)
		{
			Cell curCell = cellsToCheck.Dequeue();

			int validNeighborCount = GetNeighbors(curCell.gridIndex, neighbors);
			for (int i = 0; i < validNeighborCount; i++)
			{
				Cell curNeighbor = neighbors[i];
				if (curNeighbor.cost == byte.MaxValue) { continue; }
				if (curNeighbor.cost + curCell.bestCost < curNeighbor.bestCost)
				{
					curNeighbor.bestCost = (ushort)(curNeighbor.cost + curCell.bestCost);
					cellsToCheck.Enqueue(curNeighbor);
					grid[curNeighbor.index] = curNeighbor;
				}
			}
		}
		neighbors.Dispose();
		cellsToCheck.Dispose();
	}

	public void CreateFlowField()
	{
		int cellCount = grid.Length;
		NativeArray<Cell> neighbors = new NativeArray<Cell>(8, Allocator.Temp);
		for (int i = 0; i < cellCount; i++)
		{
			Cell curCell = grid[i];

			int validNeighborCount = GetNeighbors(curCell.gridIndex, neighbors);

			int bestCost = curCell.bestCost;

			for (int j = 0; j < validNeighborCount; j++)
			{
				Cell curNeighbor = neighbors[j];
				if (curNeighbor.bestCost < bestCost)
				{
					bestCost = curNeighbor.bestCost;
					int2 dir = curNeighbor.gridIndex - curCell.gridIndex;
					curCell.bestDirectionIndex = GetDirectionIndex( dir.x, dir.y);
					grid[i] = curCell;
				}
			}

		}
		neighbors.Dispose();
	}

	public int GetFlatIndex(int x, int y)
	{
		return y + x * height;
	}

	[return: AssumeRange(0, 8)]
	private int GetNeighbors(int2 from, NativeArray<Cell> neighbors)
	{
		int validNeighbors = 0;

		for (int i = 0; i < neighbors.Length; i++)
		{
			int neighborId = GetCellAtRelativePos(from, directions[i]);
			if (neighborId != -1)
			{
				neighbors[validNeighbors] = grid[neighborId];
				validNeighbors++;
			}
		}
		return validNeighbors;
	}

	private int GetCellAtRelativePos(int2 orignPos, int2 relativePos)
	{
		int2 finalPos = orignPos + relativePos;

		if (finalPos.x >= 0 && finalPos.x < width && finalPos.y >= 0 && finalPos.y < height)
		{
			return GetFlatIndex(finalPos.x, finalPos.y);
		}
		return -1;
	}
	[return: AssumeRange(-1, 7)]
	int GetDirectionIndex(int x, int y)
    {
		switch (x, y)
		{
			case (0, 1):
				return 0;
			case (0, -1):
				return 1;
			case (1, 0):
				return 2;
			case (-1, 0):
				return 3;
			case (1, 1):
				return 4;
			case (-1, 1):
				return 5;
			case (1, -1):
				return 6;
			case (-1, -1):
				return 7;
			default:
				return -1;
		}
	}

}
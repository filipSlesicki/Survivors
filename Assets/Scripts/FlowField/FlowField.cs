using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
public class FlowField
{
	public Cell[] grid;
	public readonly int width;
	public readonly int height;
	public float cellRadius { get; private set; }

	private float cellDiameter;
	public static readonly int2[] directions = new int2[]
	{
		new int2(0,1),		//North
		new int2(0,-1),		//South
		new int2(1,0),		//East
		new int2(-1,0),		//West
		new int2(1,1),		//NorthEast 
		new int2(-1,1),		//NorthWest
		new int2(1,-1),		//SouthEast
		new int2(-1,-1),	//SouthWest
	};
	public static readonly Vector2[] normalizedDirections = new Vector2[]
	{
		new Vector2(0,1),			//North
		new Vector2(0,-1),		//South
		new Vector2(1,0),			//East
		new Vector2(-1,0),		//West
		new Vector2(0.7f,0.7f),	//NorthEast 
		new Vector2(-0.7f,0.7f),	//NorthWest
		new Vector2(0.7f,-0.7f),	//SouthEast
		new Vector2(-0.7f,-0.7f),	//SouthWest
	};

	[ReadOnly]
	NativeArray<int2> _directions;

	public CalculateFlowFieldJob calculateFlowFieldJob;

	public FlowField(float _cellRadius, int2 _gridSize)
	{
		cellRadius = _cellRadius;
		cellDiameter = cellRadius * 2f;
		width = _gridSize.x;
		height = _gridSize.y;
		_directions = new NativeArray<int2>(directions, Allocator.Persistent);
	}

	public void OnDestroy()
	{
		_directions.Dispose();
	}



	public void CreateGrid()
	{
		grid = new Cell[width * height];
		int i = 0;
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{

				Vector2 worldPos = new Vector2(cellDiameter * x + cellRadius,cellDiameter * y + cellRadius);
				grid[i] = new Cell(worldPos, new int2(x, y), i);
				i++;
			}
		}
	}


	public void CreateCostField(LayerCosts layerCosts)
	{
		Vector3 cellHalfExtents = Vector3.one * cellRadius;
		int terrainMask = layerCosts.GetCombinedMask();

		int cellCount = grid.Length;
		for (int i = 0; i < cellCount; i++)
		{
			Cell currentCell = grid[i];

			Collider2D[] obstacles = Physics2D.OverlapBoxAll(currentCell.worldPos, cellHalfExtents, 0, terrainMask);
			foreach (Collider2D col in obstacles)
			{
				int terrainCost = layerCosts.GetTerrainCost(col.gameObject.layer);
				currentCell.IncreaseCost(terrainCost);
			}
			grid[i] = currentCell;
		}
	}

	public JobHandle CalculateFlowFieldToTarget(Cell destinationCell)
	{
		calculateFlowFieldJob = new CalculateFlowFieldJob()
		{
			grid = new NativeArray<Cell>(grid, Allocator.TempJob),
			target = destinationCell,
			width = width,
			height = height,
			directions = _directions
		};

		return calculateFlowFieldJob.Schedule();
	}

	public void FinishFlowFieldJob(JobHandle handle)
	{
		handle.Complete();
		calculateFlowFieldJob.grid.CopyTo(grid);
		//this.grid.CopyFrom(calculateFlowFieldJob.grid);
		calculateFlowFieldJob.grid.Dispose();
	}

	public void CleanJobHandle(JobHandle handle)
	{
		handle.Complete();
		calculateFlowFieldJob.grid.Dispose();
	}

	public int GetFlatIndex(int x, int y)
	{
		return y + x * height;
	}

	public Cell GetCellFromWorldPos(Vector2 worldPos)
	{
		float inverseCellDiameter = 1f / cellDiameter;
		int gridX = Mathf.Clamp((int)((worldPos.x) * inverseCellDiameter), 0, width - 1);
		int gridY = Mathf.Clamp((int)((worldPos.y) * inverseCellDiameter), 0, height - 1);
		int index = GetFlatIndex(gridX, gridY);
		return grid[index];
	}

}

using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
public class FlowField
{
	Grid grid;

	[ReadOnly]
	static NativeArray<int2> _directions;

	public CalculateFlowFieldJob calculateFlowFieldJob;

	public FlowField(Grid grid)
	{
		this.grid = grid;
		if(!_directions.IsCreated)
		_directions = new NativeArray<int2>(Directions.directions, Allocator.Persistent);
	}

	public void OnDestroy()
	{
		_directions.Dispose();
	}


	public JobHandle CalculateFlowFieldToTarget(Cell destinationCell)
	{
		calculateFlowFieldJob = new CalculateFlowFieldJob()
		{
			grid = new NativeArray<Cell>(grid.cells, Allocator.TempJob),
			target = destinationCell,
			width = grid.width,
			height = grid.height,
			directions = _directions
		};

		return calculateFlowFieldJob.Schedule();
	}

	public void FinishFlowFieldJob(JobHandle handle)
	{
		handle.Complete();
		calculateFlowFieldJob.grid.CopyTo(grid.cells);
		//this.grid.CopyFrom(calculateFlowFieldJob.grid);
		calculateFlowFieldJob.grid.Dispose();
	}

	public void CleanJobHandle(JobHandle handle)
	{
		handle.Complete();
		calculateFlowFieldJob.grid.Dispose();
	}
	public void CreateCostField(LayerCosts layerCosts)
	{
		Vector3 cellHalfExtents = Vector3.one * grid.cellRadius;
		int terrainMask = layerCosts.GetCombinedMask();

		int cellCount = grid.cells.Length;
		for (int i = 0; i < cellCount; i++)
		{
			Cell currentCell = grid.cells[i];

			Collider2D[] obstacles = Physics2D.OverlapBoxAll(currentCell.worldPos, cellHalfExtents, 0, terrainMask);
			foreach (Collider2D col in obstacles)
			{
				int terrainCost = layerCosts.GetTerrainCost(col.gameObject.layer);
				currentCell.IncreaseCost(terrainCost);
			}
			grid.cells[i] = currentCell;
		}
	}
}

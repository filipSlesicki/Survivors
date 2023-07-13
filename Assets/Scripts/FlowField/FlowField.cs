using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
public class FlowField
{
	Grid grid;

	[ReadOnly]
	static NativeArray<int2> _directions;

	NativeArray<Cell> nativeGrid;

	CalculateFlowFieldJob calculateFlowFieldJob;
	CalculateFlowFieldToPortalsJob calculateFlowFieldToPortalJob;

	public FlowField(Grid grid)
	{
		this.grid = grid;
		nativeGrid = new NativeArray<Cell>(grid.cells, Allocator.Persistent);
		if (!_directions.IsCreated)
		_directions = new NativeArray<int2>(Directions.directions, Allocator.Persistent);
		calculateFlowFieldJob = new CalculateFlowFieldJob();
		calculateFlowFieldToPortalJob = new CalculateFlowFieldToPortalsJob();
	}

	public void OnDestroy()
	{
		if(_directions.IsCreated)
		_directions.Dispose();

		nativeGrid.Dispose();
	}


	public JobHandle CalculateFlowFieldToTarget(Cell destinationCell)
	{
		nativeGrid.CopyFrom(grid.cells);
		calculateFlowFieldJob.grid = nativeGrid;
		calculateFlowFieldJob.directions = _directions;
		calculateFlowFieldJob.width = grid.width;
		calculateFlowFieldJob.height = grid.height;
		calculateFlowFieldJob.target = destinationCell;

		return calculateFlowFieldJob.Schedule();
	}

	public JobHandle CalculateFlowFieldToPortal(Cell[] destinationCells, int directionIndex)
	{
		nativeGrid.CopyFrom(grid.cells);
		calculateFlowFieldToPortalJob.grid = nativeGrid;
		calculateFlowFieldToPortalJob.directions = _directions;
		calculateFlowFieldToPortalJob.width = grid.width;
		calculateFlowFieldToPortalJob.height = grid.height;
		calculateFlowFieldToPortalJob.targets = new NativeArray<Cell>(destinationCells, Allocator.TempJob) ;
		calculateFlowFieldToPortalJob.direction = directionIndex;
		return calculateFlowFieldToPortalJob.Schedule();
	}

	public void FinishFlowFieldJob(JobHandle handle)
	{
		handle.Complete();
		calculateFlowFieldJob.grid.CopyTo(grid.cells);
	}
	public void FinishFlowfieldToPortalJob(JobHandle handle)
	{
		handle.Complete();
		calculateFlowFieldToPortalJob.grid.CopyTo(grid.cells);
		calculateFlowFieldToPortalJob.targets.Dispose();
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

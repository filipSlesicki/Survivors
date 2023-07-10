using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
/// <summary>
/// Controls world grid and flow fields. Sectors are still Work in progress.
/// </summary>
public class GridController : MonoBehaviour
{
	[Range(0,3)]
	[Tooltip("How many frames can a flow field calculation for current sector run")]
	public int currentSectorFFJobDuration;
	[Range(0, 3)]
	[Tooltip("How many frames can a flow field calculation for other sectors run")]
	public int otherSectorsFFJobDuration;
	[Tooltip("How many sectors are horizontally and vertically")]
	public int2 sectorCount;
	[Tooltip("How big is every sector in cells")]
	public int sectorSize = 10;
	[Tooltip("Radius of each individual cell")]
	public float cellRadius = 0.5f;
	public LayerCosts moveCosts;
	/// <summary>
	/// Grid of sectors as cells
	/// </summary>
	public Grid sectorGrid;
	/// <summary>
	/// Array of sectors
	/// </summary>
	public Sector[] sectors;
	/// <summary>
	/// Flow target. Flowield targets it's position
	/// </summary>
	public FlowFieldTarget target;
	/// <summary>
	/// Sector the <see cref="target"/ is currently in>
	/// </summary>
	public static Sector playerSector;
	/// <summary>
	/// Event called when Flowifield job finishes
	/// </summary>
	public UnityEngine.Events.UnityEvent OnFlowfieldChanged;

	/// <summary>
	/// Handle to finish flowfield calcilation for <see cref="playerSector"/> 
	/// </summary>
	JobHandle currentSectorHandle;
	/// <summary>
	/// Singleton instance for easy access. Might change later
	/// </summary>
	public static GridController Instance;

	[HideInInspector] public Vector2 topRightCorner;
	bool changedSector;
	bool changedCell;
	int currentSectorjobFrames = 0;
	int otherSectorsjobFrames = 0;
	Dictionary<int, JobHandle> jobHandlesDictionary;
	private void Awake()
	{
		InitializeGrid();
		Instance = this;
	}
    private void FixedUpdate()
	{
		UpdateSectorChange();
		UpdateCellChange();
	}
	private void LateUpdate()
	{
		FinishCurrentSectorJob();
		FinishOtherSectorJobs();
	}

	/// <summary>
	/// Initializes grids and sectors
	/// </summary>
	private void InitializeGrid()
	{
		sectorGrid = new Grid(sectorSize / 2, sectorCount, Vector2.zero);

		sectors = new Sector[sectorCount.x * sectorCount.y];
		jobHandlesDictionary = new Dictionary<int, JobHandle>(sectors.Length);
		for (int i = 0; i < sectorGrid.cells.Length; i++)
		{
			sectors[i] = new Sector(cellRadius, sectorSize, sectorGrid.cells[i].worldPos, i);
			sectors[i].flowField.CreateCostField(moveCosts);
		}
		playerSector = sectors[sectorGrid.GetCellFromWorldPos(target.trans.position).flatIndex];
		PlacePortals();
		UpdateSectorChange();
		OnFlowfieldChanged?.Invoke();
		topRightCorner = new Vector2
			(sectorCount.x * sectorSize * cellRadius * 2,
			sectorCount.y * sectorSize * cellRadius * 2);
	}
	/// <summary>
	/// Checks if target moved to another sector and updates All sectors flowFields
	/// </summary>
	void UpdateSectorChange()
    {

		Cell sectorCell = sectorGrid.GetCellFromWorldPos(target.trans.position);

		if (target.lastSectorIndex != sectorCell.flatIndex && !changedSector)
		{
			changedSector = true;
			playerSector = sectors[sectorCell.flatIndex];
			target.lastSectorIndex = sectorCell.flatIndex;
			SetPathsToSector(playerSector.index);
			for (int i = 0; i < sectors.Length; i++)
			{
				if (i == sectorCell.flatIndex)
				{
					continue;
				}
				//sectors[i].CalculateFlowToPortalImmediately(sectorGrid.cells[i].bestDirectionIndex);
				if(sectorGrid.cells[i].bestDirectionIndex == -1)
                {
					continue;
				}
				var sectorJobHandle = sectors[i].CalculateFlowfieldToPortal(sectorGrid.cells[i].bestDirectionIndex);
				jobHandlesDictionary.Add(i, sectorJobHandle);
			}
		}
	}
	/// <summary>
	/// Checks if target moved to another cell and updates current sector flow field
	/// </summary>
	void UpdateCellChange()
	{
		Cell targetCell = sectors[playerSector.index].grid.GetCellFromWorldPos(target.trans.position);
		int currentTargetPos = targetCell.flatIndex;
		if (target.lastGridIndex != currentTargetPos && !changedCell)
		{
			target.lastGridIndex = currentTargetPos;
			//if (changedCell)
			//{
			//	Debug.Log("still running last update");
			//	return;
			//}
			changedCell = true;
			currentSectorHandle = playerSector.flowField.CalculateFlowFieldToTarget(targetCell);
			JobHandle.ScheduleBatchedJobs();
		}
	}
	/// <summary>
	/// Finishes flowfield job for current sector
	/// </summary>
	void FinishCurrentSectorJob()
    {
		if (changedCell)
		{
			if (currentSectorjobFrames >= currentSectorFFJobDuration)
			{
				changedCell = false;
				currentSectorjobFrames = 0;

				playerSector.flowField.FinishFlowFieldJob(currentSectorHandle);
				if (!changedSector)
				{
					OnFlowfieldChanged?.Invoke();
				}
			}
			else
			{
				currentSectorjobFrames++;
			}				
		}
	}
	/// <summary>
	/// Finished flowfield jobs for all other secotors
	/// </summary>
	void FinishOtherSectorJobs()
    {
		if (changedSector)
		{
			if (otherSectorsjobFrames >= otherSectorsFFJobDuration)
            {
                foreach (var secotrJob in jobHandlesDictionary)
                {
					int sectorIndex = secotrJob.Key;
					sectors[sectorIndex].FinishPathToPortal(secotrJob.Value, sectorGrid.cells[sectorIndex].bestDirectionIndex);
                }
				jobHandlesDictionary.Clear();
				changedSector = false;
				OnFlowfieldChanged?.Invoke();
				otherSectorsjobFrames = 0;
			}
			else
            {
				otherSectorsjobFrames++;
			}
		
		}
	}

	/// <summary>
	/// Place portals between sectors
	/// </summary>
	void PlacePortals()
    {
		//TODO: Multiple portals per sector
		int sectorCount = sectors.Length;
		//reusable edge cells buffers
		Cell[] currentEdgeCells = new Cell[sectorSize]; 
		Cell[] neighbourEdgeCells = new Cell[sectorSize];

		for (int sectorIndex = 0; sectorIndex < sectorCount; sectorIndex++)
        {
			Sector currentSector = sectors[sectorIndex];
			int2 sectorGridPos = sectorGrid.cells[sectorIndex].gridIndex;

			//Add portals in every cardinal direction
            for (int directionIndex = 0; directionIndex < 4; directionIndex++)
            {
				//Find neighbour sector in direction
				int neighbourSectorIndex = sectorGrid.GetCellInDirection(sectorGridPos, Directions.directions[directionIndex]);
				if (neighbourSectorIndex == -1)
					continue;

				Sector neighbourSector = sectors[neighbourSectorIndex];
				//Get all edge cells for current and neighbour sectors and save them to buffers
				currentSector.grid.GetEdgeCells(directionIndex, currentEdgeCells);
				neighbourSector.grid.GetEdgeCells(Directions.GetOppositeDirection(directionIndex), neighbourEdgeCells);


				//Find first open cell from the middle.
				int middleIndex = sectorSize / 2;
				for (int i = 0; i < sectorSize; i++)
				{
					int sign = i % 2 == 0 ? 1 : -1;
					int index = middleIndex + Mathf.CeilToInt(i / 2f) * sign;
					//Check if both sides are walkable
					if (currentEdgeCells[index].cost != 255 && neighbourEdgeCells[index].cost != 255)
					{
						SectorPortal portal = new SectorPortal(currentSector.index, neighbourSector.index, currentEdgeCells[index]);
						currentSector.portalCells[directionIndex] = portal;
						break;
					}
					currentSector.portalCells[directionIndex] = null;
				}
			}

        }
    }
	/// <summary>
	/// Gets cell from world position
	/// </summary>
	/// <param name="worldPos">Object's position</param>
	/// <returns>grid cell in current sector</returns>
	public Cell GetCellAtWorldPosition(Vector2 worldPos)
	{
		var sector = sectorGrid.GetCellFromWorldPos(worldPos);
		return sectors[sector.flatIndex].grid.GetCellFromWorldPos(worldPos);
	}

	public bool IsPositionValid(Vector2 worldPos)
    {
		Cell cellAtPos = GetCellAtWorldPosition(worldPos);
		return cellAtPos.bestCost != 255;

	}

	Queue<Cell> queue = new Queue<Cell>(50);
	/// <summary>
	/// Sets sector directions to go to target sector
	/// </summary>
	/// <param name="targetSectorIndex">sector index</param>
	void SetPathsToSector(int targetSectorIndex)
	{
		//Reset sector costs
		for (int i = 0; i < sectors.Length; i++)
		{
			var sectorCell = sectorGrid.cells[i];
			sectorCell.bestCost = ushort.MaxValue;
			sectorCell.bestDirectionIndex = -1;
			sectorGrid.cells[i] = sectorCell;
		}
		

		Cell startSectorCell = sectorGrid.cells[targetSectorIndex];
		startSectorCell.bestCost = 0;
		queue.Enqueue(startSectorCell);
		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			Sector currentSector = sectors[current.flatIndex];
			for (int i = 0; i < 4; i++) //TODO: Add support for more portals
			{
				var portal = currentSector.portalCells[i];
				if (portal == null)
					continue;
				var neighbourIndex = portal.toIndex;
				ushort nextCost = (ushort)(current.bestCost + 1);//TODO: Change to real distance
				if (nextCost < sectorGrid.cells[neighbourIndex].bestCost)
				{
					var neighbourCell = sectorGrid.cells[neighbourIndex];
					neighbourCell.bestCost = nextCost;
					neighbourCell.bestDirectionIndex = Directions.GetOppositeDirection(i);
					sectorGrid.cells[neighbourIndex] = neighbourCell;
					queue.Enqueue(neighbourCell);
				}
			}
		}
	}

	/// <summary>
	/// Cleanup
	/// </summary>
	private void OnDisable()
	{
		//Cleanup jobs
		if (!currentSectorHandle.IsCompleted)
		{
			playerSector.flowField.CleanJobHandle(currentSectorHandle);
		}

        foreach (var sectorJob in jobHandlesDictionary)
        {
			sectors[sectorJob.Key].flowField.CleanJobHandle(sectorJob.Value);

		}
		jobHandlesDictionary.Clear();
		playerSector.flowField.OnDestroy();

	}
}

using UnityEditor;
using UnityEngine;
using Unity.Mathematics;


public enum FlowFieldDisplayType { None, AllIcons, DestinationIcon, CostField, IntegrationField, Indexes, SectorDirections };

public class GridDebug : MonoBehaviour
{
	public GridController gridController;
	public bool displayGrid;
	public bool drawSectors;
	public bool drawSubSectors;

	public FlowFieldDisplayType curDisplayType;

	private int2 gridSize;
	private float cellRadius;
	private Sector curSector;

	public SpriteRenderer directionIconPrefab;
	public Sprite[] directionArrows;
	public Sprite targetIcon;
	public Sprite unwalkableIcon;


    public void SetFlowField(Sector sector)
	{
		curSector = sector;
		cellRadius = sector.grid.cellRadius;
		gridSize =  new int2( sector.grid.width,sector.grid.height);
	}
	
	public void DrawFlowField()
	{
		if (!displayGrid)
			return;

		if(curSector == null)
        {
			SetFlowField(GridController.playerSector);
        }
		ClearCellDisplay();

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.AllIcons:
				DisplayAllCells();
				break;

			case FlowFieldDisplayType.SectorDirections:
				DisplaySectorCells();
				break;

			case FlowFieldDisplayType.DestinationIcon:
				//DisplayDestinationCell();
				break;

			default:
				break;
		}
	}

	private void DisplayAllCells()
	{
		if (curSector == null) 
		{ 
			return; 
		}

        foreach (var sector in gridController.sectors)
        {
			foreach (Cell curCell in sector.grid.cells)
			{
				DisplayCell(curCell);
			}
		}
	}

	private void DisplaySectorCells()
    {
        foreach (var sectorCell in gridController.sectorGrid.cells)
        {
			DisplayCell(sectorCell);
		}
    }


    private void DisplayCell(Cell cell)
	{
		SpriteRenderer iconSR = Instantiate(directionIconPrefab);
		iconSR.transform.parent = transform;
		iconSR.transform.position = cell.WorldPos3D;
		iconSR.gameObject.name =  cell.gridIndex.ToString();

		if (cell.cost == 255)
		{
			iconSR.sprite = unwalkableIcon;
		}
		else if (cell.bestCost == 0)
		{
			iconSR.sprite = targetIcon;
		}
		else
        {
			if(cell.bestDirectionIndex>=0)
			iconSR.sprite = directionArrows[cell.bestDirectionIndex];

		}
	}

	public void ClearCellDisplay()
	{
		foreach (Transform t in transform)
		{
			GameObject.Destroy(t.gameObject);
		}
	}
	
	private void OnDrawGizmos()
	{
		if (displayGrid)
		{
			if(drawSubSectors)
				DrawSubsectors();
			if(drawSectors)
				DrawSectors();

		}
		
		if (curSector == null) { return; }

		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.MiddleCenter;

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.CostField:

				foreach (Cell curCell in curSector.grid.cells)
				{
					Handles.Label(curCell.WorldPos3D, curCell.cost.ToString(), style);
				}
				break;
				
			case FlowFieldDisplayType.IntegrationField:

				foreach (Cell curCell in curSector.grid.cells)
				{
					Handles.Label(curCell.WorldPos3D, curCell.bestCost.ToString(), style);
				}
				break;

			case FlowFieldDisplayType.Indexes:
                foreach (var sector in gridController.sectors)
                {
					foreach (Cell curCell in sector.grid.cells)
					{
						Handles.Label(curCell.WorldPos3D, curCell.flatIndex.ToString(), style);
					}
				}
				//foreach (Cell curCell in curSector.grid.cells)
				//{
				//	Handles.Label(curCell.WorldPos3D, curCell.index.ToString(), style);
				//}
				break;

			default:
				break;
		}
		
	}

	void DrawSectors()
    {
		if (gridController.sectorGrid == null)
		{
			DrawGrid(gridController.sectorCount, Color.red, gridController.sectorSize/2, Vector3.zero);
		}
		else
		gridController.sectorGrid.Draw(Color.red);
    }

	void DrawSubsectors()
    {
		if(gridController.sectors == null)
        {
            for (int x = 0; x < gridController.sectorCount.x; x++)
            {
                for (int y = 0; y < gridController.sectorCount.y; y++)
                {
					Vector3 startPos = new Vector3(x * gridController.sectorSize, y * gridController.sectorSize, 0);
					DrawGrid(gridController.sectorSize, Color.green, gridController.cellRadius, startPos);
				}
            }
        }
		else
        {
			foreach (var sector in gridController.sectors)
			{
				sector.grid.Draw(Color.yellow);
			}
		}

    }

	private void DrawGrid(int2 drawGridSize, Color drawColor, float drawCellRadius, Vector3 startPos)
	{
		Gizmos.color = drawColor;
		for (int x = 0; x < drawGridSize.x; x++)
		{
			for (int y = 0; y < drawGridSize.y; y++)
			{
				Vector3 center = startPos +  new Vector3(drawCellRadius * 2 * x + drawCellRadius, drawCellRadius * 2 * y + drawCellRadius,0);
				Vector3 size = Vector3.one * drawCellRadius * 2;
				Gizmos.DrawWireCube(center, size);
			}
		}
	}
}

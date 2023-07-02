using UnityEditor;
using UnityEngine;
using Unity.Mathematics;


public enum FlowFieldDisplayType { None, AllIcons, DestinationIcon, CostField, IntegrationField };

public class GridDebug : MonoBehaviour
{
	public GridController gridController;
	public bool displayGrid;

	public FlowFieldDisplayType curDisplayType;

	private int2 gridSize;
	private float cellRadius;
	private FlowField curFlowField;

	public SpriteRenderer directionIconPrefab;
	public Sprite[] directionArrows;
	public Sprite targetIcon;
	public Sprite unwalkableIcon;


    public void SetFlowField(FlowField newFlowField)
	{
		curFlowField = newFlowField;
		cellRadius = newFlowField.cellRadius;
		gridSize =  new int2( newFlowField.width,newFlowField.height);
	}
	
	public void DrawFlowField()
	{
		if (!displayGrid)
			return;

		if(curFlowField == null)
        {
			SetFlowField(GridController.curFlowField);
        }
		ClearCellDisplay();

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.AllIcons:
				DisplayAllCells();
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
		if (curFlowField == null) 
		{ 
			return; 
		}

		foreach (Cell curCell in curFlowField.grid)
		{
			DisplayCell(curCell);
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
			if (curFlowField == null)
			{
				DrawGrid(gridController.gridSize, Color.yellow, gridController.cellRadius);
			}
			else
			{
				DrawGrid(gridSize, Color.green, cellRadius);
			}
		}
		
		if (curFlowField == null) { return; }

		GUIStyle style = new GUIStyle(GUI.skin.label);
		style.alignment = TextAnchor.MiddleCenter;

		switch (curDisplayType)
		{
			case FlowFieldDisplayType.CostField:

				foreach (Cell curCell in curFlowField.grid)
				{
					Handles.Label(curCell.WorldPos3D, curCell.cost.ToString(), style);
				}
				break;
				
			case FlowFieldDisplayType.IntegrationField:

				foreach (Cell curCell in curFlowField.grid)
				{
					Handles.Label(curCell.WorldPos3D, curCell.bestCost.ToString(), style);
				}
				break;
				
			default:
				break;
		}
		
	}

	private void DrawGrid(int2 drawGridSize, Color drawColor, float drawCellRadius)
	{
		Gizmos.color = drawColor;
		for (int x = 0; x < drawGridSize.x; x++)
		{
			for (int y = 0; y < drawGridSize.y; y++)
			{
				Vector3 center = new Vector3(drawCellRadius * 2 * x + drawCellRadius, drawCellRadius * 2 * y + drawCellRadius,0);
				Vector3 size = Vector3.one * drawCellRadius * 2;
				Gizmos.DrawWireCube(center, size);
			}
		}
	}
}

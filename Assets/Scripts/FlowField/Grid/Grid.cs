using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class Grid
{
	/// <summary>
	/// Grid with in cells
	/// </summary>
	public readonly int width;
	/// <summary>
	/// Grid height in cells
	/// </summary>
	public readonly int height;
	/// <summary>
	/// Radius of each cell. Half of total extents
	/// </summary>
	public readonly float cellRadius;
	/// <summary>
	/// Diammeter of each cell. Radius *2
	/// </summary>
	private readonly float cellDiameter;
	/// <summary>
	/// Start of the grid
	/// </summary>
	public readonly Vector2 bottomLeftCorner;
	/// <summary>
	/// All cells in this grid
	/// </summary>
	public Cell[] cells;

	public Grid(float _cellRadius, int2 _gridSize, Vector2 bottomLeftCorner)
	{
		cellRadius = _cellRadius;
		cellDiameter = cellRadius * 2f;
		width = _gridSize.x;
		height = _gridSize.y;
		this.bottomLeftCorner = bottomLeftCorner;
		CreateGrid(bottomLeftCorner);
	}

	public void CreateGrid(Vector2 startPos)
	{
		cells = new Cell[width * height];
		int i = 0;
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Vector2 worldPos = startPos + new Vector2(cellDiameter * x + cellRadius, cellDiameter * y + cellRadius);
				cells[i] = new Cell(worldPos, new int2(x, y), i);
				i++;
			}
		}
	}
	/// <summary>
	/// Transforms cell's X and Y cooridinates into <see cref="cells"/> index
	/// </summary>
	/// <param name="x">X cooridinate</param>
	/// <param name="y">Y coordinate</param>
	/// <returns></returns>
	public int CalculateFlatIndex(int x, int y)
	{
		return y + x * height;
	}

	public Cell GetCellFromWorldPos(Vector2 worldPos)
	{
		float inverseCellDiameter = 1f / cellDiameter;
		int gridX = Mathf.Clamp((int)((worldPos.x) * inverseCellDiameter - bottomLeftCorner.x), 0, width - 1);
		int gridY = Mathf.Clamp((int)((worldPos.y) * inverseCellDiameter - bottomLeftCorner.y), 0, height - 1);
		int index = CalculateFlatIndex(gridX, gridY);
		return cells[index];
	}

	/// <summary>
	/// Creates a list of all cell indexes in range from world position
	/// </summary>
	/// <param name="wordPosition">from position</param>
	/// <param name="range">check range</param>
	/// <returns>list of all indexes in range</returns>
	public List<int> GetCellIndexesInRange(Vector3 wordPosition, float range)
	{
		List<int> inRange = new List<int>();
		//Transform world position to grid local position
		Vector2 localPosition = (Vector2)wordPosition - bottomLeftCorner;
		// Calculate the grid coordinates of the center position
		Vector2 centerGridPos =  new Vector2(localPosition.x / cellDiameter, localPosition.y / cellDiameter);

		// Calculate the range in terms of grid units
		int rangeInGridUnits = Mathf.CeilToInt(range / cellDiameter);

		//Calculate extents
		int minX = (int)Mathf.Max(0, centerGridPos.x - rangeInGridUnits);
		int maxX = ((int)Mathf.Min(centerGridPos.x + rangeInGridUnits, height-1));
		int minY = (int)Mathf.Max(0, centerGridPos.y - rangeInGridUnits);
		int maxY = ((int)Mathf.Min(centerGridPos.y + rangeInGridUnits, height-1));
		// Iterate over the cells within the range
		for (int gridX = minX; gridX <= maxX; gridX++)
		{
			for (int gridY = minY; gridY <= maxY; gridY++)
			{
				// Get the cell at the current grid coordinates
				int index = CalculateFlatIndex(gridX, gridY);
				Cell cell = cells[index];

				// Calculate the distance between the cell and the center position
				float distance = Vector2.Distance(wordPosition, cell.worldPos);

				// Check if the cell is within range
				if (distance - range <= 0)
				{
					inRange.Add(cell.flatIndex);
				}
			}
		}
		return inRange;
	}
	/// <summary>
	/// Gets all neighbour indexes of cell at 
	/// </summary>
	/// <param name="from">From cell grid position</param>
	/// <param name="neighbors">passed in neighbour array</param>
	/// <returns>Neighbour Count</returns>
	public int GetNeighbors(int2 from, Cell[] neighbors)
	{
		int validNeighbors = 0;

		for (int i = 0; i < neighbors.Length; i++)
		{
			int neighborId = GetCellInDirection(from, Directions.directions[i]);
			if (neighborId != -1)
			{
				neighbors[validNeighbors] = cells[neighborId];
				validNeighbors++;
			}
		}
		return validNeighbors;
	}
	/// <summary>
	/// Gets a cell index from provided grid index in direction
	/// </summary>
	/// <param name="orignPos">grid index of origin cell</param>
	/// <param name="direction">grid direction</param>
	/// <returns></returns>
	public int GetCellInDirection(int2 orignPos, int2 direction)
	{
		int2 finalPos = orignPos + direction;

		if (finalPos.x >= 0 && finalPos.x < width && finalPos.y >= 0 && finalPos.y < height)
		{
			return CalculateFlatIndex(finalPos.x, finalPos.y);
		}
		return -1;
	}

	/// <summary>
	/// Gets all cells on edge in target direction and saves them to provided array. Only works for cardinal directions
	/// </summary>
	/// <param name="dirIndex"> index in <see cref="Directions.directions"/></param>
	/// <param name="edgeCells">arry to write into</param>
	public void GetEdgeCells(int dirIndex, Cell[] edgeCells)
    {
		if(dirIndex > 3)
        {
			Debug.LogWarning("Unsupported direction");
			return;
        }
		switch(dirIndex)
        {
			case 0:
                for (int i = 0; i < width; i++)
                {
					int topRowIndex = height - 1;
					edgeCells[i] = cells[CalculateFlatIndex(i, topRowIndex)];
                }
				break;
			case 1:
				for (int i = 0; i < width; i++)
				{
					int bottomIndex = 0;
					edgeCells[i] = cells[CalculateFlatIndex(i, bottomIndex)];
				}
				break;
			case 2:
				for (int i = 0; i < height; i++)
				{
					int rightmostIndex = width - 1;
					edgeCells[i] = cells[CalculateFlatIndex(rightmostIndex, i)];
				}
				break;
			case 3:
				for (int i = 0; i < height; i++)
				{
					int leftmostIndex = 0;
					edgeCells[i] = cells[CalculateFlatIndex(leftmostIndex, i)];
				}
				break;
			default:
				break;
        }
    }
	/// <summary>
	/// Draw cell gizmos. Call from OnDrawGizmos
	/// </summary>
	/// <param name="color">gizmos color</param>
	public void Draw(Color color)
    {
		Gizmos.color = color;
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Vector3 center = new Vector3(cellDiameter * x + cellRadius, cellDiameter * y + cellRadius, 0);
				Vector3 size = Vector3.one * cellDiameter;
				Gizmos.DrawWireCube(center, size);
			}
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;

public class Sector
{
    public Grid grid;
    public FlowField flowField;
    public Vector2 center;
    public SectorPortal[] portalCells = new SectorPortal[4];
    public Cell targetPortalCell;
    public int index;

    public Sector(float _cellRadius, int2 _gridSize, Vector2 center, int flatIndex)
    {
        this.index = flatIndex;
        this.center = center;
        Vector2 bottomLeftPos = center - new Vector2(_gridSize.x * _cellRadius, _gridSize.y * _cellRadius);
        grid = new Grid(_cellRadius, _gridSize, bottomLeftPos);
        flowField = new FlowField(grid);
    }

    /// <summary>
    /// Calculates Flowfield to another sector.
    /// </summary>
    /// <param name="dirIndex"> index in <see cref="Directions.directions"/> Should be between 0 and 3</param>
    /// <returns>Handle to finish job later</returns>
    public JobHandle CalculateFlowfieldToPortal(int dirIndex)
    {
        //Calmp to only cardinal directions.
        if(dirIndex > 3)
        {
            dirIndex -= 4;
        }
        targetPortalCell = portalCells[dirIndex].fromCell;
        return flowField.CalculateFlowFieldToTarget(targetPortalCell);
    }
    /// <summary>
    /// CalculateFlowfieldToPortal and finish immediately
    /// </summary>
    public void CalculateFlowToPortalImmediately(int dirIndex)
    {
        targetPortalCell = portalCells[dirIndex].fromCell;
        FinishPathToPortal( flowField.CalculateFlowFieldToTarget(targetPortalCell),dirIndex);
    }
    /// <summary>
    /// Finish CalculateFlowfieldToPortal job and set portal cell direction to the next sector
    /// </summary>
    /// <param name="handle">job handle</param>
    /// <param name="dirIndex"><see cref=" Directions.directions"/> index</param>
    public void FinishPathToPortal(JobHandle handle, int dirIndex)
    {
        flowField.FinishFlowFieldJob(handle);

        targetPortalCell.bestDirectionIndex = dirIndex;
        targetPortalCell.bestCost = 1;
        grid.cells[targetPortalCell.flatIndex] = targetPortalCell;
    }
}
public class SectorPortal
{
    public int fromIndex;
    public int toIndex;
    public Cell fromCell;

    public SectorPortal(int from, int to, Cell cell)
    {
        this.fromIndex = from;
        this.toIndex = to;
        this.fromCell = cell;
    }
}
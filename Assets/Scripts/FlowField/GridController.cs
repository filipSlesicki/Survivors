using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;

public class GridController : MonoBehaviour
{
	public int2 gridSize;
	public float cellRadius = 0.5f;
	public static FlowField curFlowField;
	public FlowFieldTarget target;
	public LayerCosts moveCosts;
	public UnityEngine.Events.UnityEvent OnFlowfieldChanged;
	JobHandle jobHandle;
	private void InitializeFlowField()
	{
		curFlowField = new FlowField(cellRadius, gridSize);
		curFlowField.CreateGrid();
		curFlowField.CreateCostField(moveCosts);
	}

	private void Awake()
	{
		InitializeFlowField();
	}

	private void FixedUpdate()
	{
		Cell targetCell = curFlowField.GetCellFromWorldPos(target.trans.position);
		int currentTargetPos = targetCell.index;
		if (target.lastGridIndex != currentTargetPos)
		{
			target.lastGridIndex = currentTargetPos;
			if (jobRunning)
			{
				Debug.Log("still running last update");
				return;
			}
			jobRunning = true;
			jobHandle = curFlowField.CalculateFlowFieldToTarget(targetCell);
			//StartCoroutine(UpdateFlowFieldCor(targetCell));
		}
	}
	bool jobRunning;
	int frameRun = 0;
	private void LateUpdate()
	{
		if (jobRunning)
		{
			frameRun++;
			if(frameRun > 2)
			{
				jobRunning = false;
				frameRun = 0;
				curFlowField.FinishFlowFieldJob(jobHandle);
				OnFlowfieldChanged?.Invoke();
			}

		}
	}



	private void OnDisable()
	{
		if (!jobHandle.IsCompleted)
		{
			curFlowField.CleanJobHandle(jobHandle);
		}

		curFlowField.OnDestroy();
	}
}

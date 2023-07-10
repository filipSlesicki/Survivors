using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
public static class Directions
{
	public static readonly int2[] directions = new int2[]
{
		new int2(0,1),		//North
		new int2(0,-1),		//South
		new int2(1,0),		//East
		new int2(-1,0),		//West
		new int2(1,1),		//NorthEast 
		new int2(-1,1),		//NorthWest
		new int2(1,-1),		//SouthEast
		new int2(-1,-1),    //SouthWest
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

	public static int GetOppositeDirection(int dirIndex)
    {
        switch (dirIndex)
        {
			case 0:
				return 1;
			case 1:
				return 0;
			case 2:
				return 3;
			case 3:
				return 2;
			case 4:
				return 7;
			case 5:
				return 6;
			case 6:
				return 5;
			case 7:
				return 4;
			default:
				return -1;
        }
    }

	public enum Direction
    {
		None = -1,
		North,
		South,
		East,
		West,
		NorthEast,
		NorthWest,
		SouthEast,
		SouthWest
    }
}

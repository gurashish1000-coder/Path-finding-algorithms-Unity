using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class to A*, RRT, and Djikstra
/// </summary>
public abstract class Pathfinder : MonoBehaviour
{
	public Transform source, target;
	public MapMaker map;

	protected bool[,] walkable;
	protected Cell sourceC, targetC;

	protected Vector2 sourceCell;
	protected Vector2 targetCell;

	protected Cell[,] cells;

	protected int iterations = 0;


	// Start is called before the first frame update
	public virtual void Start()
	{
		map = FindObjectOfType<MapMaker>();
	}

	protected void markPath()
	{
		List<Cell> cs = new List<Cell>();
		Cell c = targetC;
		while (c != sourceC)
		{
			cs.Add(c);
			c.ChangeColour(Color.black);
			c = c.prev;
		}
		iterations = 0;
	}

	[ContextMenu("Reset")]
	protected void Reset()
	{
		map.Reset();
		iterations = 0;
	}

	public abstract void Calculate();

	protected int dist(Cell A, Cell B)
	{
		int deltaX = A.x - B.x;
		deltaX = Mathf.Abs(deltaX);
		int deltaY = A.y - B.y;
		deltaY = Mathf.Abs(deltaY);

		if (deltaX > deltaY)
		{
			return 14 * deltaY + 10 * deltaX;
		}
		else
		{
			return 14 * deltaX + 10 * deltaY;
		}
	}

	protected List<Cell> WalkableNeighbours(Cell c)
	{
		List<Cell> neighbours = new List<Cell>();

		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				int xx = c.x + i;
				int yy = c.y + j;
				if (i == 0 && j == 0)
				{
					// this is this cell
					continue;
				}
				if (xx > 0 && yy > 0 && xx < map.width() && yy < map.height() && walkable[xx, yy])
				{
					neighbours.Add(cells[xx, yy]);
				}
			}
		}

		return neighbours;
	}

	[ContextMenu("Calculate Source and Target")]
	protected void closest()
	{
		cells = map.cells;
		float bestS = Mathf.Infinity;
		float bestT = Mathf.Infinity;
		foreach (Cell c in cells)
		{
			float sd = Vector3.Distance(c.gameObject.transform.position, source.position);
			float td = Vector3.Distance(c.gameObject.transform.position, target.position);

			if (sd < bestS)
			{
				bestS = sd;
				sourceC = c;
			}
			if (td < bestT)
			{
				bestT = td;
				targetC = c;
			}
		}

		sourceC.ChangeColour(Color.blue);
		targetC.ChangeColour(Color.cyan);
	}
}
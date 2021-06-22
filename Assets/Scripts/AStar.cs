using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : Pathfinder
{
	public override void Start()
	{
		base.Start();
	}

	[ContextMenu("Run A Star")]
	public override void Calculate() {
		walkable = map.walkable;
		closest();
		List<Cell> open = new List<Cell>();
		Dictionary<Cell, int> closed = new Dictionary<Cell, int>();

		open.Add(sourceC);

		while (open.Count > 0)
		{
			iterations++;
			Cell c = open[0];
			// get lowest f cost
			for (int i = 1; i < open.Count; i++)
			{
				if (open[i].FCost() <= c.FCost() && open[i].hCost < c.hCost)
				{
					c = open[i];
				}
			}

			open.Remove(c);
			// add to closed for fast look ups
			// the value does not matter
			closed[c] = 7;

			c.ChangeColour(Color.yellow);

			if (c == targetC)
			{
				print("found path in " + iterations + " iterations");
				iterations = 0;
				markPath();
				return;
			}

			foreach (Cell cc in WalkableNeighbours(c))
			{
				bool inOpen = open.Contains(cc);

				if (closed.ContainsKey(cc))
				{
					continue;
				}
				int new_g = cc.gCost + dist(c, cc);
				// if this is a new node or updating gcost
				if (new_g < cc.gCost || !inOpen)
				{
					cc.gCost = new_g;
					cc.hCost = dist(cc, targetC);
					cc.prev = c;

					if (!inOpen)
					{
						open.Add(cc);
					}
				}
			}
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Really should make a <pathfinder> parent class to clean up the code but
/// we ran out of time
/// </summary>

public class RRT : Pathfinder
{
	public Color visitedColour;

	Color bad;
	Color good;

	public override void Start()
	{
		base.Start();

		//closest();
	}

	[ContextMenu("Run RRT")]
	public override void Calculate() {
		bad = map.noWalk;
		good = map.Walk;
		walkable = map.walkable;
		closest();
		List<Cell> visited = new List<Cell>();
		System.Random r = new System.Random();
		visited.Add(sourceC);
		//print("source " + sourceC.x + " " + sourceC.y);

		for (int i = 0; i < 300; i++)
		{
			iterations++;
			bool validRandom = false;
			Cell randCell = targetC;
			while (!validRandom)
			{
				int x = r.Next(0, map.width());
				int y = r.Next(0, map.height());

				Cell random = cells[x, y];
				if (random.colour != bad || random == targetC)
				{
					randCell = random;
					validRandom = true;
				}
			}

			//print("Random " + randCell.x + " " + randCell.y);
			randCell.ChangeColour(Color.green);
			Cell nearest = NearestInTree(visited, randCell);
			//print("nearest " + nearest.x + " " + nearest.y);

			Cell best_neighbour = NearestNeighbour(nearest, randCell);
			//print("best neighbour " + best_neighbour.x + " " + best_neighbour.y);
			
			best_neighbour.prev = nearest;
			visited.Add(best_neighbour);
			best_neighbour.ChangeColour(visitedColour);
			if (best_neighbour == targetC)
			{
				print("Reached goal in " + iterations + " iterations");
				markPath();
				return;
			}
		}

	}

	private Cell NearestInTree(List<Cell> visited, Cell newc)
	{
		Cell closestTree = null;
		float best = Mathf.Infinity;
		foreach (Cell c in visited)
		{
			float d = Vector3.Distance(c.gameObject.transform.position, newc.transform.position);
			if (d < best)
			{
				best = d;
				closestTree = c;
			}
		}

		return closestTree;
	}

	private Cell NearestNeighbour(Cell origin, Cell newc)
	{
		List<Cell> neighbours = WalkableNeighbours(origin);
		float best = 1000f;
		Cell closest = origin;

		foreach (Cell c in neighbours)
		{
			float d = Vector3.Distance(c.transform.position, newc.transform.position);
			//print(c.x + " " + c.y + " " + d + " best: " + best);
			if (d < best)
			{
				best = d;
				//print("improving best " + best);
				closest = c;
			}
		}

		return closest;
	}
}

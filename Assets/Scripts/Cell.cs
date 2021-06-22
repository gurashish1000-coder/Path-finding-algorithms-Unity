using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
	public Color colour;
	public int x;
	public int y;

	public int gCost;
	public int hCost;

	public Cell prev;

    // Start is called before the first frame update
    void Start()
    {
		this.gameObject.GetComponent<Renderer>().material.color = colour;
	}

	public void ChangeColour(Color newColour)
	{
		colour = newColour;
		this.gameObject.GetComponent<Renderer>().material.color = newColour;
	}

	public int FCost()
	{
		return gCost + hCost;
	}
}

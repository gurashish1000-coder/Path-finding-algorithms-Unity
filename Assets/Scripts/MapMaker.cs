using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
	public GameObject cellObj;
	public float threshhold;

	public bool[,] walkable;
	public Cell[,] cells;

	public Color Walk;
	public Color noWalk;

	public Texture2D noiseTex;
	Color[] pix;
	Renderer rend;

	// The origin of the sampled area in the plane.
	public float xOrg;
	public float yOrg;

	private int pixWidth;
	private int pixHeight;

	// The number of cycles of the basic noise pattern that are repeated
	// over the width and height of the texture.
	public float scale = 0.1F;
	
	// Start is called before the first frame update
	void Start()
    {
		pixWidth = noiseTex.width;
		pixHeight = noiseTex.height;

		rend = GetComponent<Renderer>();
		cells = new Cell[pixWidth, pixHeight];
		
		// Set up the texture and a Color array to hold pixels during processing.
	
		pix = new Color[noiseTex.width * noiseTex.height];
		rend.material.mainTexture = noiseTex;

		walkable = new bool[pixWidth, pixHeight];

		for (int i = 0; i < pixWidth; i++)
		{
			for (int j = 0; j < pixHeight; j++)
			{
				Vector3 pos = new Vector3(i, 0, j);
				cells[i, j] = Instantiate(cellObj, pos, Quaternion.Euler(90, 0,0 )).GetComponent<Cell>();
				cells[i, j].x = i;
				cells[i, j].y = j;
			}
		}

		UpdateMap();
	}

	// Update is called once per frame
	void UpdateMap()
    {

		Vector2 size = new Vector2(pixWidth, pixHeight);

		for (int i = 0; i < size.x; i++)
		{
			for (int j = 0; j < size.y; j++)
			{
				float z = noiseTex.GetPixel(i, j).grayscale;
				//print("i: "+ i);
				//print("j: "+ j);
				//print("z: "+ z);
				if (z > threshhold)
				{
					walkable[i, j] = false;
					cells[i, j].ChangeColour(noWalk);
				}
				else
				{
					walkable[i, j] = true;
					cells[i, j].ChangeColour(Walk);
				}
			}
		}
		 
    }

	public int width()
	{
		return pixWidth;
	}

	public int height()
	{
		return pixHeight;
	}

	public void Reset()
	{
		UpdateMap();
	}


}

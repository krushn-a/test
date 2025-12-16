using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int width = 10;
    [SerializeField] private int height = 10;
    [SerializeField] private Material lightMaterial;
    [SerializeField] private Material darkMaterial;
    [SerializeField] private Transform tileParent;

    [SerializeField] private TextMeshProUGUI posX;
    [SerializeField] private TextMeshProUGUI posZ;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        generateGrid();
    }

    private void Update()
    {
        //Getting the mouse position
        Vector3 mousePos = Mouse.current.position.ReadValue();
        
        //Directing a ray from camera to the mouse position
        Ray ray = cam.ScreenPointToRay(mousePos);

        RaycastHit hit;

        //Performing a raycast to get the data about collided object
        if (Physics.Raycast(ray, out hit)) 
        {
            TileData data = hit.collider.GetComponent<TileData>();

            if (data != null) 
            {
                posX.text = $"{data.posX}";
                posZ.text = $"{data.posZ}";
            }
        }
    }

    private void generateGrid() 
    {
        for (int x = 0; x < width; x++) 
        {
            for (int z = 0; z < height; z++) 
            {
                //Setting the spawn position
                Vector3 spawPos = new Vector3(x, 0, z);

                //Instanting the cubes to form the map
                GameObject tiles = Instantiate(tilePrefab, spawPos, Quaternion.identity);

                TileData data = tiles.GetComponent<TileData>();

                if(data != null) 
                {
                    data.Init(x, z);
                }

                if (tileParent != null) 
                {
                    tiles.transform.parent = tileParent;
                }

                //Assigning a material to distinguish between tiles
                Renderer rend = tiles.GetComponent<Renderer>();
                if ((x + z) % 2 == 0)
                {
                    rend.material = lightMaterial;
                }
                else
                {
                    rend.material = darkMaterial;
                }
            }
        }
    }
}

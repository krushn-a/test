using UnityEngine;

public class TileData : MonoBehaviour
{
    //to store the grid coordinates
    public int posX;
    public int posZ;


    public void Init(int gridX, int gridZ) 
    {
        posX = gridX;
        posZ = gridZ;
    }
}

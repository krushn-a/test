using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public LevelData levelData;

    [SerializeField] private GameObject obstaclePrefab;
    private void Start()
    {
        generateObstacles();
    }

    private void generateObstacles() 
    {
        if (levelData == null) 
        {
            Debug.Log("No levelData assigned");
            return;
        }

        for (int x = 0; x < 10; x++) 
        {
            for (int z = 0; z < 10; z++) 
            {
                //calculating the index from the list 
                int index = x + (z * 10);

                //checking if the tile is blocked or not
                if (levelData.obstacles[index] == true) 
                {
                    //calculate the position of the tile
                    Vector3 obstaclePos = new Vector3(x, 0.52f, z);

                    //spawing the sphere
                    Instantiate(obstaclePrefab, obstaclePos, Quaternion.identity);
                }
            }
        }
    }
}

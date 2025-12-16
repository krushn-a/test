using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerCotroller : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    public LevelData levelData;
    private Camera cam;

    private bool isMoving = false;

    [SerializeField] private EnemyController enemyAI;

    private void Start()
    {
        cam = Camera.main;
    }

    protected virtual void Update()
    {
        //if player is moving dont do anything
        if (isMoving) return;

        if (Mouse.current.leftButton.wasPressedThisFrame) 
        {
            Debug.Log("move");
            checkMouseClick();
        }
    }

    private void checkMouseClick() 
    {
        //Getting the mouse position
        Vector3 mousePos = Mouse.current.position.ReadValue();

        //Directing a ray from camera to the mouse position
        Ray ray = cam.ScreenPointToRay(mousePos);

        RaycastHit hit;

        //Performing a raycast to get the data about collided object
        if (Physics.Raycast(ray, out hit))
        {
            TileData tile = hit.collider.GetComponent<TileData>();

            if (tile != null)
            {
                int startX = Mathf.RoundToInt(transform.position.x);
                int startZ = Mathf.RoundToInt(transform.position.z);

                Vector2Int startPos = new Vector2Int(startX, startZ);


                Vector2Int targetPos = new Vector2Int(tile.posX, tile.posZ);

                //find the path
                List<Vector2Int> path = findPath(startPos, targetPos);

                if(path != null) 
                {
                    StartCoroutine(pathFollow(path));
                }
            }
        }
    }


    //to get possible movements around a center tile
    protected List<Vector2Int> getNeighbours(Vector2Int center) 
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();

        //storing the direction to where we can move
        Vector2Int[] directions = {
            new Vector2Int(0, -1),
            new Vector2Int(-1,0),
            new Vector2Int(1,0),
            new Vector2Int(0,1),
        };

        foreach (Vector2Int dir in directions) 
        {
            Vector2Int nextPos = center + dir;

            //checking if the nextPos is within the bounds or not that [0,10) for both x and y
            if (nextPos.x >= 0 && nextPos.x < 10 && nextPos.y >= 0 && nextPos.y < 10) 
            {
                int index = nextPos.x + (nextPos.y * 10);

                if (levelData.obstacles[index] == false) 
                {
                    neighbours.Add(nextPos);
                }
            }
        }

        return neighbours;
    }

    //returns a list of steps required to go from start to end
    protected List<Vector2Int> findPath(Vector2Int startPos, Vector2Int targetPos) 
    {

        //a queue to store the tiles we need to check
        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        queue.Enqueue(startPos);

        //A dictionary to remember the path
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        cameFrom[startPos] = startPos;

        bool foundtarget = false;

        while (queue.Count > 0) 
        {
            Vector2Int current = queue.Dequeue();

            if(current == targetPos) 
            {
                foundtarget = true;
                break;
            }

            foreach(Vector2Int next in getNeighbours(current)) 
            {
                //if we havent visited the tile yet
                if (!cameFrom.ContainsKey(next)) 
                {
                    queue.Enqueue(next);
                    cameFrom[next] = current; //setting where i came from
                }
            }
        }

        //if never found the target return null
        if (!foundtarget) return null;

        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currentStep = targetPos;

        //tracing back the path where we came from
        while(currentStep != startPos) 
        {
            path.Add(currentStep);
            currentStep = cameFrom[currentStep];
        }

        path.Reverse();

        return path;
    }


    protected IEnumerator pathFollow(List<Vector2Int> path) 
    {
        isMoving = true; //blocking input so we cant click while walking

        //looping thorugh each step in the path we found
        foreach (Vector2Int step in path) 
        {
            Vector3 targetPosition = new Vector3(step.x, 1.0f, step.y);

            //Keep moving as long as we are far away from target file
            while(Vector3.Distance(transform.position,targetPosition) > 0.05f) 
            {
                //this calculates a tiny step closer to the target
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                //wait for sometime(one frame)
                yield return null;

            }

            transform.position = targetPosition;
        }

        onMoveComplete();
    }

    public void endTurn() 
    {
        isMoving = false;
    }

    protected virtual void onMoveComplete() 
    {
        if(enemyAI != null) 
        {
            enemyAI.calculateMove();
        }

        else 
        {
            isMoving = false;
        }
    }
}

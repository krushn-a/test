using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyController : PlayerCotroller, IAI
{
    public PlayerCotroller targetPlayer;

    public Vector3 enemyStartPos;

    private void Start()
    {
        transform.position = new Vector3(enemyStartPos.x, 1.0f, enemyStartPos.z);
    }

    protected override void Update()
    {
    }
    public void calculateMove()
    {
        if (targetPlayer == null) 
        {
            targetPlayer.endTurn();
            return;
        }

        //getting our Enemy position
        Vector2Int myPos = GetGridPosition(transform.position);

        //getting the players position
        Vector2Int getPlayerPos = GetGridPosition(targetPlayer.transform.position);

        //getting all the adjacent tiles around the player
        List<Vector2Int> neighbours = getNeighbours(getPlayerPos);

        if(neighbours.Count == 0) 
        {
            targetPlayer.endTurn();
            return;
        }

        //just picking the first tile we got
        Vector2Int targetTile = neighbours[0];

        //finding the path to the adjacent tile
        List<Vector2Int> enemyPath = findPath(myPos, targetTile);

        if(enemyPath != null && enemyPath.Count > 0) 
        {
            StartCoroutine(pathFollow(enemyPath));
        }
        else 
        {
            targetPlayer.endTurn();
        }
    }


    //Converting world coordinate to grid coordinate
    private Vector2Int GetGridPosition(Vector3 enemyWorldPos) 
    {
        int x = Mathf.RoundToInt(enemyWorldPos.x);
        int z = Mathf.RoundToInt(enemyWorldPos.z);

        return new Vector2Int(x, z);
    }

    protected override void onMoveComplete()
    {
        //to notify the player that enemy finished its moving
        if (targetPlayer != null) targetPlayer.endTurn();
    }
}

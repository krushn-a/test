using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Level data")]
public class LevelData : ScriptableObject
{
    public List<bool> obstacles = new List<bool>();

    public void clearData() 
    {
        obstacles.Clear();
        for (int i = 0; i < 100; i++) 
        {
            obstacles.Add(false);
        }
    }
}

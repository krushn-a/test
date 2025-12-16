using UnityEngine;
using UnityEditor;

public class ObstacleTool : EditorWindow
{
    public LevelData levelData;

    [MenuItem("Tools/Obstacle Generator")]
    public static void showWindow() 
    {
        GetWindow<ObstacleTool>("Obstacle Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Editor", EditorStyles.boldLabel);

        //creating a field to drag and drop levelData
        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);

        if (levelData == null)
        {
            GUILayout.Label("Please assign a level data field");
            return;
        }

        for (int z = 9; z >= 0; z--) 
        {
            //to start horizontal row of buttons
            GUILayout.BeginHorizontal();

            for (int x = 0; x < 10; x++) 
            {
                int index = x + (z * 10);

                if (index < levelData.obstacles.Count) 
                {
                    DrawButton(index);
                }
            }

            //End horizontal row
            GUILayout.EndHorizontal();
        }
    }

    private void DrawButton(int index) 
    {
        bool isBlocked = levelData.obstacles[index];

        GUI.backgroundColor = isBlocked ? Color.red : Color.green;

        if (GUILayout.Button(isBlocked ? "X" : "O", GUILayout.Width(30), GUILayout.Height(30))) 
        {
            levelData.obstacles[index] = !isBlocked;

            EditorUtility.SetDirty(levelData);
        }

        GUI.backgroundColor = Color.white;
    }
}

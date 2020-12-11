using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, AStar.Level
{
    [SerializeField] private GameObject mur;
    [SerializeField] private PlayerController player;
    private PlayerController spawnedPlayer;
    [SerializeField] private MazeGenerator mazeGenerator;
    [SerializeField] private ProjectilesSpawn projectiles;
    private int[,] maze;
    public bool toggleDiagonal = false;

    private float timer = 0f;
    public bool isPaused = false;

    public GameObject chest;
    public bool isChestActive = false;
    public bool gameOver = false;


    private void Start()
    {
        maze = mazeGenerator.CreateMaze();
        createMaze3D(maze,mur);
        SpawnPlayer(maze, player);
        SpawnChest(maze, chest);
        isChestActive = true;
    }

    private void SpawnPlayer(int[,] maze, PlayerController player)
    {
        (int, int) pos = StartingPoint(maze);
        spawnedPlayer = Instantiate(player, FromMazeTo3D(pos) + Vector3.up, Quaternion.identity);
    }

    private void createMaze3D(int[,] maze, GameObject mur)
    {
        int finalWidth = maze.GetLength(0);
        int finalHeight = maze.GetLength(1);

        //affichage :
        for (int y = 0; y < finalHeight; y++)
        {
            for (int x = 0; x < finalWidth; x++)
            {
                if (maze[x, y] == 0)
                {
                    GameObject go = Instantiate(mur, FromMazeTo3D((x, y)), Quaternion.identity);
                    GameObject Go = Instantiate(mur, FromMazeTo3D((x, y))+Vector3.up, Quaternion.identity);
                }
            }
        }
    }
     
    (int, int) StartingPoint(int[,] maze)
    {
        (int, int) startPoint;
        do
        {
            startPoint = (Random.Range(0, maze.GetLength(0)), Random.Range(0, maze.GetLength(1)));
        } while (!MazeGenerator.IsFree(maze, startPoint));
        return startPoint;
    }

    private static Vector3 FromMazeTo3D((int, int) mazeCoordinates)
    {
        return new Vector3(mazeCoordinates.Item1 + 0.5f, 0.5f, -(mazeCoordinates.Item2 + 0.5f));
    }

    private static (int, int) From3DMaze(Vector3 coordinates3d)
    {
        int x = (int)coordinates3d.x;
        int y = -(int)coordinates3d.z;
        return (x, y);
    }

    private static List<Vector3> FromMazeTo3D(List<(int, int)> path)
    {
        List<Vector3> ret = new List<Vector3>();
        foreach (var onePoint in path)
        {
            ret.Add(FromMazeTo3D(onePoint));
        }
        return ret;
    }

    public void MovePlayer(Vector3 Point)
    {
        // 1 - Convert 3D coordinates to Maze coordinates => finalMazePos
        (int, int) endPos = From3DMaze(Point);

        // 2 - Apply Dijkstra to find the path between playerPos and finalMazePos
        //List<(int, int)> shortestPath = Dijkstra.Apply(maze, From3DMaze(spawnedPlayer.transform.position), endPos);
        List<(int, int)> shortestPath = AStar.Apply(this, From3DMaze(spawnedPlayer.transform.position), endPos,toggleDiagonal);

        // 3 - Ask the player to follow this path
        spawnedPlayer.Move(FromMazeTo3D(shortestPath));
    }

    public bool IsFree((int x, int y) pos)
    {
        return MazeGenerator.IsFree(maze, pos);
    }

    public double Cost((int x, int y) from, (int x, int y) to)
    {
        int costx = to.x - from.x;
        int costy = to.y - from.y;
        return Mathf.Sqrt(costx * costx + costy * costy);
    }

    public double Heuristic((int x, int y) from, (int x, int y) to)
    {
        int costx = to.x - from.x;
        int costy = to.y - from.y;
        return Mathf.Sqrt(costx * costx + costy * costy);
    }


    private void SpawnChest(int[,] maze, GameObject chest)
    {
            (int, int) pos = StartingPoint(maze);
            GameObject Chest = Instantiate(chest, FromMazeTo3D(pos) + Vector3.up, Quaternion.identity);
    }

    private void Update()
    {
        float spawnAtRandomTime = 3f;
        if (gameOver == false)
        {
            if (isChestActive == false && isPaused == false)
            {
                SpawnChest(maze, chest);
                isChestActive = true;
            }

            if (isPaused == false)
            {
                timer += Time.deltaTime;
            }
            if (timer >= spawnAtRandomTime)
            {
                spawnAtRandomTime = Random.Range(.2f, 4f);
                timer = 0f;
                projectiles.SpawnProjectile();
            }
        }
        else
        {

        }

    }
    
}

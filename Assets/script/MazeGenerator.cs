using Packages.Rider.Editor.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditorInternal;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public GameObject cube; 
    public int height = 10;
    public int width = 10;
    int roomSize = 3;
    


    void Start()
    {
        //SpawnCubes(0,100,0,0,0);
        //CreateMaze();
    }

    void Update()
    {
        
    }

    public int[,] CreateMaze()
    {

        int[,] maze;

        int finalWidth = 2 * width + 1;
        int finalHeight = 2 * height + 1;
        maze = new int[finalWidth, finalHeight];

        int startPointX;
        int startPointY;
        startPointX = UnityEngine.Random.Range(0, width) * 2 + 1;
        startPointY = UnityEngine.Random.Range(0, height) * 2 + 1;
        //Debug.Log(startPointX+" "+ startPointY);

        //mise en place de la grille :
        for (int y = 0; y < finalHeight; y++)
        {
            for (int x = 0; x < finalWidth; x++)
            {
                if (x % 2 == 0 || y % 2 == 0)
                {
                    maze[x, y] = 0;
                }
                else
                {
                    maze[x, y] = 1;
                }
            }
        }
        createMaze(maze, (startPointX, startPointY));
        AddRoom(maze, (startPointX, startPointY), width, height, roomSize);
        displayInConsole(finalWidth, finalHeight, maze);

        return maze;
    }

    private void displayInConsole(int finalWidth, int finalHeight, int[,] maze)
    {
        string str = "";

        //affichage :
        for (int y = 0; y < finalHeight; y++)
        {
            for (int x = 0; x < finalWidth; x++)
            {
                if (maze[x, y] == 0)
                {
                    str += "█";
                }
                else if (maze[x, y] == 1)
                {
                    str += "▓";
                }
                else if (maze[x, y] == 2)
                {
                    str += "░";
                }
                else if (maze[x, y] == 3)
                {
                    str += "▒";
                }
            }
            str += "\n";
        }
        Debug.Log(str);

    }

    private void createMaze(int[,] maze, (int , int ) cell)
    {
       /*3.0) La marquer comme explorée (= lui mettre la valeur 2)
         3.1) Tant qu’il y une cellule non visitée adjacente (Sinon c’est un cul de sac )
         3.1.1) Choisir une direction au hasard vers une cellule non visitée
         3.1.2) Casser le mur vers la nouvelle cellule
         3.1.3) Explorer depuis la nouvelle cellule*/

        maze[cell.Item1, cell.Item2] = 2;
        while (true)
        {
            List<(int, int)> cellsToExplore = GetCellsToExplore(maze, cell);
            if (cellsToExplore.Count == 0)
                break;

            int rand = UnityEngine.Random.Range(0, cellsToExplore.Count);
            (int,int) randomCell = cellsToExplore[rand];
            BreakWall(maze,cell, randomCell);
            createMaze(maze, randomCell);
        }
    }

    private void AddRoom(int[,] maze, (int, int) cell, int width, int height, int roomSize)
    {
        int nbRoom = UnityEngine.Random.Range(1,5);
        for (int i = 0; i < nbRoom; i++)
        {
           
            int startX = UnityEngine.Random.Range(0, (width * 2 + 1));
            int startY = UnityEngine.Random.Range(0, (height * 2 + 1));
            

            for (int y = startY; y < startY + roomSize; y++)
            {
                for (int x = startX; x < startX + roomSize; x++)
                {
                    if (x > 0 && y > 0 && x < (maze.GetLength(0)-1) && y < (maze.GetLength(1)-1))
                    {
                        maze[x, y] = 3;
                    }
                }
            }
        }
    }

    private void BreakWall(int[,] maze, (int, int) cell, (int, int) randomCell)
    {
        int xWall = (cell.Item1 + randomCell.Item1) / 2;
        int yWall = (cell.Item2 + randomCell.Item2) / 2;
        maze[xWall, yWall] = 2;
    }

    private List<(int, int)> GetCellsToExplore(int[,] maze, (int, int) cell)
    {
        List<(int, int)> cells = new List<(int, int)>();
        int leftX = cell.Item1 - 2;
        int rightX = cell.Item1 + 2;
        int topY = cell.Item2 - 2;
        int bottomY = cell.Item2 + 2;

        // left cell
        if (leftX >= 0 && maze[leftX, cell.Item2] == 1)
        {
            cells.Add((leftX, cell.Item2));
        }
        // right cell
        if (rightX < maze.GetLength(0) && maze[rightX, cell.Item2] == 1)
        {
            cells.Add((rightX, cell.Item2));
        }
        // top cell
        if (topY >= 0 && maze[cell.Item1, topY] == 1)
        {
            cells.Add((cell.Item1, topY));
        }
        // bottom cell
        if (bottomY < maze.GetLength(1) && maze[cell.Item1, bottomY] == 1)
        {
            cells.Add((cell.Item1, bottomY));
        }
        return cells;
    }

    void SpawnCubes(int step,int maxStep, int x,int y,int z)
    {
        int axeIncrement = UnityEngine.Random.Range(1, 4);

        GameObject go = Instantiate(cube, new Vector3(x, y, z), Quaternion.identity);
        go.name = "obj " + x + y +  z;



        if (step < maxStep)
        {
            int xTest = 0;
            int yTest = 0;
            int zTest = 0;
            do
            {
                int n = UnityEngine.Random.Range(0, 2);
                if (n == 0)
                    n = -1;

                switch (UnityEngine.Random.Range(1, 4))
                {
                    case 1:
                        xTest = x + n;
                        yTest = y;
                        zTest = z;
                        break;
                    case 2:
                        xTest = x;
                        yTest = y + n;
                        zTest = z;
                        break;
                    case 3:
                        xTest = x;
                        yTest = y;
                        zTest = z + n;
                        break;
                }
            } while (GameObject.Find("obj " + xTest+ yTest+ zTest));

            SpawnCubes(step + 1, maxStep, xTest, yTest, zTest);
        }
    }

    public static bool IsFree(int[,] maze, (int x, int y) pos)
    {
        if (pos.x < 0)
            return false;
        if (pos.x >= maze.GetLength(0))
            return false;
        if (pos.y < 0)
            return false;
        if (pos.y >= maze.GetLength(1))
            return false;
        return maze[pos.x, pos.y] != 0;
    }

}

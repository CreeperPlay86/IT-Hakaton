using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellSpawner : MonoBehaviour
{
    public List<Cell> Cell; // Список префабов клеток с их вероятностями
    public Vector3 CellSize = new Vector3(1, 1, 0);
    public Maze maze;

    private void Start()
    {
        MazeGenerator generator = new MazeGenerator();
        maze = generator.GenerateMaze();

        for (int x = 0; x < maze.cells.GetLength(0); x++)
        {
            for (int y = 0; y < maze.cells.GetLength(1); y++)
            {
                // Выбираем префаб клетки на основании заданной вероятности
                Cell selectedCellPrefab = SelectCellPrefab();

                Cell c = Instantiate(selectedCellPrefab, new Vector3(x * CellSize.x, y * CellSize.y, y * CellSize.z), Quaternion.identity);
                
                c.Wall_Left.SetActive(maze.cells[x, y].WallLeft);
                c.Wall_Down.SetActive(maze.cells[x, y].WallBottom);
            }
        }
    }

    private Cell SelectCellPrefab()
    {
        float totalProbability = Cell.Sum(cp => cp.Probability);
        float randomValue = Random.Range(0, totalProbability);
        float cumulativeProbability = 0f;

        foreach (var cellProbability in Cell)
        {
            cumulativeProbability += cellProbability.Probability;
            if (randomValue < cumulativeProbability)
            {
                return cellProbability.CellPrefab;
            }
        }

        // На случай, если по каким-то причинам не было выбрано ни одной клетки
        return Cell[0].CellPrefab; 
    }
}
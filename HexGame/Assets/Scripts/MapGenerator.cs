using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator:MonoBehaviour
{
    //добавить возможность настраивать ширину и высоту поля через инсперктор +
    //Сменить тип HexCells на двухмерный массив +
    //Create the MapController Class, where will be called the MapCreate() which will return HexCells array. +

    [SerializeField]
    private List<BaseCell> hexCellPrefabs = new List<BaseCell>();
    [SerializeField]
    private GameObject PlayerPrefab;
    [SerializeField]
    private GameController gController;

    public  BaseCell StartCell;
    public BaseCell EndCell;

    public List<BaseCell> HexCells;
    public BaseCell[,] HexCells1;
    public List<CellContent> CellsContent = new List<CellContent>();


    
    private int CheckTypeOfEnemy()
    {
        System.Random rnd = new System.Random();
        return rnd.Next(1, 4); //1-DD, 2-Healer, 3-Tank
    }

    private int CheckTypeOfBonus()
    {
        System.Random rnd = new System.Random();
        return rnd.Next(4, 7); 
    }
    


    private Vector3 GetWorldPosition(int x, int y, float hexSize = 1)
    {
        hexSize /= Mathf.Sqrt(3);

        if (y % 2 == 0)
        {
            return new Vector3(x, 0, y * hexSize * 6 / 4);
        }
        else
        {
            return new Vector3(x + .5f, 0, y * hexSize * 3 / 2);
        }
    }


    int TakeRandomIntOfRange(int stValue, int endValue, int exception = 100000)
    {
        if (exception == 100000)
        {
            System.Random rnd = new System.Random(); 
            return rnd.Next(stValue, endValue);
        }
        else
        {
            System.Random rnd = new System.Random();
            int value = rnd.Next(stValue, endValue);
            while (value == exception)
            {
                value = rnd.Next(stValue, endValue);
            }
            return value;
        } 
    }
  

    List<Vector2Int> GetALLNeighbours(Vector2Int vect, List<Vector2Int> missingCellCoordinates)
    {
        List<Vector2Int>  nList = new List<Vector2Int>();
        if (vect.y % 2 == 0)
        {
            nList.Clear();
            if (!((vect.x - 1) < 0) )
            {
                if (!missingCellCoordinates.Contains(new Vector2Int(vect.x - 1, vect.y)))
                {
                    nList.Add(new Vector2Int(vect.x - 1, vect.y));
                }
            }
            if (!((vect.x - 1) < 0))
            {
                if (!missingCellCoordinates.Contains(new Vector2Int(vect.x - 1, vect.y + 1)))
                {
                    nList.Add(new Vector2Int(vect.x - 1, vect.y + 1));
                }
            }
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x, vect.y + 1)))
                {
                nList.Add(new Vector2Int(vect.x, vect.y + 1));
            }
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x + 1, vect.y)))
                {
                nList.Add(new Vector2Int(vect.x + 1, vect.y));
            }
            if (!((vect.y - 1) < 0))
            {
                if (!missingCellCoordinates.Contains(new Vector2Int(vect.x, vect.y - 1)))
                {
                    nList.Add(new Vector2Int(vect.x, vect.y - 1));
                }
            }
            if (!((vect.x - 1) < 0 | (vect.y - 1) < 0))
            {
                if (!missingCellCoordinates.Contains(new Vector2Int(vect.x - 1, vect.y - 1)))
                {
                    nList.Add(new Vector2Int(vect.x - 1, vect.y - 1));
                }
            }
        }

        else
        {
            nList.Clear();
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x, vect.y + 1)))
            {
                nList.Add(new Vector2Int(vect.x, vect.y + 1));
            }
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x + 1, vect.y + 1)))
            {
                nList.Add(new Vector2Int(vect.x + 1, vect.y + 1));
            }
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x + 1, vect.y)))
            {
                nList.Add(new Vector2Int(vect.x + 1, vect.y));
            }
            if (!((vect.y - 1) < 0))
            {
                if (!missingCellCoordinates.Contains(new Vector2Int(vect.x + 1, vect.y - 1)))
                {
                    nList.Add(new Vector2Int(vect.x + 1, vect.y - 1));
                }
            }
            if (!((vect.y - 1) < 0))
            {
                if (!missingCellCoordinates.Contains(new Vector2Int(vect.x, vect.y - 1)))
                {
                    nList.Add(new Vector2Int(vect.x, vect.y - 1));
                }
            }
            if (!((vect.x - 1) < 0))
            {
                if (!missingCellCoordinates.Contains(new Vector2Int(vect.x - 1, vect.y)))
                {
                    nList.Add(new Vector2Int(vect.x - 1, vect.y));
                }
            }
        }
        return nList;
    }

    List<Vector2Int> GetALLSpawnedNeighbours(Vector2Int vect, List<Vector2Int> missingCellCoordinates, List<Vector2Int> map, List<Vector2Int>deleted)
    {
        var checkNeighbors = GetALLNeighbours(vect, missingCellCoordinates);
        var spawnedNeighbours = new List<Vector2Int>();

                foreach (var neighbour in checkNeighbors)
                {
                    if (map.Contains(neighbour) && !deleted.Contains(neighbour))
                    {
                        spawnedNeighbours.Add(neighbour);
                    }
                }
            
        return spawnedNeighbours;
    }

    private void CheckContent(EmptyCell cell)
    {
        if (cell.CellType == CellType.StartCell || cell.CellType == CellType.EndCell) return;
        System.Random rnd = new System.Random();
        int value = rnd.Next(0, 3); //1 - bonus, 2 - enemy, 0 - empty
        if (value == 1)
        {
            cell.CellType = CellType.BonusCell;//BonusCell
            cell.SetContentPrefab(CellsContent[CheckTypeOfBonus()]);
        }
        else if (value == 2)
        {
            cell.CellType = CellType.EnemyCell;//EnemyCell
            cell.SetContentPrefab(CellsContent[CheckTypeOfEnemy()]);
        }
        else
        {
            cell.CellType = CellType.EmptyCell;//Empty 
        }
    }

    public BaseCell[,] MapGenerate(int rows, int columns, Transform transform, Player player, int lvl)
    {
        //int lvlkoeff = (int)Mathf.Round(lvl / 2);
        //rows = rows + lvlkoeff;
        //columns = columns +lvlkoeff;
        BaseCell cellType;
        var map = new BaseCell[rows, columns];
        var startPoint = new Vector2Int(Mathf.CeilToInt(rows / 2), Mathf.CeilToInt(columns / 2));
        var neighbourCoords = new List<Vector2Int>() { startPoint };
        var lenght = rows * columns;
        var missingCellCoordinates = new List<Vector2Int>();
        int startCellIndex = TakeRandomIntOfRange(0, lenght - 1);
        int endCellIndex = TakeRandomIntOfRange(0, lenght - 1, startCellIndex);
        Debug.Log($"start: {startCellIndex.ToString()} end: {endCellIndex.ToString()}");

        for (int i = 0; i < neighbourCoords.Count; i++)
        {
            var coord = neighbourCoords[i];
            var innerNeighbourCoords = GetALLNeighbours(coord, missingCellCoordinates);// GetNeighboursCoords(coord, rows, columns); 
            /*foreach (var item in innerNeighbourCoords)
            {
                if (item.x >= rows || item.y >= columns)
                    continue;
                if(neighbourCoords.Contains(item)) 
                    continue;
  
                neighbourCoords.Add(item);       
            }*/
            if (startCellIndex == i)
            {
                cellType = StartCell;
            }
            else if (endCellIndex == i)
            {
                cellType = EndCell;
            }
            else
            {
                cellType = GetCellType(coord, missingCellCoordinates, innerNeighbourCoords);
            }

            if (cellType is MissingCell)
            {
                missingCellCoordinates.Add(coord);
            }
            else
            {
                /*var checkCoords = GetALLSpawnedNeighbours(coord, missingCellCoordinates, neighbourCoords, missingCellCoordinates);
                if (checkCoords.Count <1)
                { 
                    continue;
                }*/
                var currentCell = CreateCell(coord, cellType, player);
                currentCell.CellIndex = new Vector2Int(coord.x, coord.y);
                CheckContent((EmptyCell)currentCell);
                map[currentCell.CellIndex.x, currentCell.CellIndex.y] = currentCell;
            }
            // var innerNeighbourCoords = GetALLNeighbours(coord, missingCellCoordinates);// GetNeighboursCoords(coord, rows, columns); 
            foreach (var item in innerNeighbourCoords)
            {
                if (item.x >= rows || item.y >= columns)
                    continue;
                if (neighbourCoords.Contains(item))
                    continue;

                neighbourCoords.Add(item);
            }
            //}
        }
        return map;
    }

    private BaseCell CreateCell(Vector2Int coords, BaseCell cellType, Player player)
    {
        BaseCell cell;
        var v3coords = GetWorldPosition(coords.x, coords.y);
        cell = Instantiate(cellType, v3coords, cellType.transform.rotation, transform);
        cell.CellIndex = new Vector2Int(coords.x, coords.y);
        cell.name = cell.name + cell.CellIndex.ToString();
        HexCells.Add(cell);

        if (cellType == StartCell)
        {
            ((EmptyCell)StartCell).CellType = CellType.StartCell;          
            var playerInst = Instantiate(PlayerPrefab, cell.transform.position, transform.rotation);

            gController.player = playerInst.GetComponent<Player>();
            gController.player.PlayerInstance = playerInst;
            StartCell = cell;
           
        }
        else if (cellType == EndCell)
        {
            ((EmptyCell)EndCell).CellType = CellType.EndCell;
            //HexCells.Add(cell);
            //cell.CellIndex = new Vector2Int(coords.x, coords.y);
           // cell.name = cell.name + cell.CellIndex.ToString();
            EndCell = cell;
        }
        else
        {
            ((EmptyCell)cell).CellType = CellType.EmptyCell; 
        }
        return cell;
    }



    //private List<Vector2Int> GetNeighboursCoords(Vector2Int currentPos, int rows, int columns)
    //{
        //return GetALLNeighbours(currentPos);
   // }
    private BaseCell GetCellType(Vector2Int startCoords, List<Vector2Int> missingCellCoordinates, List<Vector2Int> innerNeighbourCoords)
    {
        var randomList = new List<BaseCell>(hexCellPrefabs);
        var cellType =  randomList[UnityEngine.Random.Range(0, randomList.Count)];
        return CheckCellType(startCoords, missingCellCoordinates, cellType, innerNeighbourCoords);
    }

    private BaseCell CheckCellType(Vector2Int startCoords, List<Vector2Int> missingCellCoordinates, BaseCell cellType, List<Vector2Int> innerNeighbourCoords)
    {
        if (startCoords.x == 0 | startCoords.y == 0) //если крайние гексы
        {
            return hexCellPrefabs[0];
        }
        //check the existing neighbors 
        if (startCoords.y % 2 == 0) //even
        {
            if (missingCellCoordinates.Contains(new Vector2Int(startCoords.x, startCoords.y + 1)) | missingCellCoordinates.Contains(new Vector2Int(startCoords.x, startCoords.y - 1)))//если над ним и под ним наискосок пустая, то поменять тип ячейки на не пустой                                                                                        // if (missingCellCoordinates.Contains(new Vector2Int(startCoords.x, startCoords.y - 1)))//если под ним наискосок пустая, то поменять тип ячейки на не пустой
            {
                cellType = hexCellPrefabs[0];
            }
        }
        else //odd
        {
            if (missingCellCoordinates.Contains(new Vector2Int(startCoords.x - 1, startCoords.y + 1)) | missingCellCoordinates.Contains(new Vector2Int(startCoords.x + 1, startCoords.y - 1)))//если над ним  и под ним наискосок пустая, то поменять тип ячейки на не пустой
                                                                                                     
            {
                cellType = hexCellPrefabs[0];
            }
            ///}
        }
        return cellType;
    }
        
}




using System.Collections.Generic;
using UnityEngine;
//using System;

public class MapGenerator : MonoBehaviour
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

    private BaseCell startCell;
    private BaseCell endCell;

    //public List<BaseCell> HexCells;
    //public BaseCell[,] HexCells1;
    public List<CellContent> CellsContent = new List<CellContent>();


    public void Clear()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }
    private int CheckTypeOfEnemy()
    {
        int result = 0;
        var playerLvl = gController.playerProgress.Lvl;
        var complexity = playerLvl / 100;
        System.Random rnd = new System.Random();
      
        var value = rnd.Next(0, 1000);
        //1-DD, 2-Healer, 3-Tank

        if (value <= 1000 * complexity)
        {
            result = 2;
        }
        else if (value <= (1000 * complexity)/2)
        {
            result = 3;
        }
        else 
        {
            result = 1;
        }

        return result;
    }

    private int CheckTypeOfBonus()
    {
        System.Random rnd = new System.Random();
        return rnd.Next(3, 6);
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


    List<Vector2Int> GetALLNeighbours(Vector2Int vect, List<Vector2Int> missingCellCoordinates, int rows, int columns)
    {
        List<Vector2Int> nList = new List<Vector2Int>();
        if (vect.y % 2 == 0)
        {
            nList.Clear();
            if (!((vect.x - 1) < 0))
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
                    if (vect.y + 1 < columns)
                    {
                        nList.Add(new Vector2Int(vect.x - 1, vect.y + 1));
                    }
                }
            }
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x, vect.y + 1)))
            {
                if (vect.y + 1 < columns)
                {
                    nList.Add(new Vector2Int(vect.x, vect.y + 1));
                }
            }
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x + 1, vect.y)))
            {
                if (vect.x + 1 < rows)
                {
                    nList.Add(new Vector2Int(vect.x + 1, vect.y));
                }
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
                if (vect.y + 1 < columns)
                {
                    nList.Add(new Vector2Int(vect.x, vect.y + 1));
                }
            }
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x + 1, vect.y + 1)))
            {
                if (vect.y + 1 < columns && vect.x + 1 < rows)
                {
                    nList.Add(new Vector2Int(vect.x + 1, vect.y + 1));
                }
            }
            if (!missingCellCoordinates.Contains(new Vector2Int(vect.x + 1, vect.y)))
            {
                if (vect.x + 1 < rows)
                {
                    nList.Add(new Vector2Int(vect.x + 1, vect.y));
                }
            }
            if (!((vect.y - 1) < 0))
            {
                if (!missingCellCoordinates.Contains(new Vector2Int(vect.x + 1, vect.y - 1)))
                {
                    if (vect.x + 1 < rows)
                    {
                        nList.Add(new Vector2Int(vect.x + 1, vect.y - 1));
                    }
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

    private void SetContentPrefab(BaseCell cell)
    {
        if (cell.ContentType == CellType.StartCell | cell.ContentType == CellType.EndCell) return;

        int value = Random.Range(0, 3); //1 - bonus, 2 - enemy, 0 - empty

        switch(value)
        {
            case 1:
                cell.ContentType = CellType.BonusCell;//BonusCell
                cell.InstantiateContentPrefab(CellsContent[CheckTypeOfBonus()]);
                break;
            case 2:
                cell.ContentType = CellType.EnemyCell;//EnemyCell
                cell.InstantiateContentPrefab(CellsContent[CheckTypeOfEnemy()]);
                break;
            default:
                cell.ContentType = CellType.EmptyCell;//Empty
                break;
        }     
    }

        public Map MapGenerate(int rows, int columns, Player player, int lvl)
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
        // int startCellIndex = TakeRandomIntOfRange(0, lenght - 1);
        // int endCellIndex = TakeRandomIntOfRange(0, lenght - 1, startCellIndex);
        //  Debug.Log($"start: {startCellIndex.ToString()} end: {endCellIndex.ToString()}");

        for (int i = 0; i < neighbourCoords.Count; i++)
        {
            var coord = neighbourCoords[i];
            var innerNeighbourCoords = GetALLNeighbours(coord, missingCellCoordinates, rows, columns);// GetNeighboursCoords(coord, rows, columns); 
            /*foreach (var item in innerNeighbourCoords)
            {
                if (item.x >= rows || item.y >= columns)
                    continue;
                if(neighbourCoords.Contains(item)) 
                    continue;
  
                neighbourCoords.Add(item);       
            }*/
            /* if (startCellIndex == i)
             {
                 cellType = StartCell;
             }
             else if (endCellIndex == i)
             {
                 cellType = EndCell;
             }
             else
             {*/
            cellType = GetCellType(coord, missingCellCoordinates, innerNeighbourCoords);
            // }

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
                if (i > 1)
                {
                    if (CheckIsSeparateCell(map, missingCellCoordinates, coord, rows, columns)) continue; // if cell haven't neighbours - we dont create it
                }
                var currentCell = CreateCell(coord, cellType, player);
                currentCell.CellIndex = new Vector2Int(coord.x, coord.y);
                SetContentPrefab(currentCell);
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

        SetStartEndCell(map, player);
        //CheckSeparateCells(map, missingCellCoordinates, rows, columns);
        
        return new Map(map, startCell, endCell);
    }


    private void SetStartEndCell(BaseCell[,] map, Player player)
    {
        var startElem = GetRandomElementOfMap(map);

        for (int i = 0; i < map.GetLength(0) * map.GetLength(1); i++)
        {
            if (!(startElem == null)) break;

            startElem = GetRandomElementOfMap(map);
        }
        var endElem = GetRandomElementOfMap(map);

        for (int i = 0; i < map.GetLength(0) * map.GetLength(1); i++)
        {
            if (!(endElem == null) & !(endElem == startElem)) break;

            endElem = GetRandomElementOfMap(map);
        }

         startElem.ContentType = CellType.StartCell;
        if (gController.player == null)
        {
            var playerInst = Instantiate(PlayerPrefab, startElem.transform.position, transform.rotation);

            gController.player = playerInst.GetComponent<Player>();
            gController.player.PlayerInstance = playerInst;
        }
        else
        {
            gController.player.transform.position = startElem.transform.position;

        }

        startCell = startElem;
        startElem.name = "StartCell" + startElem.CellIndex;
        startElem.ContentLink = null;

        endElem.ContentType = CellType.EndCell;
        endCell = endElem;
        endElem.name = "EndCell" + endElem.CellIndex;
        endElem.ContentLink = null;
    }

    private BaseCell GetRandomElementOfMap(BaseCell[,] map)
    {
        var rand = new System.Random();
        return map[rand.Next(0, map.GetLength(0)), rand.Next(0, map.GetLength(1))];
    }


    private BaseCell CreateCell(Vector2Int coords, BaseCell cellType, Player player)
    {
        BaseCell cell;
        var v3coords = GetWorldPosition(coords.x, coords.y);
        cell = Instantiate(cellType, v3coords, cellType.transform.rotation, transform);
        cell.CellIndex = new Vector2Int(coords.x, coords.y);
        cell.name = cell.name + cell.CellIndex.ToString();
       // HexCells.Add(cell);

        /*if (cellType == StartCell)
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
        {*/
        cell.ContentType = CellType.EmptyCell;
        // }
        return cell;
    }

    private bool CheckIsSeparateCell(BaseCell[,] map, List<Vector2Int> missingCellCoordinates, Vector2Int coord, int rows, int columns)
    {
        var existingneighbours = new List<Vector2Int>();
        var neighbours = GetALLNeighbours(coord, missingCellCoordinates, rows, columns);
        foreach (Vector2Int neighbour in neighbours)
        {
            if (!(map[neighbour.x, neighbour.y] == null))
            {
                existingneighbours.Add(neighbour);
            }
        }
        if (existingneighbours.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }


    private BaseCell GetCellType(Vector2Int startCoords, List<Vector2Int> missingCellCoordinates, List<Vector2Int> innerNeighbourCoords)
    {
        var randomList = new List<BaseCell>(hexCellPrefabs);
        var cellType = randomList[UnityEngine.Random.Range(0, randomList.Count)];
        return CheckCellType(startCoords, missingCellCoordinates, cellType, innerNeighbourCoords);
    }

    private BaseCell CheckCellType(Vector2Int startCoords, List<Vector2Int> missingCellCoordinates, BaseCell cellType, List<Vector2Int> innerNeighbourCoords)
    {
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
        }
        return cellType;
    }

}




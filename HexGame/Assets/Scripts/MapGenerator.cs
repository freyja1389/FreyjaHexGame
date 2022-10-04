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


    /*public BaseCell[,] MapCreate(int rows, int columns, Transform transform, Player player, int lvl)
    {
        BaseCell cellType = null;
        HexCells = new BaseCell[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            var list = new List<BaseCell>();
            // HexCells.Add(list);

            for (int j = 0; j < columns; j++)
            {
                if (i == 0 & j == 0)
                {
                    cellType = StartCell;
                    ((EmptyCell)StartCell).StartCell = true;
                }
                else if (i == rows - 1 & j == columns - 1)
                {
                    cellType = EndCell;
                    ((EmptyCell)EndCell).EndCell = true;
                }
                else
                {
                    cellType = hexCellPrefabs[UnityEngine.Random.Range(0, hexCellPrefabs.Count)];
                    //if  missing cell - check neighbors
                    if (cellType is MissingCell)
                    {
                       // if (i != 0)
                        //{
                        if (j != 0)
                        {

                                cellType = CheckCellType(i, j, cellType);

                           // }
                        }
                    }
                }
                if (!(cellType is MissingCell))
                {
                    {
                        var instance = Instantiate(cellType, GetPosition(i, j), cellType.transform.rotation, transform);
                        if (instance is EmptyCell)
                        {
                            if (((EmptyCell)instance).StartCell)
                            {
                                var playerInst = Instantiate(PlayerPrefab, StartCell.transform.position, transform.rotation);
                                player.PlayerInstance = playerInst;
                            }
                        }
                        if (cellType is EnemyCell enemy)
                        {
                            enemy.DmgPoints = UnityEngine.Random.Range(7, 15);
                        }
                        else if (cellType is BonusCell bonus)
                        {
                            bonus.HealPoints = UnityEngine.Random.Range(10, 40);
                        }
                        //list.Add(instance);
                        HexCells[i, j] = instance;
                        instance.CellIndex = new Vector2Int(i, j);
                        instance.name = instance.name + instance.CellIndex.ToString();
                    }
                }
            }
        }
            return HexCells;
    }*/

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


   /* private BaseCell CheckCellType(int i, int j, BaseCell cellType)
    {

        //check the existing neighbors 
        if (i % 2 == 0) //even
        {
           // if (i != 0)
           // {
                if (HexCells1[i, j-1] is MissingCell)//если под ним наискосок пустая, то поменять тип ячейки на не пустой
                {
                    if (cellType is MissingCell)
                    {
                        cellType = hexCellPrefabs[UnityEngine.Random.Range(0, hexCellPrefabs.Count)];
                        CheckCellType(i, j, cellType);
                    }

                }
           // }
            return cellType;

        }
        else //odd
        {
            //if (j != 0)
            //{
                if (HexCells1[i -1, j - 1] is MissingCell)//если под ним наискосок пустая, то поменять тип ячейки на не пустой
                {
                    if (cellType is MissingCell)
                    {
                        cellType = hexCellPrefabs[UnityEngine.Random.Range(0, hexCellPrefabs.Count)];
                        CheckCellType(i, j, cellType);
                    }
                }
            ///}
            return cellType;
        }
    }*/


    /*public List<BaseCell> MapCreate1(int rows, int columns, Transform transform, Player player, int lvl)
    {
        BaseCell cellType = null;
        List<Vector2Int> neighbours;
        List<BaseCell> HexCells = new List<BaseCell>();
        List<Vector2Int> vectors = new List<Vector2Int>();
        int hexcount = rows * columns;
        var pos = GetWorldPosition(5, 5); //start position (need to be overrided in cycle)
        int i = 1;
        int startCellIndex = TakeRandomIntOfRange(1,hexcount-1);
        int endCellIndex = TakeRandomIntOfRange(1,hexcount-1, startCellIndex);
        Debug.Log($"start: {startCellIndex.ToString()} end: {endCellIndex.ToString()}");
        if (startCellIndex == i)
        {
            cellType = StartCell;
            ((EmptyCell)StartCell).StartCell = true;
            BaseCell instance = Instantiate(cellType, pos, cellType.transform.rotation, transform);
            HexCells.Add(instance);
            instance.CellIndex = new Vector2Int(0, 0);
            instance.name = instance.name + instance.CellIndex.ToString();
            var playerInst = Instantiate(PlayerPrefab, StartCell.transform.position, transform.rotation);
            player.PlayerInstance = playerInst;
            StartCell = instance;
        }
        else if (endCellIndex == i)
        {
            cellType = EndCell;
            BaseCell instance = Instantiate(cellType, pos, cellType.transform.rotation, transform);
            HexCells.Add(instance);
            instance.CellIndex = new Vector2Int(0, 0);
            instance.name = instance.name + instance.CellIndex.ToString();
            ((EmptyCell)EndCell).EndCell = true;
            EndCell = instance;
        }
        else
        {
            var randomList = new List<BaseCell>(hexCellPrefabs);
            var missCell = randomList.Find(x => x is MissingCell);
            randomList.Remove(missCell);
  
            cellType = randomList[UnityEngine.Random.Range(0, randomList.Count)];

            
            BaseCell instance = Instantiate(cellType, pos, cellType.transform.rotation, transform);
            CheckContent((EmptyCell)instance);
            instance.CellIndex = new Vector2Int(5, 5);
            instance.name = instance.name + instance.CellIndex.ToString();
            HexCells.Add(instance);
        }

        vectors.Add(new Vector2Int(5,5));
        neighbours = GetALLNeighbours(new Vector2Int(5, 5));
        /*for (int i=1; i<hexcount; i++ )
         {
             var pinstance = instance;
             if (i == hexcount-1)
             {
                 cellType = EndCell;
                 ((EmptyCell)EndCell).EndCell = true;
                 instance = Instantiate(cellType, new Vector3(pinstance.CellIndex.x, pinstance.CellIndex.y), cellType.transform.rotation, transform);
                 HexCells[0, 0] = instance;
                 instance.CellIndex = new Vector2Int(instance.CellIndex.x, instance.CellIndex.y);
                 instance.name = instance.name + instance.CellIndex.ToString();
                 i++;
             }*/

       
               // i = CellGeneraton(neighbours, player, hexcount, i, vectors, startCellIndex, endCellIndex, HexCells);
      

        //}
       // return HexCells;
   // }

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
   /* int CellGeneraton(List<Vector2Int> indexForGeneration, Player player, int hexcount, int i, List<Vector2Int> vectors, int startCellIndex, int endCellIndex, List<BaseCell> HexCells)
    {
        if (i >= hexcount)
        {
            return i;
        }

        List<Vector2Int> instances = new List<Vector2Int>();
        i = GenetationCellsOfIndexes(indexForGeneration, vectors, instances, i, startCellIndex, endCellIndex, player, HexCells);

        //foreach (Vector2Int inst in instances)
        for(int j = 0; j < instances.Count; j++ )
        {
            var inst = instances[j];


            if (i >= hexcount)
            {
                return i;
            }

            List<Vector2Int> innerneighbours = GetALLNeighbours(inst);
            foreach (var vect in vectors)
            {
                if(innerneighbours.Contains(vect))
                {
                    innerneighbours.Remove(vect);
                } 
            }
            //i = CellGeneraton(innerneighbours, player, hexcount, i, vectors);
            i = GenetationCellsOfIndexes(innerneighbours, vectors, instances, i, startCellIndex, endCellIndex, player, HexCells);
        }
        return i;
    }
    
    int GenetationCellsOfIndexes(List<Vector2Int> indexForGeneration, List<Vector2Int> vectors, List<Vector2Int> instances, int i, int startCellIndex, int endCellIndex, Player player, List<BaseCell> HexCells)
    {
        BaseCell instance;
        for (int j = indexForGeneration.Count - 1; j >= 0; j--)
        {
            var neighbour = indexForGeneration[j];
            //проверка на то, что такая ячейка уже добавлена в предыдущей итерации
            if (vectors.Contains(neighbour))
            {
                continue;
            }

            var randomList = new List<BaseCell>(hexCellPrefabs);
            if (indexForGeneration.Count <= 1)
            {
                var missCell = randomList.Find(x => x is MissingCell);
                randomList.Remove(missCell);
            }

            if (startCellIndex == i)
            {
                BaseCell cellType = StartCell;
                // ((EmptyCell)StartCell).StartCell = true;
                instance = Instantiate(StartCell, GetWorldPosition(neighbour.x, neighbour.y), cellType.transform.rotation, transform);
                ((EmptyCell)instance).StartCell = true;
                StartCell = instance;
                var playerInst = Instantiate(PlayerPrefab, GetWorldPosition(neighbour.x, neighbour.y), cellType.transform.rotation, transform);
                player.PlayerInstance = playerInst;
                vectors.Add(neighbour);
                instances.Add(neighbour);
                indexForGeneration.Remove(neighbour);
                instance.CellIndex = new Vector2Int(neighbour.x, neighbour.y);
                instance.name = instance.name + instance.CellIndex.ToString();
                HexCells.Add(instance);
                i++;
            }
            else if (endCellIndex == i)
            {
                BaseCell cellType = EndCell;
                //((EmptyCell)EndCell).EndCell = true;
                instance = Instantiate(EndCell, GetWorldPosition(neighbour.x, neighbour.y), cellType.transform.rotation, transform);
                ((EmptyCell)instance).EndCell = true;
                EndCell = instance;
                vectors.Add(neighbour);
                instances.Add(neighbour);
                indexForGeneration.Remove(neighbour);
                instance.CellIndex = new Vector2Int(neighbour.x, neighbour.y);
                instance.name = instance.name + instance.CellIndex.ToString();
                HexCells.Add(instance);
                i++;
            }
            else
            {

                BaseCell cellType = randomList[UnityEngine.Random.Range(0, randomList.Count)];

                if (!(cellType is MissingCell))
                {
                        instance = Instantiate(cellType, GetWorldPosition(neighbour.x, neighbour.y), cellType.transform.rotation, transform);
                   
                    CheckContent((EmptyCell)instance);
                    instance.CellIndex = new Vector2Int(neighbour.x, neighbour.y);
                    instance.name = instance.name + instance.CellIndex.ToString();
                    HexCells.Add(instance);
                }

                vectors.Add(neighbour);
                instances.Add(neighbour);
                indexForGeneration.Remove(neighbour);
                i++;
            }


        }
        return i;
    }
    */

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

    private void CheckContent(EmptyCell cell)
    {
        System.Random rnd = new System.Random();
        int value = rnd.Next(0, 3); //1 - bonus, 2 - enemy, 0 - empty
        if (value == 1)
        {
            cell.BonusCell = true;
        }
        else if (value == 2)
        {
            cell.EnemyCell = true;
        }
    }

    public BaseCell[,] MapGenerate(int rows, int columns, Transform transform, Player player, int lvl)
    {
        BaseCell cellType;
        var map = new BaseCell[rows, columns];
        var startPoint = new Vector2Int(Mathf.CeilToInt(rows/2), Mathf.CeilToInt(columns/2));
        var neighbourCoords = new List<Vector2Int>() { startPoint };
        var lenght = rows * columns;
        var missingCellCoordinates = new List<Vector2Int>();
        int startCellIndex = TakeRandomIntOfRange(1, lenght - 1);
        int endCellIndex = TakeRandomIntOfRange(1, lenght - 1, startCellIndex);
        Debug.Log($"start: {startCellIndex.ToString()} end: {endCellIndex.ToString()}");

        for (int i = 0; i<neighbourCoords.Count; i++)
        {
            var coord = neighbourCoords[i];
            var innerNeighbourCoords = GetALLNeighbours(coord, missingCellCoordinates);// GetNeighboursCoords(coord, rows, columns); 
            foreach (var item in innerNeighbourCoords)
            {
                if (item.x >= rows || item.y >= columns)
                    continue;
                if(neighbourCoords.Contains(item)) 
                    continue;
  
                neighbourCoords.Add(item);       
            }
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
                var currentCell = CreateCell(coord, cellType, player);
                currentCell.CellIndex = new Vector2Int(coord.x, coord.y);
                CheckContent((EmptyCell)currentCell);
                map[currentCell.CellIndex.x, currentCell.CellIndex.y] = currentCell;
            }
        }
        return map;
    }

    private BaseCell CreateCell(Vector2Int coords, BaseCell cellType, Player player)
    {
        BaseCell cell;
        var v3coords = GetWorldPosition(coords.x, coords.y);
        cell = Instantiate(cellType, v3coords, cellType.transform.rotation, transform);
        if (cellType == StartCell)
        {
            ((EmptyCell)StartCell).StartCell = true;
            HexCells.Add(cell);
            cell.CellIndex = new Vector2Int(coords.x, coords.y);
            cell.name = cell.name + cell.CellIndex.ToString();
            var playerInst = Instantiate(PlayerPrefab, cell.transform.position, transform.rotation);
            player.PlayerInstance = playerInst;
            StartCell = cell;
        }
        else if (cellType == EndCell)
        {
            ((EmptyCell)EndCell).EndCell = true;
            HexCells.Add(cell);
            cell.CellIndex = new Vector2Int(coords.x, coords.y);
            cell.name = cell.name + cell.CellIndex.ToString();
            ((EmptyCell)EndCell).EndCell = true;
            EndCell = cell;
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
   /* private List<Vector2Int> GetAvaiableCoords(Vector2Int startCoords, List<Vector2Int> neighbourCoords, List<Vector2Int> missingCellCoordinates)
    {
        List<Vector2Int> AvaiableCoords = new List<Vector2Int>();
        foreach (var coord in neighbourCoords)
        {
            if (coord == null) 
                continue;
            
            var distance = coord - startCoords;
            if (startCoords.y % 2 == 0)
            {
                if (gController.neighborRulesEvenY.Contains(distance))
                {
                    if (!missingCellCoordinates.Contains(distance))
                    {
                        AvaiableCoords.Add(coord);
                    }
                }
            }
            else
            {
                if (gController.neighborRulesOddY.Contains(distance))
                {
                    if (!missingCellCoordinates.Contains(distance))
                    {
                        AvaiableCoords.Add(coord);
                    }
                }
            }


        }
        return AvaiableCoords;
    }*/
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




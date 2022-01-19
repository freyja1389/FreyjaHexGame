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

   public  BaseCell StartCell;

    //public List<List<HexCell>> HexCells = new List<List<HexCell>>();

    public BaseCell[,] HexCells;
    public BaseCell[,] MapCreate(int rows, int columns, Transform transform)
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
                }
                else
                {
                    cellType = hexCellPrefabs[UnityEngine.Random.Range(0, hexCellPrefabs.Count)];
                }
                var instance = Instantiate(cellType, GetPosition(i, j), cellType.transform.rotation, transform);
                if (cellType is EnemyCell enemy)
                {
                    enemy.DmgPoints = UnityEngine.Random.Range(3, 25);
                }
                else if (cellType is BonusCell bonus)
                {
                    bonus.HealPoints = UnityEngine.Random.Range(3, 25);
                }
                //list.Add(instance);
                HexCells[i, j] = instance;
                instance.CellIndex = new Vector2Int(i, j);
            }
        }
        return HexCells;
    }

    private Vector3 GetPosition(int x, int y, float hexSize = 1)
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

}

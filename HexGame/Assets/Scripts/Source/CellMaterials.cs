using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellMaterials", menuName = "Scriptable constants/Cell Materials", order = 1)]
public class CellMaterials : ScriptableObject
{
    [SerializeField]
    private List<NamedMaterial> NamedMaterials;

    public Material GetMaterial(MaterialNames name)
    {
        return NamedMaterials.Find(x => x.Name == name)?.Material; //?./ - FAST NULL CHECK
    }

    [System.Serializable]
    public class NamedMaterial
    {
        public MaterialNames Name;
        public Material Material;
    }

    public enum MaterialNames
    {
        None = 0,
        Locked = 1,
        Unlocked = 2,
        EmptyMaterial = 3,
        EndMaterial = 4,
        StartMaterial = 5,
        BonusHealerMaterial = 6,
        EnemyDDMaterial = 7,
    }
}



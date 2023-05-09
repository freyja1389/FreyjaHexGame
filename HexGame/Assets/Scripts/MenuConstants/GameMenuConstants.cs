using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New GameMenuConstants", menuName = "Scriptable constants/Game menu constants", order = 1)]
public class GameMenuConstants : ScriptableObject
{
    [SerializeField]
    public string WinMessage;
    [SerializeField]
    public string LooseMessage;


}

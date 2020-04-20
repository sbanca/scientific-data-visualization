using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameversion = "0.0.1";

    public string Gameversion { get { return _gameversion; } }

    [SerializeField]

    private string _nickname = "PunFish";

    public string Nickname { 
        get { 

            int value = Random.Range(0,9999);
            
            return _nickname + value.ToString();
            
        } 
    }

}

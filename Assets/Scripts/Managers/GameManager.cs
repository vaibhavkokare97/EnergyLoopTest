using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private ConstructLevel _constructLevel;
    public ConstructLevel constructLevel
    {
        get
        {
            return _constructLevel;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class TileDataBase : MonoBehaviour
{
    public bool topConnected = false;
    public bool topRightConnected = false;
    public bool topLeftConnected = false;
    public bool bottomConnected = false;
    public bool bottomRightConnected = false;
    public bool bottomLeftConnected = false;

    protected Tilemap _tileMap;
    protected Tile _tile;
    protected Vector3 _location;
    protected Vector3Int _tileLocation;
    [SerializeField] protected TileStructure.TileType _tileType;

    public List<TileData> neighbouringTiles = new List<TileData>(); // neighbour's start from top
    protected bool _energized = false;
    public bool energized
    {
        get
        {
            return _energized;
        }
        set
        {
            if (_energized != value)
            {
                OnEnergized.Invoke(value);
                _energized = value;
            }
        }
    }

    public Action OnTileRotate;
    public Action<bool> OnEnergized;

    private void OnEnable()
    {
        GetComponent<Rotate>().OnRotateClickAction += ChangeConnectedSides;
    }

    void Start()
    {
        _tile = new Tile(); // this is a tile
        _tileMap = GetComponentInParent<Tilemap>();
        _location = gameObject.transform.position;
        _tileLocation = _tileMap.WorldToCell(_location);
        _tileMap.SetTile(_tileLocation, _tile);
    }

    void ChangeConnectedSides()
    {
        bool oldTopConnected = topConnected;
        bool oldTopRightConnected = topRightConnected;
        bool oldTopLeftConnected = topLeftConnected;
        bool oldBottomConnected = bottomConnected;
        bool oldBottomRightConnected = bottomRightConnected;
        bool oldBottomLeftConnected = bottomLeftConnected;

        topConnected = oldTopLeftConnected;
        topRightConnected = oldTopConnected;
        bottomRightConnected = oldTopRightConnected;
        bottomConnected = oldBottomRightConnected;
        bottomLeftConnected = oldBottomConnected;
        topLeftConnected = oldBottomLeftConnected;

        OnTileRotate.Invoke();
    }

    private void OnDiable()
    {
        GetComponent<Rotate>().OnRotateClickAction -= ChangeConnectedSides;
    }

}

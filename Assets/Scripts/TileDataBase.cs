using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public abstract class TileDataBase : MonoBehaviour
{
    public bool[] connectedLinksToSide = new bool[6] { false, false, false, false, false, false };

    protected Tilemap _tileMap;
    public Tile tile;
    protected Vector3 _location;
    protected Vector3Int _tileLocation;
    [SerializeField] protected TileStructure.TileType _tileType;

    public Dictionary<TileData, bool> neighbouringTiles = new Dictionary<TileData, bool>(); // neighbour's start from top
    private bool _energized = false;
    public virtual bool energized
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
                GetComponent<SpriteRenderer>().color = (energized) ? Color.red : Color.white;
            }
        }
    }

    public Action<bool> OnEnergized;
    public Action OnConnectionChange;

    private void OnEnable()
    {
        GetComponent<Rotate>().OnRotateClickAction += ChangeConnectedSides;
        foreach (var item in neighbouringTiles)
        {
            item.Key.OnConnectionChange += CheckAllSidesAboutEnergizing;
            item.Key.OnEnergized += CheckAllSidesAboutEnergizing;
        }
    }

    public void SetTile()
    {
        tile = new Tile(); // this is a tile
    }

    protected virtual IEnumerator Start()
    {
        _tileMap = GetComponentInParent<Tilemap>();
        _location = gameObject.transform.position;
        _tileLocation = _tileMap.WorldToCell(_location);
        _tileMap.SetTile(_tileLocation, tile);

        yield return new WaitForSeconds(0.3f);
        GetNeighbouringTiles();
    }



    protected virtual void ChangeConnectedSides()
    {
        bool[] debugConnectedLinksToSide = new bool[6] { false, false, false, false, false, false };
        for (int i = 0; i < connectedLinksToSide.Length; i++)
        {
            debugConnectedLinksToSide[i] = connectedLinksToSide[i];
        }

        for (int i = 0; i < connectedLinksToSide.Length; i++)
        {
            connectedLinksToSide[i] = debugConnectedLinksToSide[(i + 5) % 6];
        }

        OnConnectionChange.Invoke();
    }

    void CheckAllSidesAboutEnergizing()
    {
        int energizedConnections = 0;
        for (int i = 0; i < connectedLinksToSide.Length; i++)
        {
            if (connectedLinksToSide[i] == neighbouringTiles[neighbouringTiles.Keys.ToList()[(i + 3)%6]] && neighbouringTiles.Keys.ToList()[(i + 3) % 6].energized)
            {
                energizedConnections++;
            }
        }
        energized = (energizedConnections > 0) ? true : false;
    }



    public void GetNeighbouringTiles()
    {
        Tile tileTop = _tileMap.GetTile<Tile>(new Vector3Int(_tileLocation.x + 1, _tileLocation.y, _tileLocation.z));
        TileData tileTopData = (tileTop != null) ? GameManager.Instance.constructLevel.tileDataList[tileTop] : null;
        neighbouringTiles.Add(tileTopData, false);

        Vector3Int neighbouringLocationTopRight = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x, _tileLocation.y + 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x + 1, _tileLocation.y + 1, _tileLocation.z);
        Tile tileTopRight = _tileMap.GetTile<Tile>(neighbouringLocationTopRight);
        TileData tileTopRightData = (tileTopRight != null) ? GameManager.Instance.constructLevel.tileDataList[tileTopRight] : null;
        neighbouringTiles.Add(tileTopRightData, false);

        Vector3Int neighbouringLocationBottomRight = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x - 1, _tileLocation.y + 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x, _tileLocation.y + 1, _tileLocation.z);
        Tile tileBottomRight = _tileMap.GetTile<Tile>(neighbouringLocationBottomRight);
        TileData tileBottomRightData = (tileBottomRight != null) ? GameManager.Instance.constructLevel.tileDataList[tileBottomRight] : null;
        neighbouringTiles.Add(tileBottomRightData, false);

        Tile tileBottom = _tileMap.GetTile<Tile>(new Vector3Int(_tileLocation.x - 1, _tileLocation.y, _tileLocation.z));
        TileData tileBottomData = (tileBottom != null) ? GameManager.Instance.constructLevel.tileDataList[tileBottom] : null;
        neighbouringTiles.Add(tileBottomData, false);

        Vector3Int neighbouringLocationBottomLeft = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x - 1, _tileLocation.y - 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x, _tileLocation.y - 1, _tileLocation.z);
        Tile tileBottomLeft = _tileMap.GetTile<Tile>(neighbouringLocationBottomLeft);
        TileData tileBottomLeftData = (tileBottomLeft != null) ? GameManager.Instance.constructLevel.tileDataList[tileBottomLeft] : null;
        neighbouringTiles.Add(tileBottomLeftData, false);

        Vector3Int neighbouringLocationTopLeft = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x, _tileLocation.y + 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x + 1, _tileLocation.y - 1, _tileLocation.z);
        Tile tileTopLeft = _tileMap.GetTile<Tile>(neighbouringLocationTopLeft);
        TileData tileTopLeftData = (tileTopLeft != null) ? GameManager.Instance.constructLevel.tileDataList[tileTopLeft] : null;
        neighbouringTiles.Add(tileTopLeftData, false);
    }

    private void OnDisable()
    {
        GetComponent<Rotate>().OnRotateClickAction -= ChangeConnectedSides;
        foreach (var item in neighbouringTiles)
        {
            item.Key.OnConnectionChange -= CheckAllSidesAboutEnergizing;
        }
    }

}

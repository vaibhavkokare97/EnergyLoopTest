using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public abstract class TileDataBase : MonoBehaviour
{
    public bool[] connectedLinksToSide = new bool[6] { false, false, false, false, false, false }; // start from top

    public Tile tile;
    protected Vector3 _location;
    protected Vector3Int _tileLocation;
    public TileStructure.TileType tileType;

    private Tilemap _tilemap;
    public List<TileDataBase> neighbouringTiles = new List<TileDataBase>(); // neighbour's start from top
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
                OnEnergized?.Invoke(value);
                _energized = value;
                GetComponent<SpriteRenderer>().color = (energized) ? Color.red : Color.white;
            }
        }
    }

    public Action<bool> OnEnergized;
    public Action<TileDataBase> OnTileTap;

    public void SetTile()
    {
        tile = new Tile(); // this is a tile
    }

    protected virtual IEnumerator Start()
    {
        
        yield return new WaitForSeconds(0.1f);
        _tilemap = GameManager.Instance.constructLevel.tilemap;
        _location = gameObject.transform.position;
        _tileLocation = _tilemap.WorldToCell(_location);
        _tilemap.SetTile(_tileLocation, tile);

        yield return new WaitForSeconds(0.3f);
        GetNeighbouringTiles();

        yield return new WaitForSeconds(0.1f);
        foreach (var item in neighbouringTiles)
        {
            if (item != null)
            {
                item.OnTileTap += TileTapConnectionChangesCheck;
                //item.Key.OnEnergized += CheckAllSidesAboutEnergizing;
            }

        }
        GetComponent<Rotate>().OnRotateClickAction += ChangeConnectedSides;
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

        OnTileTap?.Invoke(this);
        GameManager.Instance.constructLevel.Energize();
    }


    void TileTapConnectionChangesCheck(TileDataBase tappedTile)
    {
        if (IsConnected(tappedTile))
        {
            GameManager.Instance.constructLevel.tileNetwork.AddEdge(tappedTile, this);
        }
        else
        {
            GameManager.Instance.constructLevel.tileNetwork.RemoveEdge(tappedTile, this);
        }
    }


    public void GetNeighbouringTiles()
    {
        Tile tileTop = _tilemap.GetTile<Tile>(new Vector3Int(_tileLocation.x + 1, _tileLocation.y, _tileLocation.z));
        TileDataBase tileTopData = (tileTop != null) ? GameManager.Instance.constructLevel.tileDataList[tileTop] : null;
        neighbouringTiles.Add(tileTopData);

        Vector3Int neighbouringLocationTopRight = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x, _tileLocation.y + 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x + 1, _tileLocation.y + 1, _tileLocation.z);
        Tile tileTopRight = _tilemap.GetTile<Tile>(neighbouringLocationTopRight);
        TileDataBase tileTopRightData = (tileTopRight != null) ? GameManager.Instance.constructLevel.tileDataList[tileTopRight] : null;
        neighbouringTiles.Add(tileTopRightData);

        Vector3Int neighbouringLocationBottomRight = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x - 1, _tileLocation.y + 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x, _tileLocation.y + 1, _tileLocation.z);
        Tile tileBottomRight = _tilemap.GetTile<Tile>(neighbouringLocationBottomRight);
        TileDataBase tileBottomRightData = (tileBottomRight != null) ? GameManager.Instance.constructLevel.tileDataList[tileBottomRight] : null;
        neighbouringTiles.Add(tileBottomRightData);

        Tile tileBottom = _tilemap.GetTile<Tile>(new Vector3Int(_tileLocation.x - 1, _tileLocation.y, _tileLocation.z));
        TileDataBase tileBottomData = (tileBottom != null) ? GameManager.Instance.constructLevel.tileDataList[tileBottom] : null;
        neighbouringTiles.Add(tileBottomData);

        Vector3Int neighbouringLocationBottomLeft = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x - 1, _tileLocation.y - 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x, _tileLocation.y - 1, _tileLocation.z);
        Tile tileBottomLeft = _tilemap.GetTile<Tile>(neighbouringLocationBottomLeft);
        TileDataBase tileBottomLeftData = (tileBottomLeft != null) ? GameManager.Instance.constructLevel.tileDataList[tileBottomLeft] : null;
        neighbouringTiles.Add(tileBottomLeftData);

        Vector3Int neighbouringLocationTopLeft = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x, _tileLocation.y + 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x + 1, _tileLocation.y - 1, _tileLocation.z);
        Tile tileTopLeft = _tilemap.GetTile<Tile>(neighbouringLocationTopLeft);
        TileDataBase tileTopLeftData = (tileTopLeft != null) ? GameManager.Instance.constructLevel.tileDataList[tileTopLeft] : null;
        neighbouringTiles.Add(tileTopLeftData);
    }

    public bool IsConnected(TileDataBase neighbourTile)
    {
        if (!neighbouringTiles.Contains(neighbourTile))
        {
            return false; // check if it is actually a neighbouring tile
        }

        int sideConnectedTo = ListElementAt(neighbouringTiles, neighbourTile);
        if(sideConnectedTo == -1)
        {
            return false;
        }

        if(neighbourTile.connectedLinksToSide[(sideConnectedTo + 3) % 6] == true && connectedLinksToSide[sideConnectedTo] == true)
        {
            return true;
        }
        return false;
    }

    public int ListElementAt<A>(List<A> list, A key)
    {
        for (int i =0; i < list.Count; i++)
        {
            if (list.ElementAt(i) == null) continue;
            if(list.ElementAt(i).Equals(key))
            {
                return i;
            }
        }
        return -1;
    }

    private void OnDisable()
    {
        GetComponent<Rotate>().OnRotateClickAction -= ChangeConnectedSides;
        foreach (var item in neighbouringTiles)
        {
            if (item != null)
            {
                item.OnTileTap -= TileTapConnectionChangesCheck;
            }
        }
    }

}

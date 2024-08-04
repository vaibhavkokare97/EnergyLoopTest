using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using DG.Tweening;

public abstract class TileDataBase : MonoBehaviour
{
    [SerializeField] protected bool[] connectedLinksToSide = new bool[6] { false, false, false, false, false, false }; // start from top

    [HideInInspector] public Tile tile;
    protected Vector3 _location;
    protected Vector3Int _tileLocation;
    public TileStructure.TileType tileType;

    private Tilemap _tilemap;
    [HideInInspector] public List<TileDataBase> neighbouringTiles = new List<TileDataBase>(); // neighbour's start from top
    protected bool _energized = false;
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
            }
        }
    }

    public Action<bool> OnEnergized;
    public Action<TileDataBase> OnTileTap;

    public void SetTile()
    {
        tile = new Tile(); // this is a tile
    }

    private float _currentAngle = 0f;
    public Action OnRotateClickAction;

    private void Awake()
    {
        SnapToNearestAngle.SnapRotation(transform, Vector3.forward, 60f);
        _currentAngle = transform.rotation.eulerAngles.z;

    }

    private void OnMouseDown()
    {
        if (GameManager.gameStage != GameManager.GameStage.Play)
        {
            return;
        }
        OnRotateClickAction?.Invoke();
        transform.DORotate(new Vector3(0, 0, _currentAngle - 60f), 0.2f, RotateMode.Fast);
        _currentAngle += -60f;
    }

    protected virtual IEnumerator Start()
    {
        connectedLinksToSide = TileStructure.ConnectedSides(tileType);
        int x = Mathf.FloorToInt(((float)((_currentAngle < 180) ? _currentAngle : (360f - _currentAngle))) / 60f);
        Debug.Log(x);
        ChangeConnectedLinks(x);
        yield return new WaitForSeconds(0.05f);
        _tilemap = GameManager.Instance.tilemap;
        _location = gameObject.transform.position;
        _tileLocation = _tilemap.WorldToCell(_location);
        _tilemap.SetTile(_tileLocation, tile);

        yield return new WaitForSeconds(0.05f);
        GetNeighbouringTiles();

        yield return new WaitForSeconds(0.05f);
        foreach (var item in neighbouringTiles)
        {
            if (item != null)
            {
                item.OnTileTap += TileTapConnectionChangesCheck;
            }

        }
        OnEnergized += OnTileEnergize;
        OnRotateClickAction += ChangeConnectedSides;
    }

    protected virtual void OnTileEnergize(bool onEnergize)
    {
        GetComponent<SpriteRenderer>().color = (onEnergize) ? Color.white : new Color(1f, 1f, 1f, 0.2f);
    }

    private void ChangeConnectedLinks(int n)
    {
        bool[] debugConnectedLinksToSide = new bool[6] { false, false, false, false, false, false };
        for (int i = 0; i < connectedLinksToSide.Length; i++)
        {
            debugConnectedLinksToSide[i] = connectedLinksToSide[i];
        }

        for (int i = 0; i < connectedLinksToSide.Length; i++)
        {
            connectedLinksToSide[i] = debugConnectedLinksToSide[(i + 5 - (n - 1)) % 6];
        }
    }


    protected virtual void ChangeConnectedSides()
    {
        ChangeConnectedLinks(1);

        OnTileTap?.Invoke(this);
        GameManager.Instance.Energize();
    }


    void TileTapConnectionChangesCheck(TileDataBase tappedTile)
    {
        if (IsConnected(tappedTile))
        {
            GameManager.Instance.tileNetwork.AddEdge(tappedTile, this);
        }
        else
        {
            GameManager.Instance.tileNetwork.RemoveEdge(tappedTile, this);
        }
    }


    public void GetNeighbouringTiles()
    {
        Tile tileTop = _tilemap.GetTile<Tile>(new Vector3Int(_tileLocation.x + 1, _tileLocation.y, _tileLocation.z));
        TileDataBase tileTopData = (tileTop != null) ? GameManager.Instance.tileDataList[tileTop] : null;
        neighbouringTiles.Add(tileTopData);

        Vector3Int neighbouringLocationTopRight = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x, _tileLocation.y + 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x + 1, _tileLocation.y + 1, _tileLocation.z);
        Tile tileTopRight = _tilemap.GetTile<Tile>(neighbouringLocationTopRight);
        TileDataBase tileTopRightData = (tileTopRight != null) ? GameManager.Instance.tileDataList[tileTopRight] : null;
        neighbouringTiles.Add(tileTopRightData);

        Vector3Int neighbouringLocationBottomRight = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x - 1, _tileLocation.y + 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x, _tileLocation.y + 1, _tileLocation.z);
        Tile tileBottomRight = _tilemap.GetTile<Tile>(neighbouringLocationBottomRight);
        TileDataBase tileBottomRightData = (tileBottomRight != null) ? GameManager.Instance.tileDataList[tileBottomRight] : null;
        neighbouringTiles.Add(tileBottomRightData);

        Tile tileBottom = _tilemap.GetTile<Tile>(new Vector3Int(_tileLocation.x - 1, _tileLocation.y, _tileLocation.z));
        TileDataBase tileBottomData = (tileBottom != null) ? GameManager.Instance.tileDataList[tileBottom] : null;
        neighbouringTiles.Add(tileBottomData);

        Vector3Int neighbouringLocationBottomLeft = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x - 1, _tileLocation.y - 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x, _tileLocation.y - 1, _tileLocation.z);
        Tile tileBottomLeft = _tilemap.GetTile<Tile>(neighbouringLocationBottomLeft);
        TileDataBase tileBottomLeftData = (tileBottomLeft != null) ? GameManager.Instance.tileDataList[tileBottomLeft] : null;
        neighbouringTiles.Add(tileBottomLeftData);

        Vector3Int neighbouringLocationTopLeft = (_tileLocation.y % 2 == 0) ? new Vector3Int(_tileLocation.x, _tileLocation.y - 1, _tileLocation.z) :
            new Vector3Int(_tileLocation.x + 1, _tileLocation.y - 1, _tileLocation.z);
        Tile tileTopLeft = _tilemap.GetTile<Tile>(neighbouringLocationTopLeft);
        TileDataBase tileTopLeftData = (tileTopLeft != null) ? GameManager.Instance.tileDataList[tileTopLeft] : null;
        neighbouringTiles.Add(tileTopLeftData);
    }

    public bool IsConnected(TileDataBase neighbourTile)
    {
        if (!neighbouringTiles.Contains(neighbourTile))
        {
            return false; // check if it is actually a neighbouring tile
        }

        int sideConnectedTo = ListElementAt(neighbouringTiles, neighbourTile);
        if (sideConnectedTo == -1)
        {
            return false;
        }

        if (neighbourTile.connectedLinksToSide[(sideConnectedTo + 3) % 6] == true && connectedLinksToSide[sideConnectedTo] == true)
        {
            return true;
        }
        return false;
    }

    public int ListElementAt<A>(List<A> list, A key)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list.ElementAt(i) == null) continue;
            if (list.ElementAt(i).Equals(key))
            {
                return i;
            }
        }
        return -1;
    }

    private void OnDisable()
    {
        OnRotateClickAction -= ChangeConnectedSides;
        OnEnergized -= OnTileEnergize;
        foreach (var item in neighbouringTiles)
        {
            if (item != null)
            {
                item.OnTileTap -= TileTapConnectionChangesCheck;
            }
        }
    }

}

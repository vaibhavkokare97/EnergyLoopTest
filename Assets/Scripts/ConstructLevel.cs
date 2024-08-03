using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConstructLevel : MonoBehaviour
{
    public Tilemap tilemap;
    public Dictionary<Tile, TileDataBase> tileDataList = new Dictionary<Tile, TileDataBase>();
    public Graph<TileDataBase> tileNetwork;

    private void Start()
    {
        FillTileDataList();
    }

    public void FillTileDataList()
    {
        tilemap = GetComponent<Tilemap>();
        tileDataList.Clear();
        tileNetwork = new Graph<TileDataBase>();
        foreach (var item in gameObject.GetComponentsInChildren<TileDataBase>())
        {
            item.SetTile();
            tileDataList.Add(item.tile, item);
            tileNetwork.AddNode(item);
        }
    }

    public void Energize()
    {
        var nonEnergyTiles = tileNetwork.Nodes.Where((e) => e.Value.tileType != TileStructure.TileType.EnergyTile).ToList();
        foreach (var node in nonEnergyTiles)
        {
            node.Value.energized = false;
        }

        var energyTiles = tileNetwork.Nodes.Where((e) => e.Value.tileType == TileStructure.TileType.EnergyTile).ToList();
        foreach (var energyTile in energyTiles)
        {
            for (int i = 0; i < energyTile.Neighbours.Count; i++)
            {
                EnergizeNeighbours(energyTile);
            }
        }
    }

    private void EnergizeNeighbours(GraphNode<TileDataBase> tileDataNode)
    {
        if (tileDataNode.Neighbours.Count > 0)
        {
            foreach (var neighbour in tileDataNode.Neighbours)
            {
                if (neighbour.Value.energized == false)
                {
                    neighbour.Value.energized = true;
                    EnergizeNeighbours(neighbour);
                }
            }
        }
    }

    public void SnapAllTiles()
    {
        foreach (var tileData in tileDataList)
        {
            SnapToNearestAngle.SnapRotation(tileData.Value.transform, Vector3.forward, 60f);
        }
    }
}

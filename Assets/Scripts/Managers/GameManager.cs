using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public Tilemap tilemap;
    public Dictionary<Tile, TileDataBase> tileDataList = new Dictionary<Tile, TileDataBase>();
    public Graph<TileDataBase> tileNetwork;
    public static int energizedNodeCount;
    [HideInInspector] public int totalBulbNodeCount;
    [HideInInspector] public int energizedBulbNodeCount;
    public static GameStage gameStage;

    List<GraphNode<TileDataBase>> nonEnergyTiles, energyTiles;

    public enum GameStage
    {
        Menu, Play, LevelComplete
    }

    private void Update()
    {
        //Debug.Log(gameStage);
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LevelManager.Instance.unlockedLevels.Count > 0);
        LevelManager.Instance.LevelSwitch(0, true);
        gameStage = GameStage.Play;
    }


    public void ResetData()
    {
        tileDataList.Clear();
        tileNetwork = new Graph<TileDataBase>();
        foreach (var item in tilemap.gameObject.GetComponentsInChildren<TileDataBase>())
        {
            item.SetTile();
            tileDataList.Add(item.tile, item);
            tileNetwork.AddNode(item);
        }
        energizedBulbNodeCount = 0;
        nonEnergyTiles = tileNetwork.Nodes.Where((e) => e.Value.tileType != TileStructure.TileType.EnergyTile).ToList();
        energyTiles = tileNetwork.Nodes.Where((e) => e.Value.tileType == TileStructure.TileType.EnergyTile).ToList();
        energizedNodeCount = energyTiles.Count;
        var bulbNodes = tileNetwork.Nodes.Where((e) => e.Value.tileType == TileStructure.TileType.BulbTile).ToList();
        totalBulbNodeCount = bulbNodes.Count;
    }

    public void Energize()
    {
        foreach (var node in nonEnergyTiles)
        {
            node.Value.energized = false;
        }

        energizedNodeCount = energyTiles.Count;

        foreach (var energyTile in energyTiles)
        {
            for (int i = 0; i < energyTile.Neighbours.Count; i++)
            {
                EnergizeNeighbours(energyTile);
            }
        }

        ScoreManager.Instance.SetLevelProgress(energizedNodeCount, tileNetwork.Nodes.Count);
        if (energizedBulbNodeCount == totalBulbNodeCount)
        {
            int nextLevel = LevelManager.Instance.currentLevel + 1;
            LevelManager.Instance.unlockedLevels[nextLevel % 3] = true;
            LevelManager.Instance.SaveLevelData();
            UIManager.Instance.ToggleToLevelCompleteScreen();
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
                    energizedNodeCount++;
                    neighbour.Value.energized = true;
                    EnergizeNeighbours(neighbour);
                }
            }
        }
    }


}

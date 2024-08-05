using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector] public Tilemap tilemap; // active tilemap
    public Dictionary<Tile, TileDataBase> tileDataList = new Dictionary<Tile, TileDataBase>();
    public Graph<TileDataBase> tileNetwork; // bi-directional graph system with nodes as tiles
    public static int energizedNodeCount; // total energized tiles
    [HideInInspector] public int totalBulbNodeCount; // total bulb tiles
    [HideInInspector] public int energizedBulbNodeCount; // energized bulb tiles
    public static GameStage gameStage;

    List<GraphNode<TileDataBase>> nonEnergyTiles, energyTiles;

    public enum GameStage
    {
        Menu, Play, LevelComplete
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => LevelManager.Instance.unlockedLevels.Count > 0);
        LevelManager.Instance.LevelSwitch(0, true);
        gameStage = GameStage.Play;
    }

    /// <summary>
    /// Reset data on start of level
    /// </summary>
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

    /// <summary>
    /// Energize the entire tileNetwork graph starting from energy node
    /// </summary>
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

        LevelManager.Instance.SetLevelProgress(energizedNodeCount, tileNetwork.Nodes.Count);
        if (energizedBulbNodeCount == totalBulbNodeCount)
        {
            int nextLevel = LevelManager.Instance.currentLevel + 1;
            LevelManager.Instance.unlockedLevels[nextLevel % 3] = true;
            UIManager.Instance.ChangeLevelsInteractabilityOnLock(LevelManager.Instance.unlockedLevels.ToArray());
            LevelManager.Instance.SaveLevelData();
            AudioManager.Instance.PlayOneShot(AudioManager.Instance.levelCompleteSound);
            UIManager.Instance.ToggleToLevelCompleteScreen();
        }
    }

    /// <summary>
    /// Recurssive function to loop through all neighbouring nodes for energizing
    /// </summary>
    /// <param name="tileDataNode">base node to wwhose neighbour have to be energized</param>
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

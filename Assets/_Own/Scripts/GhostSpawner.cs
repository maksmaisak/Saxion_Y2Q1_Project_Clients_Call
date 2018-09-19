using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class GhostSpawner: MyBehaviour
{
    [SerializeField] GameObject playerGhostPrefab;
    [SerializeField] int maximumGhostsPerScene = 20;
    [SerializeField] float minimumDistanceBetweenGhosts = 5.0f;

    private void Start()
    {
        Assert.IsNotNull(playerGhostPrefab);
        SpawnPlayerGhosts();
    }

    private void SpawnPlayerGhosts()
    {
        Leaderboard leaderboard = LeaderboardManager.instance.GetLeaderboardForCurrentMode();

        List<LeaderboardEntry> entries = 
            leaderboard.LeaderboardEntries
            .Where(x => x.playerDeathSceneName == SceneManager.GetActiveScene().name)
            .OrderByDescending(x => x.playerScore)
            .ToList();

        int index = 0;
        foreach (LeaderboardEntry entry in entries)
        {
            if(index > 0)
            {
                /// Don't spawn ghosts if they are too close to each other,
                /// Just the player with the highest score.
                if (entries[index - 1].playerDeathPosition.z - entry.playerDeathPosition.z < minimumDistanceBetweenGhosts)
                    continue;
            }

            GameObject ghost = Instantiate(playerGhostPrefab, entry.playerDeathPosition, Quaternion.identity);
            PlayerGhost playerGhost = ghost.GetComponent<PlayerGhost>();

            if (playerGhost == null)
                continue;

            playerGhost.playerGhostText.text = entry.playerName + "\n" + entry.playerScore;

            index++;
            if (index >= maximumGhostsPerScene) break;
        }
    }
}


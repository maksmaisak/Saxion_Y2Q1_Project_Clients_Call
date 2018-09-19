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
    [SerializeField] int maximumScoreTolerance = 2000;

    private void Start()
    {
        Assert.IsNotNull(playerGhostPrefab);
        SpawnPlayerGhosts();
    }

    private void SpawnPlayerGhosts()
    {
        Leaderboard leaderboard = LeaderboardManager.instance.GetLeaderboardForCurrentMode();

        int playerScore = GlobalState.instance.playerScore;

        List<LeaderboardEntry> entries = 
            leaderboard.LeaderboardEntries
            .Where(x => x.playerDeathSceneName == SceneManager.GetActiveScene().name && 
            (x.playerScore > playerScore && x.playerScore - playerScore < maximumScoreTolerance))
            .OrderByDescending(x => x.playerScore)
            .ToList();

        int index = 0;
        int skipCount = 0;
        int ghostSpawned = 0;
        foreach (LeaderboardEntry entry in entries)
        {
            if(index > 0)
            {
                /// Don't spawn ghosts if they are too close to each other,
                /// Just the player with the highest score.
                if (entries[index - 1].playerDeathPosition.z - entry.playerDeathPosition.z < minimumDistanceBetweenGhosts)
                {
                    skipCount++;
                    continue;
                }
            }

            GameObject ghost = Instantiate(playerGhostPrefab, entry.playerDeathPosition, Quaternion.identity);
            PlayerGhost playerGhost = ghost.GetComponent<PlayerGhost>();

            if (playerGhost == null)
                continue;

            playerGhost.playerGhostText.text = entry.playerName + "\n" + entry.playerScore;

            if (skipCount > 0)
                index += skipCount;
            else
                index++;

            skipCount = 0;
            ghostSpawned++;

            if (ghostSpawned >= maximumGhostsPerScene) break;
        }
    }
}


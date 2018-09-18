using UnityEngine;
using System.IO;
using System.Linq;

public static class LeaderboardSerializer
{
    public static void SaveLeaderboardToFile(Leaderboard leaderboard, string fileName)
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!File.Exists(filePath))
            File.Create(filePath).Close();

        string dataAsJson =
            JsonHelper.ToJson(leaderboard.LeaderboardEntries.ToArray(), true);

        File.WriteAllText(filePath, dataAsJson);
    }

    public static Leaderboard GetLeaderboardFromFile(string fileName)
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        Leaderboard leaderboard = new Leaderboard();

        if (File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath);
            // Pass the json to JsonUtility, and tell it to create a Leaderboard object from it

            var entries = JsonHelper.FromJson<LeaderboardEntry>(dataAsJson);

            if (entries.Length > 0)
                for (int i = 0; i < entries.Length; i++)
                    leaderboard.AddNewEntry(entries[i]);
        }
        else
        {
            Debug.Log("Cannot load leaderboard! File is non-existent!");
        }

        return leaderboard;
    }
}
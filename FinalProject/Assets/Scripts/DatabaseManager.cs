using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Collections.Generic;
//using Unity.VisualScripting.Dependencies.Sqlite;

public class HighScoreEntry
{
    public string playerName;
    public int score;
    public float completionTime;
}

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }

    private string dbPath;
    private IDbConnection connection;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeDatabase();
    }

    public void InitializeDatabase()
    {
        dbPath = Path.Combine(Application.persistentDataPath, "gameData.db");

        string connectionString = "URI=file:" + dbPath;

        connection = new SqliteConnection(connectionString);
        connection.Open();

        Debug.Log("Database connected: " + dbPath);

        CreateHighScoresTable();
    }

    void CreateHighScoresTable()
    {
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText =
                "CREATE TABLE IF NOT EXISTS HighScores (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "playerName TEXT, " +
                "score INTEGER, " +
                "completionTime REAL);";

            command.ExecuteNonQuery();
        }

        Debug.Log("HighScores table created or already exists");
    }

    public void SaveScore(string playerName, int score, float completionTime)
    {
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText =
                "INSERT INTO HighScores (playerName, score, completionTime) " +
                "VALUES (@name, @score, @time);";

            IDbDataParameter nameParam = command.CreateParameter();
            nameParam.ParameterName = "@name";
            nameParam.Value = playerName;
            command.Parameters.Add(nameParam);

            IDbDataParameter scoreParam = command.CreateParameter();
            scoreParam.ParameterName = "@score";
            scoreParam.Value = score;
            command.Parameters.Add(scoreParam);

            IDbDataParameter timeParam = command.CreateParameter();
            timeParam.ParameterName = "@time";
            timeParam.Value = completionTime;
            command.Parameters.Add(timeParam);

            command.ExecuteNonQuery();
        }

        Debug.Log("Score saved: " + playerName + " | " + score + " | " + completionTime);
    }

    public List<HighScoreEntry> GetTopHighScores()
    {
        List<HighScoreEntry> scores = new List<HighScoreEntry>();

        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText =
                "SELECT playerName, score, completionTime " +
                "FROM HighScores " +
                "ORDER BY score DESC " +
                "LIMIT 5;";

            using (IDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    HighScoreEntry entry = new HighScoreEntry();

                    entry.playerName = reader.GetString(0);
                    entry.score = reader.GetInt32(1);
                    entry.completionTime = (float)reader.GetDouble(2);

                    scores.Add(entry);
                }

                reader.Close();
            }
        }

        Debug.Log("Retrieved " + scores.Count + " high scores");

        return scores;
    }

    void OnApplicationQuit()
    {
        if (connection != null)
        {
            connection.Close();
        }
    }
}

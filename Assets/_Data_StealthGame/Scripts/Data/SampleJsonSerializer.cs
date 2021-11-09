using UnityEngine;
using System.IO;
/// <summary>
/// serialize dataset class to create json file for testing purpose
/// </summary>
public class SampleJsonSerializer : MonoBehaviour
{
    /// <summary>
    /// create json
    /// </summary>
    private void Start()
    {
        // create sample datasets
        int datasetsLength = 3;

        LevelDataSet[] sampleLevels = new LevelDataSet[datasetsLength];
        PlayerAbilityDataSet[] samplePlayerAbilities = new PlayerAbilityDataSet[datasetsLength];

        for (int i = 0; i < datasetsLength; i++)
        {
            LevelDataSet sampleLevel = new LevelDataSet();
            sampleLevel._level = i;
            sampleLevel._duration = 30f * (i + 1);
            sampleLevel._enemySpawnRate = 3;
            sampleLevel._maxEnemyNumberInField = 10 + 5 * i;

            PlayerAbilityDataSet samplePlayerAbility = new PlayerAbilityDataSet();
            samplePlayerAbility._level = i;
            samplePlayerAbility._unlockedNodeNumber = 5 * (i + 1);

            sampleLevels[i] = sampleLevel;
            samplePlayerAbilities[i] = samplePlayerAbility;
        }

        SpawnAreaDataSet[] spawnAreas = new SpawnAreaDataSet[(int)ExerciseType.LENGTH];

        for (int i = 0; i < spawnAreas.Length; i++)
        {
            SpawnAreaDataSet sampleSpawnArea = new SpawnAreaDataSet();
            sampleSpawnArea._exerciseType = (ExerciseType)i;
            sampleSpawnArea._isScannedDataPreferred = false;
            sampleSpawnArea._center = new Vector3(0, 0, 2);
            sampleSpawnArea._size = new Vector3(2, 2, 3);
            sampleSpawnArea._rotation = Quaternion.Euler(0, 0, 0);

            spawnAreas[i] = sampleSpawnArea;
        }
        
        // write json files
        WriteFile(JsonHelper.ToJson<LevelDataSet>(sampleLevels), Config._levelDataPath);
        WriteFile(JsonHelper.ToJson<PlayerAbilityDataSet>(samplePlayerAbilities), Config._playerAbilityDataPath);
        WriteFile(JsonHelper.ToJson<SpawnAreaDataSet>(spawnAreas), Config._spawnAreaDataPath);

        DebugLog.Info(this.ToString(), "successfully generated sample data");
    }

    /// <summary>
    /// write text as a file
    /// </summary>
    /// <param name="text"></param>
    /// <param name="path"></param>
    private void WriteFile(string text, string path)
    {
        File.WriteAllText(path, text);
    }
}

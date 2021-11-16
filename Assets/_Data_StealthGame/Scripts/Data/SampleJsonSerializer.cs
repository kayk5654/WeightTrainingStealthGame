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

        if (!File.Exists(Config._levelDataPath))
        {
            LevelDataSet[] sampleLevels = new LevelDataSet[datasetsLength];

            for (int i = 0; i < datasetsLength; i++)
            {
                LevelDataSet sampleLevel = new LevelDataSet();
                sampleLevel._level = i;
                sampleLevel._duration = 30f * (i + 1);
                sampleLevel._enemySpawnRate = 3;
                sampleLevel._maxEnemyNumberInField = 3 * (i + 1);
                sampleLevels[i] = sampleLevel;
            }

            WriteFile(JsonHelper.ToJson<LevelDataSet>(sampleLevels), Config._levelDataPath);
        }

        if (!File.Exists(Config._playerAbilityDataPath))
        {
            PlayerAbilityDataSet[] samplePlayerAbilities = new PlayerAbilityDataSet[datasetsLength];

            for (int i = 0; i < datasetsLength; i++)
            {
                PlayerAbilityDataSet samplePlayerAbility = new PlayerAbilityDataSet();
                samplePlayerAbility._level = i;
                samplePlayerAbility._unlockedNodeNumber = 5 * (i + 1);
                samplePlayerAbilities[i] = samplePlayerAbility;
            }

            WriteFile(JsonHelper.ToJson<PlayerAbilityDataSet>(samplePlayerAbilities), Config._playerAbilityDataPath);
        }

        if (!File.Exists(Config._spawnAreaDataPath))
        {
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

            WriteFile(JsonHelper.ToJson<SpawnAreaDataSet>(spawnAreas), Config._spawnAreaDataPath);
        }

        if (!File.Exists(Config._exerciseInputDataPath))
        {
            ExerciseInputDataSet[] exerciseInputs = new ExerciseInputDataSet[(int)ExerciseType.LENGTH];

            for(int i = 0; i < exerciseInputs.Length; i++)
            {
                ExerciseInputDataSet sampleExerciseInput = new ExerciseInputDataSet();
                sampleExerciseInput._exerciseType = (ExerciseType)i;
                sampleExerciseInput._inputType = ExerciseInputType.HeadTracking;
                sampleExerciseInput._peakHeightOffset = 0.2f;
                sampleExerciseInput._heightOffsetMargin = 0.12f;

                exerciseInputs[i] = sampleExerciseInput;
            }

            WriteFile(JsonHelper.ToJson<ExerciseInputDataSet>(exerciseInputs), Config._exerciseInputDataPath);
        }

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

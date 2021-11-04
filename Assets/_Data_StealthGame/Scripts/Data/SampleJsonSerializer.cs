using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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

            PlayerAbilityDataSet samplePlayerAbility = new PlayerAbilityDataSet();
            samplePlayerAbility._level = i;
            samplePlayerAbility._unlockedNodeNumber = 5 * (i + 1);

            sampleLevels[i] = sampleLevel;
            samplePlayerAbilities[i] = samplePlayerAbility;
        }
        
        // write json files
        WriteFile(Serialize(sampleLevels), Config._levelDataPath);
        WriteFile(Serialize(samplePlayerAbilities), Config._playerAbilityDataPath);
    }

    /// <summary>
    /// serialize a class instance and create json string
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    private string Serialize(object dataset)
    {
        Type type = dataset.GetType();
        if (!type.IsSerializable) { return null; }
        

        string json = "";
        return json;
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

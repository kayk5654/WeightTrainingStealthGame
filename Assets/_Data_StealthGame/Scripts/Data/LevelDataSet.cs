/// <summary>
/// dataset of a level
/// </summary>
[System.Serializable]
public class LevelDataSet : GameDataSetBase
{
    // duration of single gameplay
    public float _duration;

    // rate of spawning enemy per unit time
    public int _enemySpawnRate;

    // max spawned enemy number in the field
    public int _maxEnemyNumberInField;

    // enemy types to spawn
    //public Enemy[] _enemyTypes;
}

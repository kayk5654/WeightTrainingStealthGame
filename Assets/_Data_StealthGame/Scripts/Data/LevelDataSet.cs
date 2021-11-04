/// <summary>
/// dataset of a level
/// </summary>
[System.Serializable]
public class LevelDataSet
{
    // player's level
    public int _level;

    // duration of single gameplay
    public float _duration;

    // rate of spawning enemy per unit time
    public int _enemySpawnRate;

    // enemy types to spawn
    //public Enemy[] _enemyTypes;
}

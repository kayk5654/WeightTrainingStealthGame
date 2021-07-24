using UnityEngine;
/// <summary>
/// this contains all common parameters of in-game items
/// </summary>
public static class ItemConfig
{
    // speed of projectiles
    public static float _projectileSpeed = 0.03f;

    // maximum distance projectiles can travel
    public static float _maxProjectileDistance = 6.0f;

    // global shader variable for reveal area number
    public static string _revealAreaNumName = "_revealAreaNum";

    // shader keyword to read reveal area
    public static string _revealAreaShaderKeyword = "_READ_REVEAL_AREA";
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// enable enemies to search attack target
/// </summary>
public class EnemyAttackTargetSearcher : MonoBehaviour
{
    [SerializeField, Tooltip("compute shader to handle player's objects")]
    private ComputeShader _nodeComputeShader;

    // kernel name of FindNearestNode()
    private string _findNearestNodeKernelName = "FindNearestNode";

    // kernel info of FindNearestNode()
    private KernelParamsHandler __findNearestNodKernel;


    /// <summary>
    /// find nearest player's object to attack
    /// </summary>
    /// <param name="origin"></param>
    /// <returns></returns>
    public static Vector3 FindNearestTarget(Vector3 origin)
    {
        // TODO: search nearby player's object from origin using _nodeComputeShader
        return Vector3.forward + origin;
    }
}

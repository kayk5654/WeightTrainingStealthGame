using UnityEngine;
/// <summary>
/// contain basic parameters of compute shader
/// </summary>
public class KernelParamsHandler
{
    // name of kernel
    public string _name;
    // index of kernel
    public int _index;
    // thread group size x
    public int _x;
    // thread group size y
    public int _y;
    // thread group size z
    public int _z;

    public KernelParamsHandler(ComputeShader computeShader, string kernelName, int threadSizeX, int threadSizeY, int threadSizeZ)
    {
        _name = kernelName;
        _index = computeShader.FindKernel(_name);
        _x = threadSizeX;
        _y = threadSizeY;
        _z = threadSizeZ;
    }
}

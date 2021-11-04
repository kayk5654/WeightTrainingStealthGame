using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// read database related to the content from an external file
/// </summary>
public interface IDatabaseReader<T>
{
    /// <summary>
    /// read data from a file
    /// </summary>
    /// <returns></returns>
    T[] ReadData(string path);
}

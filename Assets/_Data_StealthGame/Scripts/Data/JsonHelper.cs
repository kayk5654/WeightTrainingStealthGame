using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// helper class to handle arrays and json
/// </summary>
public class JsonHelper
{
    /// <summary>
    /// deserialize a json to an array
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    public static T[] FromJson<T>(string json)
    {
        JsonWrapper<T> wrapper = JsonUtility.FromJson<JsonWrapper<T>>(json);
        return wrapper._item;
    }

    /// <summary>
    /// serialize an array to json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static string ToJson<T>(T[] array)
    {
        JsonWrapper<T> wrapper = new JsonWrapper<T>(array);
        return JsonUtility.ToJson(wrapper);
    }
}

/// <summary>
/// wrapper of array to convert to json
/// </summary>
/// <typeparam name="T"></typeparam>
[System.Serializable]
public class JsonWrapper<T>
{
    // array to contain
    public T[] _item;

    /// <summary>
    /// constructor
    /// </summary>
    /// <param name="array"></param>
    public JsonWrapper(T[] array)
    {
        _item = array;
    }
}

using System.IO;
using UnityEngine;
using UnityEngine.Networking;
/// <summary>
/// read text fron external files
/// </summary>
public class TextReader
{
    /// <summary>
    /// read data from a file
    /// </summary>
    /// <returns></returns>
    public string ReadData(string path)
    {
        string text = "";

#if UNITY_ANDROID
        if(path.Contains(Application.streamingAssetsPath))
        {
            var loadingRequest = UnityWebRequest.Get(path);
            loadingRequest.SendWebRequest();
            while (!loadingRequest.isDone)
            {
                if (loadingRequest.isNetworkError || loadingRequest.isHttpError)
                {
                    break;
                }
            }

            if (!loadingRequest.isNetworkError && !loadingRequest.isHttpError)
            {
                text = loadingRequest.downloadHandler.text;
            }
        }
        else
        {
            text = File.ReadAllText(path);
        }
        
#else
        text = File.ReadAllText(path);
#endif

        return text;
    }
}

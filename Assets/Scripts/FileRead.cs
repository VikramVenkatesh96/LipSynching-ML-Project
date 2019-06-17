using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileRead : MonoBehaviour
{
    
    public static string FindPathFromConfig(string filePath,string var)
    {
        string url;
        FileStream fileStream = new FileStream(filePath,FileMode.Open,FileAccess.Read);
        using (var streamReader = new StreamReader(fileStream)) {
            while ((url=streamReader.ReadLine())!=null)
            {
                url=url.Substring(url.IndexOf(var) + var.Length + 1);        //This will work only when var is typed the same as in the config file
                break;                                                       //else IndexOf() returns -1
            }
        }
        return url;

    }
}

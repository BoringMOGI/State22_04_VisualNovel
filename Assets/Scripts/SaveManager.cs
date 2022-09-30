using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string Path(string fileName)
    {
        // 프로그램 기본 경로/Save/파일이름.txt.
        return $"{Application.dataPath}/Save/{fileName}.txt";
    }

    // slot : 1 ~ 8
    public static void SaveData(Data data, int slot)
    {
        string path = Path($"Slot{slot}");
        Debug.Log(path);
        using(StreamWriter sw = new StreamWriter(path))
            sw.Write(data.GetJson());
    }
    public static bool LoadData(int slot, out string saveData)
    {
        string path = Path($"Slot{slot}");
        if(File.Exists(path))
        {
            using (StreamReader sr = new StreamReader(path))
            {
                saveData = sr.ReadToEnd();
                return true;
            }
        }
        else
        {
            saveData = string.Empty;
            return false;
        }
    }
}

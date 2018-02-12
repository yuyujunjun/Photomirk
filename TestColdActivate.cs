using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class TestColdActivate : MonoBehaviour {



    void Start()
    {
        string str = "";
        var objs = Resources.FindObjectsOfTypeAll(typeof( UnityEngine.Object));

        str = "total objs length = " +  objs.Length  + "\n";
        foreach (var obj in objs)
        {
            Type type = obj.GetType();
            str = "name = "+obj.name  + ", type = "  + type;
            str = "\n";
        }

        string path = Path.Combine(Application.persistentDataPath, "MMMMMMylogs.txt");
        File.WriteAllText(path, str);
    }

}

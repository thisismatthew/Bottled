using System.Text;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
 [ExecuteInEditMode]
public class VersionShaderGraphCheck : MonoBehaviour
{

public static bool Lo;

    [InitializeOnLoadMethod]
    public static void OnProjectLoadedInEditor()
    {
        Lo = true;

  
    }

     public void Update() {
    
 if (Lo) {
 var listRequest = Client.List(true);
        while (!listRequest.IsCompleted)
            Thread.Sleep(100);

 
        var packages = listRequest.Result;
        var text = new StringBuilder();
        foreach (var package in packages)
        {
            if (package.source == PackageSource.Registry)
                text.AppendLine($"{package.name}: {package.version} ");
        }
       if (text.ToString().Contains("shadergraph: 5.7.2")) {
transform.GetChild(0).gameObject.SetActive(true);
       }
       else
       {
     transform.GetChild(0).gameObject.SetActive(false); 
       
       }
//Debug.Log(text.ToString());
Lo = false;
 }
  
    }
    
}

#endif
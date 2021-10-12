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
public class VersionUnityCheck : MonoBehaviour
{

public static bool Lo;

    [InitializeOnLoadMethod]
    public static void OnProjectLoadedInEditor()
    {
     //   Debug.Log("Project loaded in Unity Editor");
        Lo = true;
    }

    public void Update() {
    
        if (Lo) {









#if UNITY_2019_1_OR_NEWER

 transform.GetChild(0).gameObject.SetActive(false); 
#else
    transform.GetChild(0).gameObject.SetActive(true);     
       
#endif

       Lo = false;
    }
    }
}
#endif
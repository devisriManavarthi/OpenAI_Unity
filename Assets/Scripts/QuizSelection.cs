using OpenAi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuizSelection : MonoBehaviour
{
    public GameObject selectionUI, gameUI;
    public Dropdown questionType;
    public static string question = "Generate any easy MCQ question related to Math. Display options by line break and correct option should always be a) option and the remaining 3 options incorrect e.g. for b) , c) and d) generate incorrect answers.";
    public static string image = "Mathematics";
    public OpenAiImageReplace openAiImageReplace;
    private bool isPrefab = false;


    public void UpdateImage()
    {
        EditorGUI.BeginDisabledGroup(openAiImageReplace.requestPending);
        if (!checkPromt())
        {
            string assetPath = AssetDatabase.GetAssetPath(openAiImageReplace.gameObject);
            isPrefab = assetPath != "";

            if (isPrefab)
            {
                EditorUtility.SetDirty(openAiImageReplace);
                GameObject prefabRoot = PrefabUtility.LoadPrefabContents(assetPath);
                OpenAiImageReplace prefabTarget = prefabRoot.GetComponent<OpenAiImageReplace>();
                prefabTarget.ReplaceImage(() =>
                {
                    PrefabUtility.SaveAsPrefabAsset(prefabRoot, assetPath, out bool success);
                    PrefabUtility.UnloadPrefabContents(prefabRoot);
                });
            }
            else
            {
                openAiImageReplace.ReplaceImage();
            }
        }
    }

    public bool checkPromt()
    {
        bool promptShown = false;

        if (Configuration.GlobalConfig.ApiKey == "")
        {
            Configuration.GlobalConfig = OpenAiApi.ReadConfigFromUserDirectory();
            promptShown = true;

        }

        return promptShown;
    }
}

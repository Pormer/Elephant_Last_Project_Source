using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Code.Utility
{
    /// <summary>
    /// Assets 폴더 안에서 원하는 타입의 에셋을 모두 찾는다.
    /// </summary>
    public static class EditorAssetsFinder
    {
        #if UNITY_EDITOR
        
        public static List<T> GetAllAssetsOfType<T>() where T : Object
        {
            List<T> assetList = new List<T>();
            
            string filter = "t:" + typeof(T).Name;
            string[] guids = AssetDatabase.FindAssets(filter);

            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                
                if(asset != null)
                    assetList.Add(asset);
            }
            
            return assetList;
        }
        
        #endif
        
    }
}
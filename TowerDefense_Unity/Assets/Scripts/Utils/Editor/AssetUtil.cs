using System.IO;
using UnityEditor;
using UnityEngine;

namespace Game.Utils.Editor
{
    public static class AssetUtil
    {
        private const string EXTENSION = ".asset"; 
        public static T Create<T>(string path, string assetName) where T : ScriptableObject
        {
            CreateDirectoryPathIfapplicable(path);
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path + assetName + EXTENSION);
            AssetDatabase.Refresh();
            return asset;
        }

        private static void CreateDirectoryPathIfapplicable(string path)
        {
            if (!Directory.Exists(path))
            {

                Directory.CreateDirectory(path);
            }
        }

        public static bool Exists(string path, string assetName)
        {
            return File.Exists(path + assetName + EXTENSION);
        }
    }
}
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.UI.InputBind
{
    internal static class InputBindingOverrideStore
    {
        private const string FolderName = "Settings";
        private const string FileName = "inputBindingOverrides.json";

        private static readonly List<InputActionAsset> RegisteredAssets = new();
        private static bool _didLoad;
        private static string _cachedJson;

        private static string FilePath => Path.Combine(Application.persistentDataPath, FolderName, FileName);

        public static void Register(InputActionAsset asset)
        {
            if (asset == null)
                return;

            if (!RegisteredAssets.Contains(asset))
            {
                RegisteredAssets.Add(asset);
            }

            EnsureLoaded();
            ApplyCachedOverrides(asset);
        }

        public static void Save(InputActionAsset sourceAsset)
        {
            if (sourceAsset == null)
                return;

            if (!RegisteredAssets.Contains(sourceAsset))
            {
                RegisteredAssets.Add(sourceAsset);
            }

            _cachedJson = sourceAsset.SaveBindingOverridesAsJson();

            var directoryPath = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            File.WriteAllText(FilePath, _cachedJson);

            for (int i = 0; i < RegisteredAssets.Count; i++)
            {
                var asset = RegisteredAssets[i];
                if (asset == null || asset == sourceAsset)
                    continue;

                ApplyCachedOverrides(asset);
            }
        }

        public static void ResetAll()
        {
            _cachedJson = string.Empty;
            _didLoad = true;

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }

            for (int i = 0; i < RegisteredAssets.Count; i++)
            {
                var asset = RegisteredAssets[i];
                if (asset == null)
                    continue;

                asset.RemoveAllBindingOverrides();
            }
        }

        private static void EnsureLoaded()
        {
            if (_didLoad)
                return;

            _didLoad = true;
            _cachedJson = File.Exists(FilePath) ? File.ReadAllText(FilePath) : string.Empty;
        }

        private static void ApplyCachedOverrides(InputActionAsset asset)
        {
            if (asset == null)
                return;

            asset.RemoveAllBindingOverrides();
            if (!string.IsNullOrWhiteSpace(_cachedJson))
            {
                asset.LoadBindingOverridesFromJson(_cachedJson);
            }
        }
    }
}

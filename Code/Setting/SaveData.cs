using System.IO;
using UnityEngine;

namespace Code.Setting
{
    public abstract class SaveData
    {
        [field: SerializeField] public string FileName { get; private set; }
        
        public virtual void Save(string filePath)
        {
            var json = JsonUtility.ToJson(this, true);
            // Debug.Log($"Saving {filePath} / Json : {json}");
            var directoryPath = System.IO.Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (!File.Exists(filePath))
                File.Create(filePath).Dispose();
            File.WriteAllText(filePath, json);
        }

        public virtual void Load(string path)
        {
            if (File.Exists(path))
            {
                var file = File.ReadAllText(path);
                JsonUtility.FromJsonOverwrite(file, this);
            }
            else
            {
                Save(path);
            }
        }
    }
}

namespace Code.Setting
{
    public enum ResolutionType
    {
        r1920x1080,
        r1920x1200,
        r2560x1080,
    }

    [System.Serializable]
    public class DisplayData : SaveData
    {
        public ResolutionType Resolution;
        public bool IsFullScreen;

        public void SetResolution(ResolutionType resolution) => Resolution = resolution;
        public void SetFullScreen(bool isFullScreen) => IsFullScreen = isFullScreen;
    }
}
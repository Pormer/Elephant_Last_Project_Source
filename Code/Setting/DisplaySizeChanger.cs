using System;
using UnityEngine;

namespace Code.Setting
{
    public static class DisplaySizeChanger
    {
        public static void ChangeDisplaySize(bool fullScreen, ResolutionType resolution)
        {
            int width,  height;
            switch (resolution)
            {
                case ResolutionType.r1920x1080:
                    width = 1920;
                    height = 1080;
                    break;
                case ResolutionType.r1920x1200:
                    width = 1920;
                    height = 1280;
                    break;
                case ResolutionType.r2560x1080:
                    width = 2560;
                    height = 1080;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(resolution), resolution, null);
            }
            Screen.SetResolution(width, height, fullScreen);
        }
    }
}
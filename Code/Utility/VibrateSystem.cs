using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Utility
{
    public static class VibrateSystem
    {
        // 디테일한건 나중에
        
        /// <summary>
        /// 진동을 편하게 실행 할 수 있는 코드
        /// </summary>
        /// <param name="lowFrequency">묵직한 진동</param>
        /// <param name="highFrequency">섬세한 진동</param>
        /// <param name="duration">진동 시간</param>
        public static async void Vibrate(float lowFrequency, float highFrequency, float duration)
        {
            try
            {
                Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
                await Awaitable.WaitForSecondsAsync(duration);
                Gamepad.current.SetMotorSpeeds(0, 0);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        
        // VibrateSystem.Vibrate(0.1f, 0.5f, 0.3f);
    }
}
using System.Collections.Generic;
using EventSystem;
using Input;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Code.Timelines
{
    public class TimelineManager : MonoBehaviour
    {
        [SerializeField] private PlayerInputSO playerInput;
        [SerializeField] private UIInputSO uiInput;
        [Space]
        [SerializeField] private GameEventChannelSO timelineChannel;
        [SerializeField] private PlayableDirector director;
        [Space]
        [SerializeField] private TimelineInfoSO[] timelineInfoData;
        
        private Dictionary<TimelineType, TimelineAsset> _timelineInfoDict;

        private void Awake()
        {
            _timelineInfoDict = new Dictionary<TimelineType, TimelineAsset>();
            
            foreach (var timelineInfo in timelineInfoData)
                _timelineInfoDict.Add(timelineInfo.timelineType, timelineInfo.timelineAssets);
            
            timelineChannel.AddListener<PlayTimelineEvent>(HandlePlayTimeline);
            timelineChannel.AddListener<StopTimelineEvent>(HandleStopTimeline);
        }

        private void OnDestroy()
        {
            timelineChannel.RemoveListener<PlayTimelineEvent>(HandlePlayTimeline);
            timelineChannel.RemoveListener<StopTimelineEvent>(HandleStopTimeline);
        }

        private void HandlePlayTimeline(PlayTimelineEvent evt)
        {
            if (_timelineInfoDict.TryGetValue(evt.timelineType, out var playableAsset) == false)
            {
                Debug.LogError($"Invalid timeline action type");
                return;
            }
            
            director.playableAsset = playableAsset;
            director.Play();
            
            //Timeline 실행 중일 땐 키 안 먹히도록 설정
            playerInput.SetEnabled(false);
            uiInput.SetEnabled(false);
        }
        
        private void HandleStopTimeline(StopTimelineEvent evt)
        {
            director.Stop();
            
            //Timeline 종료 시 다시 활성화
            playerInput.SetEnabled(true);
            uiInput.SetEnabled(true);
        }

        public void OnEndTimeline()
        {
            playerInput.SetEnabled(true);
            uiInput.SetEnabled(true);
        }
    }
}
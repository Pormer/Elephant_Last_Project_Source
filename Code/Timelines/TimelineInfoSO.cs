using UnityEngine;
using UnityEngine.Timeline;

namespace Code.Timelines
{
    [CreateAssetMenu(fileName = "TimelineInfo", menuName = "SO/Timeline/TimelineInfoSO", order = 0)]
    public class TimelineInfoSO : ScriptableObject
    {
        public TimelineType timelineType;
        public TimelineAsset timelineAssets;
    }
}
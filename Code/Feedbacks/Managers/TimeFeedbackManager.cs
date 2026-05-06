using System.Collections;
using EventSystem;
using UnityEngine;

namespace Code.Feedbacks.Managers
{
    public class TimeFeedbackManager : FeedbackManager
    {
        [SerializeField] private float defaultTimeScale = 1.0f;
        
        private void Awake()
        {
            feedbackChannel.AddListener<TimeStopFeedbackEvent>(HandleTimeStopEvent);
        }

        private void OnDestroy()
        {
            feedbackChannel.RemoveListener<TimeStopFeedbackEvent>(HandleTimeStopEvent);
        }

        private void HandleTimeStopEvent(TimeStopFeedbackEvent evt)
        {
            //UI를 킬때면 멈춰줘야함
            Time.timeScale = evt.changeTimeScale;
            StartCoroutine(WaitToDefaultTimeScale(evt.duration));
        }

        private IEnumerator WaitToDefaultTimeScale(float duration)
        {
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = defaultTimeScale;
        }
    }
}
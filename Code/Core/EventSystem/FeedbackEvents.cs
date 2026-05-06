using Ami.BroAudio;
using DG.Tweening;
using Feedbacks.Contexts;
using GondrLib.ObjectPool.RunTime;
using UnityEngine;

namespace EventSystem
{
    public static class FeedbackEvents
    {
        public static readonly DamageTextFeedbackEvent DamageTextFeedbackEvent = new DamageTextFeedbackEvent();
        public static readonly EffectFeedbackEvent EffectFeedbackEvent = new EffectFeedbackEvent();
        public static readonly ImpulseFeedbackEvent ImpulseFeedbackEvent = new ImpulseFeedbackEvent();
        public static readonly TimeStopFeedbackEvent TimeStopFeedbackEvent = new TimeStopFeedbackEvent();
        public static readonly CameraZoomFeedbackEvent CameraZoomFeedbackEvent = new CameraZoomFeedbackEvent();
        public static readonly BGMChangeEvent BGMChangeEvent = new BGMChangeEvent();
        public static readonly SetDefaultBGMEvent SetDefaultBGMEvent = new SetDefaultBGMEvent();
        public static readonly UseDefaultBGMEvent UseDefaultBGMEvent = new UseDefaultBGMEvent();
        public static readonly PauseBGMEvent PauseBGMEvent = new PauseBGMEvent();
    }

    public class DamageTextFeedbackEvent : GameEvent
    {
        public int damage;
        public Vector3 position;
        public bool isCritical;

        public DamageTextFeedbackEvent Initializer(int amount, Vector3 pos, bool critical = false)
        {
            damage = amount;
            position = pos;
            isCritical = critical;
            return this;
        }
    }

    public class EffectFeedbackEvent : GameEvent
    {
        public PoolItemSO effectPoolItem;
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        
        public EffectFeedbackEvent Initializer(PoolItemSO item, Vector3 pos, Quaternion rot, Vector3 size = new())
        {
            effectPoolItem = item;
            position = pos;
            rotation = rot;
            scale = size;
            return this;
        }
    }
    
    public class ImpulseFeedbackEvent : GameEvent
    {
        public ImpulseFeedbackDataSO impulseData;
        
        public ImpulseFeedbackEvent Initializer(ImpulseFeedbackDataSO data)
        {
            impulseData = data;
            return this;
        }
    }
    
    public class TimeStopFeedbackEvent : GameEvent
    {
        public float changeTimeScale;
        public float duration;
        
        public TimeStopFeedbackEvent Initializer(float amount, float time)
        {
            changeTimeScale = amount;
            duration = time;
            return this;
        }
    }
    
    public class CameraZoomFeedbackEvent : GameEvent
    {
        public float addZoomValue;
        public float duration;
        public Ease easeType;
        
        public CameraZoomFeedbackEvent Initializer(float amount, float time, Ease type)
        {
            addZoomValue = amount;
            duration = time;
            easeType = type;
            return this;
        }
    }
    
    //BGM을 바꾼다.
    public class BGMChangeEvent : GameEvent
    {
        public SoundID targetBgmID;
        
        public BGMChangeEvent Initializer(SoundID id)
        {
            targetBgmID = id;
            return this;
        }
    }

    //기본 BGM을 설정한다.
    public class SetDefaultBGMEvent : GameEvent
    {
        public SoundID defaultBgmID;
        public bool isImmediate;
        
        public SetDefaultBGMEvent Initializer(SoundID id, bool isDirect)
        {
            defaultBgmID = id;
            isImmediate = isDirect;
            return this;
        }
    }
    
    //기본 BGM을 설정한다.
    public class UseDefaultBGMEvent : GameEvent { }

    public class PauseBGMEvent : GameEvent
    {
        public bool isStopBGM;

        public PauseBGMEvent Initializer(bool paused)
        {
            isStopBGM = paused;
            return this;
        }
    }
}
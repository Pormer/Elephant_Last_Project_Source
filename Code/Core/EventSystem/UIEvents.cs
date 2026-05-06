using System;
using System.Collections.Generic;
using Code.UI.InputBind;
using Code.UI.UIStateMachine;
using UnityEngine;
using UnityEngine.UI;

namespace EventSystem
{
    public static class UIEvents
    {
        public static readonly SetUIInputEnableEvent SetUIInputEnableEvent = new SetUIInputEnableEvent();
        public static readonly UIChangeEvent UIStateChangeEvent = new UIChangeEvent();
        public static readonly UIStateChangeEndedEvent UIStateChangeEndedEvent = new UIStateChangeEndedEvent();
        public static readonly SubUIChangeEvent SubUIChangeEvent = new SubUIChangeEvent();
        public static readonly UIFadeInOutEvent UIFadeInOutEvent = new UIFadeInOutEvent();
        public static readonly UIFadeStartEvent UIFadeStartEvent = new UIFadeStartEvent();
        public static readonly UIFadeCompleteEvent UIFadeCompleteEvent = new UIFadeCompleteEvent();
        public static readonly SelectedElementChange FocusElementChange = new SelectedElementChange();
        public static readonly InputChangeEvent InputChangeEvent = new InputChangeEvent();
        public static readonly SubUICancelEvent SubUICancelEvent = new SubUICancelEvent();
        public static readonly InteractionShowEvent InteractionShowEvent = new InteractionShowEvent();
        public static readonly InteractionHideEvent InteractionHideEvent = new InteractionHideEvent();
    }

    public class SetUIInputEnableEvent : GameEvent
    {
        public bool Enabled;

        public SetUIInputEnableEvent Initializer(bool enabled)
        {
            Enabled = enabled;
            return this;
        }
    }

    public class AlphaScreenEvent : GameEvent
    {
        public bool isPop;

        public AlphaScreenEvent Initializer(bool isPop)
        {
            this.isPop = isPop;
            return this;
        }
    }

    public class UIChangeEvent : GameEvent
    {
        public UIStateType NewStateType { get; set; }

        public UIChangeEvent Initializer(UIStateType newStateType = UIStateType.None)
        {
            NewStateType = newStateType;
            return this;
        }
    }

    public class SubUIChangeEvent : UIChangeEvent
    {
    }

    public class UIStateChangeEndedEvent : GameEvent
    {
        public IUIState NewState { get; set; }

        public UIStateChangeEndedEvent Initializer(IUIState newState)
        {
            NewState = newState;
            return this;
        }
    }

    public class UIFadeInOutEvent : GameEvent
    {
        public Action Callback;

        public UIFadeInOutEvent Initializer(Action callback)
        {
            Callback = callback;
            return this;
        }
    }

    public class UIFadeStartEvent : GameEvent
    {
        public bool IsFadeIn;
        public Action Callback;

        public UIFadeStartEvent Initializer(bool isFadeIn, Action callback = null)
        {
            IsFadeIn = isFadeIn;
            Callback = callback;
            return this;
        }
    }

    public class UIFadeCompleteEvent : GameEvent
    {
        public bool IsFadeIn { get; set; }

        public UIFadeCompleteEvent Initializer(bool isFadeIn)
        {
            IsFadeIn = isFadeIn;
            return this;
        }
    }

    public class SelectedElementChange : GameEvent
    {
        public Selectable SelectedObject { get; set; }

        public SelectedElementChange Initializer(Selectable selectedObject)
        {
            SelectedObject = selectedObject;
            return this;
        }
    }

    public class InputChangeEvent : GameEvent
    {
        public bool PlayerEnabled;
        public bool UIEnabled;

        public InputChangeEvent Initializer(bool playerEnabled, bool uiEnabled)
        {
            PlayerEnabled = playerEnabled;
            UIEnabled = uiEnabled;
            return this;
        }
    }

    public class SubUICancelEvent : GameEvent { }

    public class InteractionShowEvent : GameEvent
    {
        public string Text;
        public Transform Target;
        public float Offset;

        public InteractionShowEvent Initializer(string text, Transform target, float offset = 1)
        {
            Text = text;
            Target = target;
            Offset = offset;
            return this;
        }
    }

    public class InteractionHideEvent : GameEvent
    {
        public Transform Target;

        public InteractionHideEvent Initializer(Transform target)
        {
            Target = target;
            return this;
        }
    }
}
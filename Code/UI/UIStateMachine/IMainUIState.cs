namespace Code.UI.UIStateMachine
{
    /// <summary>
    /// This Interface is only for Views
    /// </summary>
    public interface IMainUIState : IUIState
    {
        bool DoesStop { get; }
    }
}

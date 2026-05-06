using System;

namespace Code.UI.Interface
{
    public interface IUIBindable
    {
        public Enum BindKey { get; }
        public void Bind(object v);
    }
}

using UnityEngine;

namespace Code.Contexts
{
    public interface IContextSetUp<in T> where T : IContext
    {
        public void SetUp(T context);
    }
}
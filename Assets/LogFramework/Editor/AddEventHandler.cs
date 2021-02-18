using System;

namespace LogFramework
{
    public interface IAddEventHandler
    {
        void Invoke(Type classType, object attribute);
    }

    public class AddEventHandler<T> : IAddEventHandler where T : LogAttribute
    {
        public delegate void AddDel(Type classType, T attribute);

        private AddDel del = null;

        public AddEventHandler(AddDel _del)
            : base()
        {
            del = _del;
        }

        public void Invoke(Type classType, object attribute)
        {
            del(classType, attribute as T);
        }
    }
}

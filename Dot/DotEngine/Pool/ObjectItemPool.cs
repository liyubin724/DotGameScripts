using System.Collections.Generic;

namespace DotEngine.Pool
{
    public delegate T CreateItem<T>() where T : class;
    public delegate void GetItemFromPool<T>(T data) where T : class;
    public delegate void ReleaseItemToPool<T>(T data) where T : class;

    public class ObjectItemPool<T> where T : class
    {
        private Stack<T> m_Stack = new Stack<T>();

        private CreateItem<T> createItem;
        private GetItemFromPool<T> getItem;
        private ReleaseItemToPool<T> releaseItem;

        public ObjectItemPool(CreateItem<T> create,GetItemFromPool<T> get,ReleaseItemToPool<T> release,int preloadCount = 0)
        {
            createItem = create;
            getItem = get;
            releaseItem = release;

            if(preloadCount>0)
            {
                for(int i =0;i<preloadCount;++i)
                {
                    T element = createItem();
                    m_Stack.Push(element);
                }
            }
        }

        public T GetItem()
        {
            T element = null;
            if(m_Stack.Count == 0)
            {
                element = createItem();
            }else
            {
                element = m_Stack.Pop();
            }

            getItem?.Invoke(element);

            return element;
        }

        public void ReleaseItem(T element)
        {
            if(element!=null)
            {
                releaseItem?.Invoke(element);
                m_Stack.Push(element);
            }
        }

        public void Clear()
        {
            m_Stack.Clear();
            createItem = null;
            getItem = null;
            releaseItem = null;
        }

        private T CreateElement()
        {
            T element = createItem.Invoke();
            return element;
        }
    }
}

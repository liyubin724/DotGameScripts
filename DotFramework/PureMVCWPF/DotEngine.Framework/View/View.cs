using System.Collections.Generic;

namespace DotEngine.Framework
{
    public class View: IView
    {
        protected readonly Dictionary<string, IViewController> mediatorMap;
        
        protected IFacade Facade
        {
            get
            {
                return Framework.Facade.GetInstance();
            }
        }

        public View()
        {
            mediatorMap = new Dictionary<string, IViewController>();
            InitializeView();
        }

        protected virtual void InitializeView()
        {
        }

        public virtual void RegisterMediator(IViewController mediator)
        {
            if(mediator!=null && !string.IsNullOrEmpty(mediator.MediatorName) && !mediatorMap.ContainsKey(mediator.MediatorName))
            {
                mediatorMap.Add(mediator.MediatorName, mediator);

                var interests = mediator.ListNotificationInterests();
                if (interests!=null && interests.Length > 0)
                {
                    foreach (var interest in interests)
                    {
                        Facade.RegisterObserver(interest, mediator.HandleNotification);
                    }
                }
                mediator.OnRegister();
            }
        }

        public virtual IViewController RetrieveMediator(string mediatorName)
        {
            return mediatorMap.TryGetValue(mediatorName, out var mediator) ? mediator : null;
        }

        public virtual IViewController RemoveMediator(string mediatorName)
        {
            if(mediatorMap.TryGetValue(mediatorName,out IViewController mediator))
            {
                mediatorMap.Remove(mediatorName);

                var interests = mediator.ListNotificationInterests();
                foreach (var interest in interests)
                {
                    Facade.RemoveObserver(interest, mediator.HandleNotification);
                }

                mediator.OnRemove();
            }
            return mediator;
        }

        public virtual bool HasMediator(string mediatorName)
        {
            return mediatorMap.ContainsKey(mediatorName);
        }
    }
}

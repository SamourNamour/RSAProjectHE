using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MTV.EventDequeuer.Contracts.Faults;

namespace MTV.EventDequeuer.Contracts.Service
{
    [ServiceContract(Namespace = "http://MTV.EventDequeuer.Contracts",
                      SessionMode = SessionMode.Required,
                      CallbackContract = typeof(IEventDequeuerCallback))]
    public interface IEventDequeuer
    {
        [OperationContract(IsOneWay = false)]
        [FaultContract(typeof(EventDequeuerException))]
        void Subscribe(Guid subscriptionId, string[] eventNames);

        [OperationContract(IsOneWay = true)]
        void EndSubscription(Guid subscriptionId);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MTV.EventDequeuer.Contracts.Data;
namespace MTV.EventDequeuer.Contracts.Service
{
    public interface IEventDequeuerCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveStreamingResult(RealTimeEventMsg streamingResult);
    }
}

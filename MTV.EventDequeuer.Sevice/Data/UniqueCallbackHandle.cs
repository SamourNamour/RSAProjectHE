using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTV.EventDequeuer.Contracts.Service;

namespace MTV.EventDequeuer.Service.Data
{
    public class UniqueCallbackHandle
    {
        public UniqueCallbackHandle(Guid callbackSessionId, IEventDequeuerCallback callback)
        {
            this.CallbackSessionId = callbackSessionId;
            this.Callback = callback;
        }

        public Guid CallbackSessionId { get; private set; }
        public IEventDequeuerCallback Callback { get; private set; }
    }
}

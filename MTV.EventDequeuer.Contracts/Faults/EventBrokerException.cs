using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTV.EventDequeuer.Contracts.Faults
{
    [DataContract]
    public class EventDequeuerException
    {
        [DataMember]
        public String Message
        {
            get;
            set;
        }

        public EventDequeuerException(String message)
        {
            Message = message;
        }
    }
}

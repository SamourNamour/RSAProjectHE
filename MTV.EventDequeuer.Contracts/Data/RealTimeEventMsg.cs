using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace MTV.EventDequeuer.Contracts.Data
{
    [DataContract]
    public class RealTimeEventMsg
    {

        public RealTimeEventMsg()
        {
            
        }

        public RealTimeEventMsg(DateTime date, string eventName)
         {
          
            this.Date = date;
            this.EventName = eventName;
         }


         [DataMember]
         public DateTime? Date { get; set; }

         [DataMember]
         public string EventName { get; set; }

    }
}

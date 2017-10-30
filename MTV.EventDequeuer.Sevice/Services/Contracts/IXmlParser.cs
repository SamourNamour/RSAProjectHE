using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTV.EventDequeuer.Contracts.Data;

namespace MTV.EventDequeuer.Service.Services.Contracts
{
    
        public interface IXmlParser
        {
            RealTimeEventMsg ParseXmlMsg(string msg);
        }
    
}

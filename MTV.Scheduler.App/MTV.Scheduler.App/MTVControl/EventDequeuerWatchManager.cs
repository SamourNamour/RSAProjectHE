using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Scheduler.App.MTVControl
{
    public class EventDequeuerWatchManager
    {

        private int counter;
        private readonly EventDequeuerWatch eventDequeuerWatch;

        public EventDequeuerWatchManager() : this(EventDequeuerWatch.Instance) { }

         public EventDequeuerWatchManager(EventDequeuerWatch eventDequeuerWatch)
        {
            this.eventDequeuerWatch = eventDequeuerWatch;
            eventDequeuerWatch.Subscribe();
        }

        public void Register()
        {
            //Do nothing, but is crucially important to establish comms
        }

        public void Dispose()
        {
            eventDequeuerWatch.Close();
        }

    }
}

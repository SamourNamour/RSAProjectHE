using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Library.Core.Proxy
{
    public partial class EventDequeuerProxy : 
        System.ServiceModel.DuplexClientBase<MTV.EventDequeuer.Contracts.Service.IEventDequeuer>,
        MTV.EventDequeuer.Contracts.Service.IEventDequeuer
    {

        public EventDequeuerProxy(System.ServiceModel.InstanceContext callbackInstance) :
            base(callbackInstance)
        {
        }

        public EventDequeuerProxy(System.ServiceModel.InstanceContext callbackInstance, 
            string endpointConfigurationName) :
            base(callbackInstance, endpointConfigurationName)
        {
        }

        public EventDequeuerProxy(System.ServiceModel.InstanceContext callbackInstance, 
            string endpointConfigurationName, string remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public EventDequeuerProxy(System.ServiceModel.InstanceContext callbackInstance, 
            string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
            base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public EventDequeuerProxy(System.ServiceModel.InstanceContext callbackInstance, 
            System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
            base(callbackInstance, binding, remoteAddress)
        {
        }


        public void Subscribe(Guid subscriptionId, string[] eventNames)
        {
            base.Channel.Subscribe(subscriptionId, eventNames);
        }

        public void EndSubscription(Guid subscriptionId)
        {
            base.Channel.EndSubscription(subscriptionId);
        }
    }
   
}

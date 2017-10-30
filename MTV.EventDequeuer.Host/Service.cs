using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Configuration;
using MTV.EventDequeuer.Common;
using System.IO;

namespace MTV.EventDequeuer.Host
{
    public partial class Service : ServiceBase
    {
        private static ServiceHost eventDequeuerHost;
        public Service()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Service.StartService();
        }

        public static void StartService()
        {
            try
            {
                StartServiceHost();
            }
            catch (Exception ex)
            {
                LogManager.Log.Error(ex);
            }
        }


        protected override void OnStop()
        {
            try
            {
                Service.StopServiceHost(eventDequeuerHost);
            }
            catch (Exception e)
            {
                LogManager.Log.Error(string.Format("While attempting to stop MTV.EventDequeuer.Service.EventDequeuer service type {0} the following exception was thrown {1}.", this.GetType().FullName, e.ToString()));
            }
        }


        private static void StartServiceHost()
        {
            try
            {

                // We will recreate the service host here to be sure we don't have a service in a faulted state
                ServiceHost serviceHost = new ServiceHost(typeof(MTV.EventDequeuer.Service.EventDequeuer));
                LogManager.Log.Info("Attempting to open MTV.EventDequeuer.Service.EventDequeuer service.");
                serviceHost.Open();
                LogServiceHostInfo(serviceHost);
                serviceHost.Faulted += ServiceHost_Faulted;
                try { File.WriteAllText(Program.AppDataPath + "status.txt", "Started"); }
                catch { };
            }
            catch (ObjectDisposedException objDisposedEx)
            {
                LogManager.Log.Error(objDisposedEx);
            }
            catch (InvalidOperationException invalidOpEx)
            {
                LogManager.Log.Error(invalidOpEx);
            }
            catch (CommunicationObjectFaultedException commObjFaultedEx)
            {
                LogManager.Log.Error(commObjFaultedEx);
            }
            catch (System.ServiceProcess.TimeoutException tmOutEx)
            {
                LogManager.Log.Error(tmOutEx);
            }
            catch (Exception ex)
            {
                LogManager.Log.Error(ex);
            }
        }

        private static void StopServiceHost(ServiceHostBase serviceHost)
        {
            if (serviceHost.State != CommunicationState.Closed)
            {
                if (serviceHost.State != CommunicationState.Faulted)
                {
                    serviceHost.Close();
                    LogManager.Log.InfoFormat(String.Format("{0} Closed.", serviceHost.Description.Name));
                    try { File.WriteAllText(Program.AppDataPath + "status.txt", "Closed"); }
                    catch { };
                }
                else
                {
                    serviceHost.Abort();
                    LogManager.Log.InfoFormat(String.Format("{0} Aborted.", serviceHost.Description.Name));
                    try { File.WriteAllText(Program.AppDataPath + "status.txt", "Aborted"); }
                    catch { };
                }
            }

            File.WriteAllText(Program.AppDataPath + "status.txt", "Stoped"); 

          
            
        }


        private static void RestartServiceHost(ServiceHost serviceHost)
        {
            StopServiceHost(serviceHost);
            StartServiceHost();
        }


        private static void LogServiceHostInfo(ServiceHostBase serviceHost)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendFormat("'{0}' Starting", serviceHost.Description.Name);
            strBuilder.Append(Environment.NewLine);

            // Behaviors
            var annotation = serviceHost.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            strBuilder.AppendFormat("Concurrency Mode = {0}", annotation.ConcurrencyMode);
            strBuilder.Append(Environment.NewLine);
            strBuilder.AppendFormat("InstanceContext Mode = {0}", annotation.InstanceContextMode);
            strBuilder.Append(Environment.NewLine);

            // Endpoints
            strBuilder.Append("The following endpoints are exposed:");
            strBuilder.Append(Environment.NewLine);
            foreach (ServiceEndpoint endPoint in serviceHost.Description.Endpoints)
            {
                strBuilder.AppendFormat("{0} at {1} with {2} binding; "
                       , endPoint.Contract.ContractType.Name
                       , endPoint.Address
                       , endPoint.Binding.Name);
                strBuilder.Append(Environment.NewLine);
            }

            // Metadata
            var metabehaviour = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
            if (metabehaviour != null)
            {
                if (metabehaviour.HttpGetEnabled)
                {
                    if (metabehaviour.HttpsGetUrl != null)
                    {
                        strBuilder.AppendFormat("Metadata enabled at {0}", serviceHost.BaseAddresses[0]);
                    }
                    else
                    {
                        strBuilder.AppendFormat("Metadata enabled at {0}", metabehaviour.HttpGetUrl);
                    }
                }
                if (metabehaviour.HttpsGetEnabled)
                    strBuilder.AppendFormat(" and {0}.", metabehaviour.HttpsGetUrl);
                if (metabehaviour.ExternalMetadataLocation != null)
                    strBuilder.AppendFormat(" Metadata can be found externally at {0}", metabehaviour.ExternalMetadataLocation);
            }

            LogManager.Log.Info(string.Format("{0} \n {1}", serviceHost.Description.Name, strBuilder.ToString()));
        }


        private static void ServiceHost_Faulted(Object sender, EventArgs e)
        {
            var serviceHost = sender as ServiceHost;
            LogManager.Log.Warn(string.Format("{0} Faulted.  Attempting Restart.", serviceHost.Description.Name));
            try { File.WriteAllText(Program.AppDataPath + "status.txt", "Restarting"); }
            catch { };
            RestartServiceHost(serviceHost);
        }


    }
}

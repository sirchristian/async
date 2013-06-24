using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Asynk.Library.Queues.WCF.Server
{
    /// <summary>
    /// Implements an AsynkQueue using WCF
    /// </summary>
    [ServiceContract(Namespace = Constants.WebSite)]
    public class WCFQueue : IAsynkQueue
    {
        #region IAsynkQueue Members
        
        /// <summary>
        /// Start an Asynk Operation
        /// </summary>
        /// <param name="item"></param>
        [OperationContract]
        public void Push(AsynkQueueItem item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        [OperationContract]
        public AsynkQueueItem Pop()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public int Count()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Category
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}

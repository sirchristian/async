using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Messaging;
using System.Transactions;

namespace Asynk.Library.Queues
{
    class MSMQQueue : IAsynkQueue
    {
        private static readonly string _queuePath = "";

        #region IAsynkQueue Members

        public void Push(AsynkQueueItem item)
        {
            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(_queuePath))
                MessageQueue.Create(_queuePath, true);

            using (TransactionScope ts = new TransactionScope(TransactionScopeOption.Required))
            {
            }

            throw new NotImplementedException();
        }

        public AsynkQueueItem Pop()
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

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

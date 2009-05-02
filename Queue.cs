using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spaetzel.QueueDA
{
    public static class Queue
    {
        /// <summary>
        /// Locks the top message
        /// </summary>
        /// <returns></returns>
        public static Message GetLockTopMessage()
        {
            Message result;

            using (QueueDBWriteInterface db = new QueueDBWriteInterface())
            {
                result = db.GetLockTopMessage();
            }

            return result;

        }

        public static int SaveMessage(Message message)
        {
            int result;
            using (QueueDBWriteInterface db = new QueueDBWriteInterface())
            {
                result = db.SaveMessage(message);
            }

            return result;
        }


        public static void DeleteLockedMessage(int messageId)
        {
            using (QueueDBWriteInterface db = new QueueDBWriteInterface())
            {
                    db.DeleteLockedMessage(messageId);
            }
        }


        public static int ClearExpiredLocks()
        {
            int result;
            using (QueueDBWriteInterface db = new QueueDBWriteInterface())
            {
                result = db.ClearExpiredLocks();
            }

            return result;
        }

        public static Message GetLockMessage(int messageId)
        {
            Message result;

            using (QueueDBWriteInterface db = new QueueDBWriteInterface())
            {
                result = db.GetLockMessage(messageId);
            }

            return result;
        }
    }
}

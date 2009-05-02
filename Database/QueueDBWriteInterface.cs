using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spaetzel.UtilityLibrary;
using Spaetzel.UtilityLibrary.Database;
using MySql.Data.MySqlClient;
using System.Collections;

namespace Spaetzel.QueueDA
{
    class QueueDBWriteInterface : QueueDBInterface
    {
        internal int SaveMessage(Message message)
        {
            // Verify that the message doesn't already exist
            string query;

            if (message.ValidDate != null)
            {
                query = "SELECT id FROM queuemessage WHERE action = ?action AND mainId = ?mainId AND secondaryId = ?secondaryId AND validDate=?validDate";
            }
            else
            {
                query = "SELECT id FROM queuemessage WHERE action = ?action AND mainId = ?mainId AND secondaryId = ?secondaryId AND validDate IS NULL";
            }

            var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("?action", message.Action);
            command.Parameters.AddWithValue("?mainId", message.MainId);
            command.Parameters.AddWithValue("?secondaryId", message.SecondaryId);
            command.Parameters.AddWithValue("?validDate", message.ValidDate);


            using( MySqlDataReader reader = command.ExecuteReader() )
            {
                if( reader.HasRows )
                {
                    throw new Exception( String.Format("Message with action '{0}', main id {1} secondary Id {2} already exists", message.Action, message.MainId, message.SecondaryId ) );
                }
            }

            query = "INSERT INTO queuemessage (Action, MainId, SecondaryId, DateAdded, Priority, ValidDate ) VALUES( ?Action, ?MainId, ?SecondaryId, ?DateAdded, ?Priority, ?ValidDate )";


            command = new MySqlCommand(query, connection);

            message.FillCommandParameters(command);
           // command.Parameters.AddWithValue("?tableName", Config.TableName);
            command.Parameters.AddWithValue("?dateAdded", DateTime.Now);

            command.ExecuteNonQuery();

            query = "SELECT LAST_INSERT_ID() FROM queuemessage";

            command = new MySqlCommand(query, connection);

            int messageId = Convert.ToInt32(command.ExecuteScalar());

            return messageId;

        }

        internal void DeleteLockedMessage(int id)
        {
            string query = "DELETE FROM queuemessage WHERE Id = ?Id AND locked = 1";

            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("?tableName", Config.TableName);
            command.Parameters.AddWithValue("?Id", id);

            int affectedRows = command.ExecuteNonQuery();

            if (affectedRows == 0)
            {
                throw new Exception(string.Format("Message with Id {0} is not locked", id));
            }
        }

        internal Message GetLockTopMessage()
        {
            MySqlCommand command = new MySqlCommand("queue_getLockMessage", connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            Message result;


            using (MySqlDataReader reader = command.ExecuteReader())
            {

                if (reader.Read())
                {
                    result = new Message();
                    result.FillFromReader(reader);
                }
                else
                {
                    // No more messages to process
                    result = null;
                }
            }

            return result;





        }

        internal int ClearExpiredLocks()
        {
            string query = "UPDATE queuemessage SET DateLocked = NULL, locked = 0 WHERE locked = 1 AND DateLocked < CURRENT_TIMESTAMP - INTERVAL ?timeSpan MINUTE";

            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("?timeSpan", Config.LockTime.Minutes);

            return command.ExecuteNonQuery();
        }

        internal Message GetLockMessage(int messageId)
        {
           // TODO: lock the message so that one message can't be processed twice
            string query = "SELECT * FROM queuemessage WHERE id = ?id";

            MySqlCommand command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("?id", messageId);

            Message result;


            using (MySqlDataReader reader = command.ExecuteReader())
            {

                if (reader.Read())
                {
                    result = new Message();
                    result.FillFromReader(reader);
                }
                else
                {
                    // No more messages to process
                    result = null;
                }
            }

            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;
using Spaetzel.UtilityLibrary;

namespace Spaetzel.QueueDA
{
    public class Message
    {
        private DateTime _dateAdded;
        private DateTime _dateLocked;
        private int _id;
        private bool _locked;
        public DateTime? ValidDate { get; set; }

        public Message()
        {
            SecondaryId = "";
        }

        public bool Locked
        {
            get
            {
                return _locked;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }
        public string Action { get; set; }
        public string MainId { get; set; }
        public string SecondaryId { get; set; }
        public DateTime DateAdded
        {
            get
            {
                return _dateAdded;
            }
        }
        public DateTime DateLocked
        {
            get
            {
                return _dateLocked;
            }
        }
        public int Priority { get; set; }

        internal void FillFromReader( MySqlDataReader reader )
        {
            _id = DBInterface.GetReaderInt(reader, "Id");
            Action = DBInterface.GetReaderString(reader, "Action");
            MainId = DBInterface.GetReaderString(reader, "MainId");
            SecondaryId = DBInterface.GetReaderString(reader, "SecondaryId");
            _dateAdded = DBInterface.GetReaderDateTime(reader, "DateAdded");
            _dateLocked = DBInterface.GetReaderDateTime(reader, "DateLocked");
            Priority = DBInterface.GetReaderInt(reader, "Priority");
            _locked = DBInterface.GetReaderBool(reader, "Locked");
            ValidDate = DBInterface.GetReaderDateTimeNull(reader, "ValidDate");
        }

        internal void FillCommandParameters(MySqlCommand command)
        {
            command.Parameters.AddWithValue("?Id", Id);
            command.Parameters.AddWithValue("?Action", Action);
            command.Parameters.AddWithValue("?MainId", MainId);
            command.Parameters.AddWithValue("?SecondaryId", SecondaryId);
            command.Parameters.AddWithValue("?Priority", Priority);
            command.Parameters.AddWithValue("?ValidDate", ValidDate);
        }
    }
}

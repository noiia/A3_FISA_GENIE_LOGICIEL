using System;

namespace Config
{
    public class SaveJob
    {
        private string _Source;
        private string _Destination;
        private int _Id;
        private string _Name;
        private DateTime _LastSave;
        private DateTime _Created;
        private string _Type;

        public SaveJob( int id, string name, string source, string destination, DateTime lastSave, DateTime created, string type)
        {
            _Source = source;
            _Destination = destination;
            _Id = id;
            _Name = name;
            _LastSave = lastSave;
            _Created = created;
            _Type = type;
        }

        public string Source => _Source;

        public string Destination => _Destination;

        public int Id => _Id;

        public string Name => _Name;

        public DateTime LastSave => _LastSave;

        public DateTime Created => _Created;

        public string Type => _Type;
    }

}
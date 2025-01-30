using System;

namespace EasySave
{
    public class SaveJob
    {
        private string source;
        private string destination;
        private int id;
        private string name;
        private DateTime lastSave;
        private DateTime created;

        public SaveJob(int id, string name, string source, string destination, DateTime lastSave, DateTime created)
        {
            this.source = source;
            this.destination = destination;
            this.id = id;
            this.name = name;
            this.lastSave = lastSave;
            this.created = created;
        }

        public string Source
        {
            get => source;
            set => source = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string Destination
        {
            get => destination;
            set => destination = value ?? throw new ArgumentNullException(nameof(value));
        }

        public int Id
        {
            get => id;
            set => id = value;
        }

        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public DateTime LastSave
        {
            get => lastSave;
            set => lastSave = value;
        }

        public DateTime Created
        {
            get => created;
            set => created = value;
        }
    }

}
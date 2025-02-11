using System;

namespace CLI
{
    public class Commande
    {
        private string _CommandeName;
        private string[] _CommandeAlias;

        public Commande(string commandeName, string[] commandeAlias)
        {
            _CommandeName = commandeName;
            _CommandeAlias = commandeAlias;
        }

        public void Run(string[] args)
        {
            this.Action(args);
        }

        public bool IsCall(string commandeName)
        {
            if (commandeName.Equals(_CommandeName, StringComparison.OrdinalIgnoreCase))
            {
                return true;   
            }
            else
            {
                foreach (string alias in _CommandeAlias)
                {
                    if (commandeName.Equals(_CommandeName, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;   
                    }
                }
                return false;
            }
        }

        public virtual void Action(string[] args)
        {}
    }
}
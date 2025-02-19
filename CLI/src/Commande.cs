using System;
using Config;
using Job.Config;


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

        public void Run(Configuration config, string[] args)
        {
            try
            {
                this.Action(config, args);
            }
            catch (Exception e)
            {
                this.Action(args);
            }
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
        {
            throw new NotImplementedException();
        }

        public virtual void Action(Configuration config, string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
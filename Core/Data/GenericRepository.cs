namespace Core.Data
{
    using Microsoft.Practices.EnterpriseLibrary.Data;
    using System.Data.Common;
    public class GenericRepository
    {
        #region Database Objects

        public Database db;
        public DbCommand dbCommand;
        #endregion

        #region Constructor

        public GenericRepository()
        {
            DatabaseProviderFactory factory = new DatabaseProviderFactory();
            db = factory.Create("xConnection");
        }

        #endregion
    }
}

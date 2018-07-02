using System.ComponentModel.Composition;
using System.Data.Entity;
using Resolver;
using DataModel.DbOperation;

namespace DataModel
{
    [Export(typeof(IComponent))]
    public class DependencySolution : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IDbOperation, DbOperation.DbOperation>();
        }
    }
}

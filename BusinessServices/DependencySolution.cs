using System.ComponentModel.Composition;
using DataModel;
using DataModel.DbOperation;
using Resolver;

namespace BusinessServices
{
    [Export(typeof(IComponent))]
    public class DependencySolution : IComponent
    {
        public void SetUp(IRegisterComponent registerComponent)
        {
            registerComponent.RegisterType<IContactServices, ContactServices>();

        }
    }
}

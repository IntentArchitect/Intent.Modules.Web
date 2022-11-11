using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;
using TestApi.Domain.Entities.Common;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Entities.DomainEntityState", Version = "1.0")]

namespace TestApi.Domain.Entities
{

    public partial class Person : IPerson
    {
        public Person()
        {
        }

        private Guid? _id = null;

        /// <summary>
        /// Get the persistent object's identifier
        /// </summary>
        public virtual Guid Id
        {
            get { return _id ?? (_id = IdentityGenerator.NewSequentialId()).Value; }
            set { _id = value; }
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.RoslynWeaver.Attributes;
using Intent.Modules.Common;
using Intent.Modules.Common.Templates;
using Intent.Modules.Common.Types.Api;

[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]
[assembly: DefaultIntentManaged(Mode.Merge)]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Merge)]
    public class ModuleModel : IMetadataModel, IHasStereotypes, IHasName, IHasFolder<IFolder>, IFolder
    {
        protected readonly IElement _element;
        public const string SpecializationType = "Module";

        [IntentManaged(Mode.Ignore)]
        public ModuleModel(IElement element)
        {
            _element = element;
        }

        [IntentManaged(Mode.Fully)]
        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        [IntentManaged(Mode.Fully)]
        public string Id => _element.Id;

        [IntentManaged(Mode.Fully)]
        public string Name => _element.Name;
        public IElementApplication Application => _element.Application;

        [IntentManaged(Mode.Fully)]
        public IList<ModelDefinitionModel> ModelDefinitions => _element.ChildElements
            .GetElementsOfType(ModelDefinitionModel.SpecializationTypeId)
            .Select(x => new ModelDefinitionModel(x))
            .ToList();

        public string Comment => _element.Comment;

        [IntentManaged(Mode.Fully)]
        public IList<ComponentModel> Components => _element.ChildElements
            .GetElementsOfType(ComponentModel.SpecializationTypeId)
            .Select(x => new ComponentModel(x))
            .ToList();


        public string GetModuleName()
        {
            return Name.RemoveSuffix("Module");
        }

        [IntentManaged(Mode.Fully)]
        public bool Equals(ModuleModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ModuleModel)obj);
        }

        [IntentManaged(Mode.Fully)]
        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }


        [IntentManaged(Mode.Fully)]
        public IElement InternalElement => _element;

        [IntentManaged(Mode.Fully)]
        public override string ToString()
        {
            return _element.ToString();
        }

        [IntentManaged(Mode.Fully)]
        public IList<FormGroupDefinitionModel> FormGroups => _element.ChildElements
            .GetElementsOfType(FormGroupDefinitionModel.SpecializationTypeId)
            .Select(x => new FormGroupDefinitionModel(x))
            .ToList();
        public const string SpecializationTypeId = "cac14331-198a-4f9a-bbb9-171eb0bd4efe";

        [IntentManaged(Mode.Ignore)]
        public ModuleModel(IElement element, string requiredType = SpecializationType)
        {
            if (!requiredType.Equals(element.SpecializationType, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new Exception($"Cannot create a '{GetType().Name}' from element with specialization type '{element.SpecializationType}'. Must be of type '{SpecializationType}'");
            }
            _element = element;
        }

        [IntentManaged(Mode.Ignore)]
        public IFolder Folder => InternalElement.GetParentFolder();

        [IntentManaged(Mode.Ignore)]
        string IFolder.Name => GetModuleName().ToKebabCase();

        public RoutingModel Routing => _element.ChildElements
                    .Where(x => x.SpecializationType == RoutingModel.SpecializationType)
                    .Select(x => new RoutingModel(x))
                    .SingleOrDefault();

        public IList<ModuleModel> Modules => _element.ChildElements
                    .GetElementsOfType(ModuleModel.SpecializationTypeId)
                    .Select(x => new ModuleModel(x))
                    .ToList();

        public IList<FolderModel> Folders => _element.ChildElements
                    .GetElementsOfType(FolderModel.SpecializationTypeId)
                    .Select(x => new FolderModel(x))
                    .ToList();

        public ModuleModel GetParentModule()
        {
            return InternalElement.GetModule();
        }

        public bool IsRootModule()
        {
            return GetParentModule() == null;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.Types.Api;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.Api.ApiElementModel", Version = "1.0")]

namespace Intent.Modelers.WebClient.Angular.Api
{
    [IntentManaged(Mode.Merge, Signature = Mode.Merge)]
    public class ComponentModel : IMetadataModel, IHasStereotypes, IHasName, IHasFolder<IFolder>, IElementWrapper
    {

        [IntentManaged(Mode.Ignore)]
        public ComponentModel(IElement element, string requiredType = SpecializationType)
        {
            _element = element;
        }

        [IntentManaged(Mode.Ignore)]
        public IFolder Folder => InternalElement.GetParentFolder();


        [IntentManaged(Mode.Fully)]
        public IEnumerable<IStereotype> Stereotypes => _element.Stereotypes;

        [IntentManaged(Mode.Fully)]
        public string Id => _element.Id;

        [IntentManaged(Mode.Fully)]
        public string Name => _element.Name;
        public string Comment => _element.Comment;

        public ModuleModel Module => _element.GetModule();

        [IntentManaged(Mode.Fully)]
        public bool Equals(ComponentModel other)
        {
            return Equals(_element, other?._element);
        }

        [IntentManaged(Mode.Fully)]
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ComponentModel)obj);
        }

        [IntentManaged(Mode.Fully)]
        public override int GetHashCode()
        {
            return (_element != null ? _element.GetHashCode() : 0);
        }
        protected readonly IElement _element;
        public const string SpecializationType = "Component";

        [IntentManaged(Mode.Fully)]
        public IElement InternalElement => _element;

        [IntentManaged(Mode.Fully)]
        public override string ToString()
        {
            return _element.ToString();
        }

        [IntentManaged(Mode.Fully)]
        public IList<ComponentCommandModel> Commands => _element.ChildElements
            .GetElementsOfType(ComponentCommandModel.SpecializationTypeId)
            .Select(x => new ComponentCommandModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<ComponentModelModel> Models => _element.ChildElements
            .GetElementsOfType(ComponentModelModel.SpecializationTypeId)
            .Select(x => new ComponentModelModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public IList<ModelDefinitionModel> ModelDefinitions => _element.ChildElements
            .GetElementsOfType(ModelDefinitionModel.SpecializationTypeId)
            .Select(x => new ModelDefinitionModel(x))
            .ToList();

        [IntentManaged(Mode.Fully)]
        public ComponentViewModel View => _element.ChildElements
            .GetElementsOfType(ComponentViewModel.SpecializationTypeId)
            .Select(x => new ComponentViewModel(x))
            .SingleOrDefault();
        public const string SpecializationTypeId = "b1c481e1-e91e-4c29-9817-00ab9cad4b6b";

        public IList<ComponentInputModel> Inputs => _element.ChildElements
            .GetElementsOfType(ComponentInputModel.SpecializationTypeId)
            .Select(x => new ComponentInputModel(x))
            .ToList();

        public IList<ComponentOutputModel> Outputs => _element.ChildElements
            .GetElementsOfType(ComponentOutputModel.SpecializationTypeId)
            .Select(x => new ComponentOutputModel(x))
            .ToList();

    }

    [IntentManaged(Mode.Fully)]
    public static class ComponentModelExtensions
    {

        public static bool IsComponentModel(this ICanBeReferencedType type)
        {
            return type != null && type is IElement element && element.SpecializationTypeId == ComponentModel.SpecializationTypeId;
        }

        public static ComponentModel AsComponentModel(this ICanBeReferencedType type)
        {
            return type.IsComponentModel() ? new ComponentModel((IElement)type) : null;
        }
    }
}
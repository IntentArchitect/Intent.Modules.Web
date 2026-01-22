using DotNetBackEndService.Application.Common.Interfaces;
using Intent.RoslynWeaver.Attributes;
using MediatR;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.MediatR.CommandModels", Version = "1.0")]

namespace DotNetBackEndService.Application.Products.UpdateProduct
{
    public class UpdateProductCommand : IRequest, ICommand
    {
        public UpdateProductCommand(Guid id, string name, string description, string sku, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            Sku = sku;
            Price = price;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
    }
}
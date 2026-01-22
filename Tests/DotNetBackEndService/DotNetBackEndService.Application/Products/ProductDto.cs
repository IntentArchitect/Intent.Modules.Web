using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.Application.Dtos.DtoModel", Version = "1.0")]

namespace DotNetBackEndService.Application.Products
{
    public class ProductDto
    {
        public ProductDto()
        {
            Name = null!;
            Description = null!;
            Sku = null!;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }

        public static ProductDto Create(Guid id, string name, string description, string sku, decimal price)
        {
            return new ProductDto
            {
                Id = id,
                Name = name,
                Description = description,
                Sku = sku,
                Price = price
            };
        }
    }
}
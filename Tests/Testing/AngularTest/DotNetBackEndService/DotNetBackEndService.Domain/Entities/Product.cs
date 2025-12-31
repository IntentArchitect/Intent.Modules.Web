using Intent.RoslynWeaver.Attributes;

[assembly: IntentTemplate("Intent.Entities.DomainEntity", Version = "2.0")]

namespace DotNetBackEndService.Domain.Entities
{
    public class Product
    {
        public Product()
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
    }
}
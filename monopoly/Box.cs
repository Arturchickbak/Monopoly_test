using System;

namespace WarehouseApp
{
    public class Box
    {
        public Guid Id { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? ProductionDate { get; set; }
        public Guid PalletId { get; set; }

        public Box(Guid id, decimal width, decimal height, decimal depth, DateTime? expirationDate, DateTime? productionDate)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
            ExpirationDate = expirationDate;
            ProductionDate = productionDate;

            if (ProductionDate.HasValue && !ExpirationDate.HasValue)
            {
                ExpirationDate = ProductionDate.Value.AddDays(100);
            }
        }

        // Объем коробки
        public decimal Volume => Width * Height * Depth;

        // Вес коробки - равен объему
        public decimal Weight => Volume;
    }
}

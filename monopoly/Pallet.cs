using System;
using System.Collections.Generic;
using System.Linq;

namespace WarehouseApp
{
    public class Pallet
    {
        public Guid Id { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Depth { get; set; }
        public List<Box> Boxes { get; set; } = new List<Box>();

        // Срок годности паллеты - минимальная дата истечения срока годности среди коробок
        public DateTime? ExpirationDate => Boxes.Any() ? Boxes.Min(box => box.ExpirationDate) : null;

        // Вес паллеты - сумма весов всех коробок
        public decimal Weight => Boxes.Sum(box => box.Weight);

        // Объем паллеты - сумма объемов всех коробок плюс объем самой паллеты
        public decimal Volume => Boxes.Sum(box => box.Volume) + (Width * Height * Depth);

        public Pallet(Guid id, decimal width, decimal height, decimal depth)
        {
            Id = id;
            Width = width;
            Height = height;
            Depth = depth;
        }

        // Проверка, что коробка помещается по ширине и глубине паллеты
        public bool CanFitBox(Box box) => box.Width <= Width && box.Depth <= Depth;
    }
}

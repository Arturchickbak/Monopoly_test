using System;

namespace WarehouseApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = @"Data Source=DESKTOP-J06U7HI;Initial Catalog=monopoly;Integrated Security=True";
            var warehouseService = new WarehouseService(connectionString);

            Console.WriteLine("Паллеты, сгруппированные по сроку годности и отсортированные по весу:");
            var groupedPallets = warehouseService.GetPalletsGroupedByExpiration();
            foreach (var pallet in groupedPallets)
            {
                Console.WriteLine($"Pallet ID: {pallet.Id}, Expiration Date: {pallet.ExpirationDate:dd.MM.yyyy}, Weight box: {pallet.Weight}, Volume: pallet {pallet.Volume:F2}");
            }

            Console.WriteLine("\nТоп-3 паллеты с наибольшим сроком годности, отсортированные по объему:");
            var topPallets = warehouseService.GetTopPalletsByBoxExpiration();
            foreach (var pallet in topPallets)
            {
                Console.WriteLine($"Top Pallet ID: {pallet.Id}, Expiration Date: {pallet.ExpirationDate:dd.MM.yyyy}, Volume: {pallet.Volume:F2}");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace WarehouseApp
{
    public class WarehouseService
    {
        private readonly string _connectionString;

        public WarehouseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void LoadBoxesForPallets(List<Pallet> pallets)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Width, Height, Depth, ExpirationDate, ProductionDate, PalletId FROM Boxes", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var box = new Box(
                            reader.GetGuid(0),
                            reader.GetDecimal(1),
                            reader.GetDecimal(2),
                            reader.GetDecimal(3),
                            reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                            reader.IsDBNull(5) ? (DateTime?)null : reader.GetDateTime(5)
                        );

                        var palletId = reader.GetGuid(6);
                        var pallet = pallets.FirstOrDefault(p => p.Id == palletId);
                        if (pallet != null && pallet.CanFitBox(box))
                        {
                            pallet.Boxes.Add(box);
                        }
                    }
                }
            }
        }

        public List<Pallet> GetPalletsGroupedByExpiration()
        {
            var pallets = new List<Pallet>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT Id, Width, Height, Depth FROM Pallets", connection);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pallet = new Pallet(
                            reader.GetGuid(0),
                            reader.GetDecimal(1),
                            reader.GetDecimal(2),
                            reader.GetDecimal(3)
                        );

                        pallets.Add(pallet);
                    }
                }
            }

            LoadBoxesForPallets(pallets);

            return pallets
                .Where(p => p.ExpirationDate.HasValue)
                .OrderBy(p => p.ExpirationDate)
                .ThenBy(p => p.Weight)
                .ToList();
        }

        public List<Pallet> GetTopPalletsByBoxExpiration()
        {
            var pallets = GetPalletsGroupedByExpiration();
            return pallets
                .OrderByDescending(p => p.ExpirationDate) // Сначала сортируем по сроку годности в порядке убывания
                .ThenBy(p => p.Volume) // Затем по возрастанию объема
                .Take(3) // Берем только топ-3
                .ToList();
        }
    }
}

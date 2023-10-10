using HistoryService.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HistoryService.DbContext;

public class HistoryContext : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<CalculationHistory> Histories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=history.db");
    }
}
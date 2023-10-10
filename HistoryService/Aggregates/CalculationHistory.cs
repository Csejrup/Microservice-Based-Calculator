namespace HistoryService.Aggregates;

public class CalculationHistory
{
    public int Id { get; set; }
    public string Operation { get; set; }
    public DateTime Timestamp { get; set; }
}

namespace MasterCurrencyData.Models
{
    public class Currency
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Symbole { get; set; }
        public string? Value { get; set; }
        public DateTime? Data { get; set; }
    }
}

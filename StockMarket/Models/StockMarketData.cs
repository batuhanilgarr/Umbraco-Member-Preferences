namespace StockMarket.Models
{
    public class StockMarketData
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Last { get; set; }
        public decimal Close { get; set; }
        public decimal PreviousClose { get; set; }
        public DateTime DateTime { get; set; }
        public decimal DailyChange { get; set; }
        public decimal DailyChangePercent { get; set; }
        public decimal Volatility { get; set; }
        public string DailyDirection { get; set; }
    }
}

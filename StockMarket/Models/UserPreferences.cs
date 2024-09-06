namespace StockMarket.Models
{
    public class UserPreferences
    {
        public int UserPreferencesId { get; set; }
        public string UserId { get; set; }
        public string SelectedCheckboxes { get; set; }

    }

    public class PreferencesModel
    {
        public List<string> Checkboxes { get; set; }
    }
}

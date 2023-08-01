namespace KronoMata.Web.Models
{
    public class ChartDataSeries<T>
    {
        public string Name { get; set; } = String.Empty;
        public List<T> Data { get; set; }

    }
}

namespace DiffingWebApiApplication
{
    public class DiffingResultData
    {
        public DiffingResultType DiffingResult { get; set; }
        public List<Difference>? Differences { get; set; }
    }
}
namespace Solvberget.Domain.Documents
{
    public class DocumentRating
    {
        public string Source { get; set; }
        public int MaxScore { get; set; }
        public double Score { get; set; }
        public string SourceUrl { get; set; }
    }
}
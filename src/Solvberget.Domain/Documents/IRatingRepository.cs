namespace Solvberget.Domain.Documents
{

    public interface IRatingRepository
    {
        DocumentRating GetDocumentRating(string id);
    }
}
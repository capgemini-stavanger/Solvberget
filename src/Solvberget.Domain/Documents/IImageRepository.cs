namespace Solvberget.Domain.Documents
{
    public interface IImageRepository
    {
        string GetDocumentImage(string id);
        string GetDocumentThumbnailImage(string id, string size);
    }
}
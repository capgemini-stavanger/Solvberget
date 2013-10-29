using System.Collections.Generic;

namespace Solvberget.Domain.Static
{
    public interface INewsRepository
    {
        List<NewsItem> GetNewsItems(int? limitCount);
    }
}
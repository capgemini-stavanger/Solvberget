using System.Collections.Generic;

namespace Solvberget.Core.DTO
{
    public class LibrarylistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<DocumentDto> Documents { get; set; }
    }
}
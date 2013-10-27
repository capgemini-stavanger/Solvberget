﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Solvberget.Domain.Abstract.V2;
using Solvberget.Domain.DTO;

namespace Solvberget.Domain.Implementation.V2
{

    public class LibraryListRepository : ILibraryListRepository
    {
        private LibraryListXmlRepository _staticLists;
        private LibraryListDynamicRepository _dynamicLists;

        public LibraryListRepository(LibraryListXmlRepository staticLists, LibraryListDynamicRepository dynamicLists)
        {
            _staticLists = staticLists;
            _dynamicLists = dynamicLists;
        }

        public IEnumerable<LibraryList> GetAll()
        {
            return _staticLists.GetLists().Union(_dynamicLists.GetLists());
        }

        public LibraryList Get(string id)
        {
            if (id.StartsWith("static_")) return _staticLists.GetLists().FirstOrDefault(l => l.Id == id);
            return _dynamicLists.GetLists().FirstOrDefault(l => l.Id == id);
        }
    }
}
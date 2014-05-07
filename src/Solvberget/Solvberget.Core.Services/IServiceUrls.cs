using System;

namespace Solvberget.Core.Services
{
    public interface IServiceUrls
    {
        string  ServiceUrl { get;  }
        string ServiceUrl_Search { get;  }
        string ServiceUrl_Document { get;  }
        string ServiceUrl_Rating { get;  }
        string ServiceUrl_Review { get;  }
        string ServiceUrl_Events { get;  }
        string ServiceUrl_Event { get;  }
        string ServiceUrl_MediaImage { get; }
    }
}
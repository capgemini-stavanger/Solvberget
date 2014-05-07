using System.Collections.Generic;
using System.Threading.Tasks;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class EventService : IEventService
    {
        private readonly DtoDownloader _downloader;
        private readonly IServiceUrls _serviceUrls;

        public EventService(DtoDownloader downloader, IServiceUrls serviceUrls)
        {
            _downloader = downloader;
            _serviceUrls = serviceUrls;
        }

        public async Task<IList<EventDto>> GetList()
        {
            var response = await _downloader.DownloadList<EventDto>(_serviceUrls.ServiceUrl + _serviceUrls.ServiceUrl_Events);
            return response.Results;

        }

        public async Task<EventDto> Get(int eventId)
        {
            return await _downloader.Download<EventDto>(_serviceUrls.ServiceUrl + string.Format(_serviceUrls.ServiceUrl_Event, eventId));
       }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.DTOs.Deprecated.DTO;
using Solvberget.Core.Properties;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    class SearchService : ISearchService
    {
        private readonly IStringDownloader _stringDownloader;

        public SearchService(IStringDownloader stringDownloader)
        {
            _stringDownloader = stringDownloader;
        }

        public async Task<IEnumerable<DocumentDto>>  Search(string query)
        {
            try
            {
                var response = await _stringDownloader.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_Search, query));
                return JsonConvert.DeserializeObject<List<DocumentDto>>(response);
            }
            catch (Exception)
            {
                return new List<DocumentDto>
                {
                    new DocumentDto
                    {
                        Title = "Kunne ikke hente resultater"
                    }
                };
            }
        }

        public async Task<DocumentDto> Get(string docId)
        {
            try
            {
                var response = await _stringDownloader.Download(Resources.ServiceUrl + string.Format(Resources.ServiceUrl_Document, docId));
                var doc = JsonConvert.DeserializeObject<DocumentDto>(response);

                switch (doc.Type)
                {
                    case "Cd":
                        return JsonConvert.DeserializeObject<CdDto>(response);
                    case "Film":
                        return JsonConvert.DeserializeObject<FilmDto>(response);
                    case "Book":
                        return JsonConvert.DeserializeObject<BookDto>(response);
                    case "Journal":
                        return JsonConvert.DeserializeObject<JournalDto>(response);
                    case "Game":
                        return JsonConvert.DeserializeObject<GameDto>(response);
                    case "SheetMusic":
                        return JsonConvert.DeserializeObject<SheetMusicDto>(response);
                    case "AudioBook":
                        return JsonConvert.DeserializeObject<AudioBookDto>(response);
                    default:
                        return doc;
                }

            }
            catch (Exception)
            {
                return new DocumentDto
                {
                    Title = "Kunne ikke laste dokument"
                };
            }
        }
    }
}

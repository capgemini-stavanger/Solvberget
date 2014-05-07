﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Core.Services
{
    public class SearchService : ISearchService
    {
        private readonly DtoDownloader _dtos;
        private readonly IStringDownloader _rawHttp;

        private readonly IServiceUrls _serviceUrls;

        public SearchService(DtoDownloader stringDownloader, IStringDownloader rawHttp, IServiceUrls serviceUrls)
        {
            _dtos = stringDownloader;
            _rawHttp = rawHttp;
            _serviceUrls = serviceUrls;
        }

        public async Task<IEnumerable<DocumentDto>>  Search(string query)
        {
            var result = await _dtos.DownloadList<DocumentDto>(_serviceUrls.ServiceUrl + string.Format(_serviceUrls.ServiceUrl_Search, query));
                        return result.Results;
        }

        public async Task<DocumentDto> Get(string docId)
        {
            try
            {
                var response = await _rawHttp.Download(_serviceUrls.ServiceUrl + string.Format(_serviceUrls.ServiceUrl_Document, docId));
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
            catch (Exception e)
            {
                return new DocumentDto
                {
                    Success = false,
                    Reply = "Kunen ikke laste dokumentet",
                    Title = "Kunne ikke laste dokumentet"
                };
            }
        }

        public async Task<DocumentRatingDto> GetRating(string docId)
        {
            var response = await _dtos.Download<DocumentRatingDto>(_serviceUrls.ServiceUrl + string.Format(_serviceUrls.ServiceUrl_Rating, docId));
            return response;
        }

        public async Task<DocumentReviewDto> GetReview(string docId)
        {
            return await _dtos.Download<DocumentReviewDto>(_serviceUrls.ServiceUrl + string.Format(_serviceUrls.ServiceUrl_Review, docId));
        }
    }
}

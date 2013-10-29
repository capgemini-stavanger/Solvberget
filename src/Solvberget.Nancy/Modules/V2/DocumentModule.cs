﻿using System;
using System.Linq;
using Nancy;
using Solvberget.Core.DTO;
using Solvberget.Domain.Aleph;
using Solvberget.Domain.Documents;

namespace Solvberget.Nancy.Modules.V2
{
    public class DocumentModule : NancyModule
    {
        public DocumentModule(IRepository documents, IImageRepository images, IRatingRepository ratings)
            : base("/v2/documents")
        {
            Get["/{id}/thumbnail"] = args =>
            {
                string url = images.GetDocumentImage(args.id);
                return Response.AsRedirect(url);
            };

            Get["/{id}"] = args =>
            {
                Document document = documents.GetDocument(args.id, false);
                return Response.AsJson(MapToDto(document));
            };

            Get["/{id}/rating"] = args =>
            {
                DocumentRating rating = ratings.GetDocumentRating(args.id);
                return Response.AsJson(rating);
            };
        }

        private object MapToDto(Document document)
        {
            DocumentDto dto;

            if (document is Book)
            {
                var book = (Book) document;
                var bookDto = new BookDto();
                dto = bookDto;
                bookDto.Classification = book.ClassificationNr;
            }
            else
            {
                dto = new DocumentDto(); // todo other types
            }

            dto.Id = document.DocumentNumber;
            dto.Type = document.DocType;
            dto.Title = document.Title;
            dto.SubTitle = document.CompressedSubTitle;
            dto.Availability = MapToAvailabilityDto(document);

            return dto;
        }

        private DocumentAvailabilityDto MapToAvailabilityDto(Document document)
        {
            var availability = document.AvailabilityInfo.FirstOrDefault();

            if (null == availability) return null;

            return new DocumentAvailabilityDto
            {
                Branch = availability.Branch,
                AvailableCount = availability.AvailableCount,
                TotalCount = availability.TotalCount,
                
                Department = availability.Department.DefaultIfEmpty(String.Empty).Aggregate((acc, dep) =>
                {
                    if (String.IsNullOrEmpty(acc)) return dep;
                    return acc + " - " + dep;
                }),

                Collection = availability.PlacementCode,
                Location = document.LocationCode,
                EstimatedAvailableDate = availability.EarliestAvailableDateFormatted
            };
        }
    }
}

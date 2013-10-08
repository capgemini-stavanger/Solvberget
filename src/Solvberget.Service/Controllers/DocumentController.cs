﻿using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using Solvberget.Domain;
using Solvberget.Domain.Abstract;
using Solvberget.Domain.DTO;

namespace Solvberget.Service.Controllers
{
    public class DocumentController : BaseController
    {
        private readonly IRepository _documentRepository;
        private readonly ISuggestionDictionary _suggestionRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IReviewRepository _reviewRepository;

        public DocumentController(IRepository documentRepository, ISuggestionDictionary suggestionRepository, IImageRepository imageRepository, IRatingRepository ratingRepository, IReviewRepository reviewRepository)
        {
            _documentRepository = documentRepository;
            _suggestionRepository = suggestionRepository;
            _imageRepository = imageRepository;
            _ratingRepository = ratingRepository;
            _reviewRepository = reviewRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Search(string id)
        {
            var result = _documentRepository.Search(id);
            return Json(result);
        }

        public JsonResult GetDocument(string id)
        {
            var result = _documentRepository.GetDocument(id, false);
            return Json(result);
        }

        public JsonResult GetDocumentLight(string id)
        {
            var result = _documentRepository.GetDocument(id, true);
            return Json(result);
        }

        public JsonResult GetDocumentReview(string id)
        {
            var result = _reviewRepository.GetDocumentReview(id);
            return Json(result);
        }

        public JsonResult RequestReservation(string documentid, string userId, string branch)
        {
            var result = _documentRepository.RequestReservation(documentid, userId, branch);
            return Json(result);
        }

        public JsonResult RequestLoanRenewal (string documentNumber, string itemSecq, string barcode, string libraryUserId)
        {
            var result = _documentRepository.RequestRenewalOfLoan(documentNumber, itemSecq, barcode, libraryUserId);
            return Json(result);
        }

        public JsonResult CancelReservation(string documentItemNumber, string documentItemSequence, string cancellationSequence)
        {
            var result = _documentRepository.CancelReservation(documentItemNumber, documentItemSequence, cancellationSequence);
            return Json(result);
        }

        public JsonResult GetDocumentRating(string id)
        {
            var result = _ratingRepository.GetDocumentRating(id);
            return Json(result);
        }

        public JsonResult GetDocumentImage(string id)
        {
            var result = _imageRepository.GetDocumentImage(id);
            return Json(result);
        }

        public JsonResult GetDocumentThumbnailImage(string id, string size)
        {
            var result = _imageRepository.GetDocumentThumbnailImage(id, size);
            return Json(result);
        }

        public JsonResult SpellingDictionaryLookup(string value)
        {
            var result = _suggestionRepository.Lookup(value);
            return Json(result);
        }

        public JsonResult SuggestionList()
        {
            return Json(_suggestionRepository.SuggestionList(), JsonRequestBehavior.AllowGet);
        }

    }

}

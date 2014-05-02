using System;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;
using Caliburn.Micro;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;
using ZXing;
using ZXing.QrCode;

namespace Solvberget.Search8.Pages
{
    public class MetaDataItem
    {
        public string Label { get; set; }
        public string Value { get; set; }
    }

    public class ResultDetailsViewModel : Screen
    {
        private readonly INavigationService _navigation;
        private readonly ISearchService _search;

        private string _documentTitle;
        private string _documentId;
        private DocumentDto _document;
        private bool _isLoading;
        private DocumentAvailabilityDto _availability;
        private DocumentReviewDto _review;

        public ResultDetailsViewModel(INavigationService navigation, ISearchService search)
        {
            _navigation = navigation;
            _search = search;

            Facts = new BindableCollection<MetaDataItem>();
        }
        
        private async void LoadDocument()
        {
            IsLoading = true;
            
            Document = null;
            Availability = null;
            Facts.Clear();

            Document = await _search.Get(DocumentId);
            Review = await _search.GetReview(DocumentId);
            Availability = Document.Availability.FirstOrDefault(); // todo: choose the one from currently configured library
            
            PopulateFacts();

            IsLoading = false;
        }
        
        private void PopulateFacts()
        {
            Facts.Clear();

            if (Document is BookDto)
            {
                var book = Document as BookDto;

                Facts.Add(new MetaDataItem {Label = "Forfatter", Value = book.AuthorName});
                Facts.Add(new MetaDataItem {Label = "Språk", Value = book.Language});
                Facts.Add(new MetaDataItem {Label = "Forlag", Value = book.Publisher});
                Facts.Add(new MetaDataItem {Label = "Publikasjonsår", Value = book.Year.ToString()});

                if (null != book.Series)
                {
                    Facts.Add(new MetaDataItem {Label = "Serietittel", Value = book.Series.Title});
                    Facts.Add(new MetaDataItem {Label = "Serienummer", Value = book.Series.SequenceNo});
                }
            }
            else if (Document is CdDto)
            {
                var cd = Document as CdDto;

                Facts.Add(new MetaDataItem {Label = "Artist eller komponist", Value = cd.ArtistOrComposerName});
                Facts.Add(new MetaDataItem {Label = "Komposisjonstype eller sjanger", Value = cd.ArtistOrComposerName});
                Facts.Add(new MetaDataItem {Label = "Språk", Value = cd.Language});
                Facts.Add(new MetaDataItem {Label = "Label/utgiver", Value = cd.Publisher});
                Facts.Add(new MetaDataItem {Label = "Publikasjonsår", Value = cd.Year.ToString()});
            }
            else if (Document is FilmDto)
            {
                var film = Document as FilmDto;

                Facts.Add(new MetaDataItem {Label = "Publikasjonsår", Value = film.Year.ToString()});
                Facts.Add(new MetaDataItem {Label = "Type og antall plater", Value = film.MediaInfo});
                Facts.Add(new MetaDataItem {Label = "Aldersgrense", Value = film.AgeLimit});
                Facts.Add(new MetaDataItem {Label = "Omtalte steder", Value = String.Join(", ", film.ReferencedPlaces)});
                Facts.Add(new MetaDataItem {Label = "Skuespillere", Value = String.Join(", ", film.ActorNames)});
                Facts.Add(new MetaDataItem {Label = "Tekstet på", Value = String.Join(", ", film.SubtitleLanguages)});
                Facts.Add(new MetaDataItem {Label = "Sjanger", Value = String.Join(", ", film.Genres)});
                Facts.Add(new MetaDataItem {Label = "Involverte personer", Value = String.Join(", ", film.InvolvedPersonNames)});
                Facts.Add(new MetaDataItem
                {
                    Label = "Ansvarlige personer",
                    Value = String.Join(", ", film.ResponsiblePersonNames)
                });
                Facts.Add(new MetaDataItem {Label = "Språk", Value = film.Language});
                Facts.Add(new MetaDataItem {Label = "Utgiver ", Value = film.Publisher});
            }
            else if (Document is GameDto)
            {
                var game = Document as GameDto;

                Facts.Add(new MetaDataItem {Label = "Platform", Value = game.Platform});
                Facts.Add(new MetaDataItem {Label = "Språk", Value = game.Language});
                Facts.Add(new MetaDataItem {Label = "Oversatt til", Value = String.Join(", ", game.Languages)});
                Facts.Add(new MetaDataItem {Label = "Utgiver", Value = game.Publisher});
                Facts.Add(new MetaDataItem {Label = "Publikasjonsår", Value = game.Year.ToString()});
            }
            else if (Document is JournalDto)
            {
                var journal = Document as JournalDto;

                Facts.Add(new MetaDataItem {Label = "Emne", Value = String.Join(", ", journal.Subjects)});
                Facts.Add(new MetaDataItem {Label = "Språk", Value = journal.Language});
                Facts.Add(new MetaDataItem {Label = "Forlag", Value = journal.Publisher});
            }
            else if (Document is SheetMusicDto)
            {
                var sm = Document as SheetMusicDto;

                Facts.Add(new MetaDataItem {Label = "Komponist", Value = String.Join(", ", sm.ComposerName)});
                Facts.Add(new MetaDataItem {Label = "Sidetall, type note og antall stemmer", Value = sm.NumberOfPagesAndParts});
                Facts.Add(new MetaDataItem {Label = "Komposisjonstype", Value = sm.CompositionType});
                Facts.Add(new MetaDataItem {Label = "Besetning", Value = String.Join(", ", sm.MusicalLineup)});
                Facts.Add(new MetaDataItem {Label = "Undertittel", Value = sm.SubTitle});
                Facts.Add(new MetaDataItem {Label = "Forlag", Value = sm.Publisher});
                Facts.Add(new MetaDataItem {Label = "Publikasjonsår", Value = sm.Year.ToString()});
            }
        }

        public DocumentReviewDto Review
        {
            get { return _review; }
            set
            {
                if (Equals(value, _review)) return;
                _review = value;
                NotifyOfPropertyChange("Review");
            }
        }

        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                if (value.Equals(_isLoading)) return;
                _isLoading = value;
                NotifyOfPropertyChange("IsLoading");
            }
        }

        public DocumentDto Document
        {
            get { return _document; }
            set
            {
                if (Equals(value, _document)) return;
                _document = value;
                NotifyOfPropertyChange("Document");
            }
        }

        public DocumentAvailabilityDto Availability
        {
            get { return _availability; }
            set
            {
                if (Equals(value, _availability)) return;
                _availability = value;
                NotifyOfPropertyChange("Availability");
            }
        }

        public BindableCollection<MetaDataItem> Facts { get; set; } 

        public string DocumentTitle
        {
            get { return _documentTitle; }
            set
            {
                if (value == _documentTitle) return;
                _documentTitle = value;
                NotifyOfPropertyChange("DocumentTitle");
            }
        }

        public string DocumentId
        {
            get { return _documentId; }
            set
            {
                if (value == _documentId) return;
                _documentId = value;
                NotifyOfPropertyChange("DocumentId");

                LoadDocument();
            }
        }

        public void GoBack()
        {
            _navigation.GoBack();
        }
    }
}
using Caliburn.Micro;
using Solvberget.Core.DTOs;
using Solvberget.Core.Services.Interfaces;

namespace Solvberget.Search8.Pages
{
    public class ResultDetailsViewModel : Screen
    {
        private readonly INavigationService _navigation;
        private readonly ISearchService _search;

        private string _documentTitle;
        private string _documentId;
        private DocumentDto _document;
        private bool _isLoading;

        public ResultDetailsViewModel(INavigationService navigation, ISearchService search)
        {
            _navigation = navigation;
            _search = search;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            LoadDocument();
        }

        private async void LoadDocument()
        {
            IsLoading = true;
            Document = await _search.Get(DocumentId);
            IsLoading = false;
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
            }
        }

        public void GoBack()
        {
            _navigation.GoBack();
        }
    }
}
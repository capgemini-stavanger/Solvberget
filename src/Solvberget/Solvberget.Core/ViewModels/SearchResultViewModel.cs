using Solvberget.Core.ViewModels.Base;

namespace Solvberget.Core.ViewModels
{
    public class SearchResultViewModel : BaseViewModel
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private string _type;

        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                RaisePropertyChanged(() => Type);
                RaisePropertyChanged(() => PresentableType);
                RaisePropertyChanged(() => PresentableTypeWithYear);
            }
        }

        public string PresentableType => Utils.ConvertMediaTypeToNiceString(Type);
        public string PresentableTypeWithYear => $"{PresentableType} ({Year})";

        private string _year;

        public string Year
        {
            get { return _year; }
            set
            {
                _year = value;
                RaisePropertyChanged(() => Year);
                RaisePropertyChanged(() => PresentableTypeWithYear);
            }
        }

        private string _docNumber;

        public string DocNumber
        {
            get { return _docNumber; }
            set
            {
                _docNumber = value;
                RaisePropertyChanged(() => DocNumber);
            }
        }

        private string _image;

        public string Image
        {
            get { return _image; }
            set
            {
                _image = value;
                RaisePropertyChanged(() => Image);
            }
        }

        private string _mediaFormat;

        public string MediaFormat
        {
            get { return _mediaFormat; }
            set
            {
                _mediaFormat = value;
                RaisePropertyChanged(() => MediaFormat);
            }
        }
    }
}
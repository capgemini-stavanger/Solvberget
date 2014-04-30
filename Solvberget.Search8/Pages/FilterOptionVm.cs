using System;
using Caliburn.Micro;
using Solvberget.Core.DTOs;

namespace Solvberget.Search8.Pages
{
    public class FilterOptionVm : PropertyChangedBase
    {
        private string _name;
        private int _count;

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyOfPropertyChange("Name");
            }
        }

        public int Count
        {
            get { return _count; }
            set
            {
                if (value == _count) return;
                _count = value;
                NotifyOfPropertyChange("Count");
            }
        }

        public Func<DocumentDto, bool> Predicate { get; set; } 

        public bool Filter(DocumentDto document)
        {
            return null == Predicate || Predicate(document);
        }
    }
}
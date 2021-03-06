using Android.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.Droid.BindingContext;
using Solvberget.Core.DTOs;
using System;
using MvvmCross.Binding.Droid.Views;

namespace Solvberget.Droid.Views.Components
{
    public class MediaDetailInformationView : LinearLayout, IMvxBindingContextOwner
    {
        private readonly IMvxBindingContext _bindingContext;

        public object DataContext
        {
            get { return _bindingContext.DataContext; }
            set
            {
                if (value == null) return;
                _bindingContext.DataContext = value;

                View view = null;

                if (DataContext is BookDto)
                {
                    view = this.BindingInflate(Resource.Layout.mediadetail_book, null);
                }
                else if (DataContext is FilmDto)
                {
                    view = this.BindingInflate(Resource.Layout.mediadetail_film, null);
                }
                else if (DataContext is CdDto)
                {
                    view = this.BindingInflate(Resource.Layout.mediadetail_cd, null);
                }
                else if (DataContext is SheetMusicDto)
                {
                    view = this.BindingInflate(Resource.Layout.mediadetail_sheetmusic, null);
                }
                else if (DataContext is GameDto)
                {
                    view = this.BindingInflate(Resource.Layout.mediadetail_game, null);
                }
                else if (DataContext is JournalDto)
                {
                    view = this.BindingInflate(Resource.Layout.mediadetail_journal, null);
                }

                if (view != null)
                {
                    AddView(view);
                }
            }
        }

        public MediaDetailInformationView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            var inflater = (LayoutInflater)context.GetSystemService(Context.LayoutInflaterService);
            _bindingContext = new MvxAndroidBindingContext(context, (IMvxLayoutInflaterHolder)context);
        }

        public IMvxBindingContext BindingContext
        {
            get { return _bindingContext; }
            set { throw new NotImplementedException(); }
        }
    }
}
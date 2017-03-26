using CoreGraphics;
using Facebook.ShareKit;
using Foundation;
using Solvberget.Core.DTOs;
using Solvberget.Core.ViewModels;
using System;
using System.Linq;
using MvvmCross.Binding.BindingContext;
using Twitter;
using UIKit;

namespace Solvberget.iOS
{
    public partial class MediaDetailView : NamedViewController
    {
        public new MediaDetailViewModel ViewModel
        {
            get
            {
                return base.ViewModel as MediaDetailViewModel;
            }
        }


        public MediaDetailView() : base("MediaDetailView", null)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoadingOverlay.LoadingText = "Henter detaljer...";
        }

        BoxRenderer _boxes;

        protected override void ViewModelReady()
        {
            base.ViewModelReady();

            foreach (var s in ScrollView.Subviews.Skip(1)) // leave the header view
                s.RemoveFromSuperview();

            _boxes = new BoxRenderer(ScrollView);

            RatingSourceLabel.Text = HeaderLabel.Text = SubtitleLabel.Text = TypeLabel.Text = String.Empty;

            Style();
            Update();
        }

        private void Style()
        {
            StarsContainer.BackgroundColor = UIColor.Clear;
            HeaderView.BackgroundColor = Application.ThemeColors.Main2;

            HeaderLabel.Font = Application.ThemeColors.TitleFont1;
            HeaderLabel.TextColor = Application.ThemeColors.MainInverse;

            SubtitleLabel.Font = Application.ThemeColors.DefaultFont;
            SubtitleLabel.TextColor = Application.ThemeColors.MainInverse;

            TypeLabel.Font = Application.ThemeColors.DefaultFont;
            TypeLabel.TextColor = Application.ThemeColors.MainInverse;
        }

        private void OnToggleFavorite(object sender, EventArgs e)
        {
            NavigationItem.RightBarButtonItem.Enabled = false;
            ToggleFavorite();
        }

        private async void ToggleFavorite()
        {
            if (ViewModel.IsFavorite) await ViewModel.RemoveFavorite();
            else await ViewModel.AddFavorite();

            InvokeOnMainThread(() =>
            {
                UpdateFavoriteButtonState();
            });
        }

        UIBarButtonItem _favButton;
        UIBarButtonItem _shareButton;

        private void UpdateFavoriteButtonState()
        {
            var favStateImage = UIImage.FromBundle("star.on.png").Scale(new CGSize(26, 26));

            if (!ViewModel.IsFavorite && !UIHelpers.MinVersion7)
            {
                favStateImage = UIImage.FromBundle("star.off.png").Scale(new CGSize(26, 26));
            }

            if (null == _favButton)
            {
                _favButton = new UIBarButtonItem(favStateImage, UIBarButtonItemStyle.Plain, OnToggleFavorite);
                _shareButton = new UIBarButtonItem(UIBarButtonSystemItem.Action, OnShare);

                NavigationItem.SetRightBarButtonItems(new UIBarButtonItem[] { _shareButton, _favButton }, false);
            }

            if (UIHelpers.MinVersion7)
            {
                _favButton.TintColor = ViewModel.IsFavorite ? Application.ThemeColors.FavoriteColor : Application.ThemeColors.MainInverse;
            }
            else
            {
                _favButton.Image = favStateImage;
            }
        }

        private void OnShare(object sender, EventArgs e)
        {
            var shareView = new UIAlertView(View.Frame);

            shareView.Title = "Del " + ViewModel.Title;

            shareView.AddButton("Del på Facebook");
            shareView.AddButton("Del på Twitter");
            shareView.AddButton("Avbryt");

            shareView.Clicked += (ss, se) =>
            {
                string launchUri = null;

                var shareMessage = "Se hva jeg fant på Sølvberget: " + ViewModel.Title;

                if (null == ViewModel.RawDto.WebAppUrl) return;

                switch (se.ButtonIndex)
                {
                    case 0:

                        var content = new ShareLinkContent();
                        content.ContentDescription = shareMessage;
                        content.SetContentUrl(new NSUrl(ViewModel.RawDto.WebAppUrl));

                        ShareDialog.Show(this, content, null);

                        break;
                    case 1:

                        var tvc = new TWTweetComposeViewController();
                        tvc.SetInitialText(shareMessage);
                        tvc.AddUrl(new NSUrl(ViewModel.RawDto.WebAppUrl));
                        PresentModalViewController(tvc, true);

                        break;
                }

                if (null == launchUri) return;

                bool success = UIApplication.SharedApplication.OpenUrl(new NSUrl(launchUri));
            };

            shareView.CancelButtonIndex = shareView.ButtonCount - 1;

            shareView.Show();
        }

        public class MyShareDialog : ShareDialog
        {
            public MyShareDialog() : base(NSObjectFlag.Empty)
            {

            }
        }

        private void Update()
        {
            UpdateFavoriteButtonState();

            HeaderLabel.Text = ViewModel.Title;
            SubtitleLabel.Text = ViewModel.SubTitle;
            TypeLabel.Text = ViewModel.Type;

            RenderRating();
            RenderAvailability();

            switch (ViewModel.Type)
            {
                case "Book":
                    RenderBook();
                    break;

                case "Film":
                    RenderMovie();
                    break;

                case "SheetMusic":
                    RenderSheetMusic();
                    break;

                case "Game":
                    RenderGame();
                    break;

                case "Journal":
                    RenderJournal();
                    break;

                case "Cd":
                    RenderCd();
                    break;
            }

            Image.Image = UIHelpers.ImageFromUrl(ViewModel.Image);

            if (null != Image.Image)
            {
                var imageScale = Image.Frame.Width / Image.Image.Size.Width;
                var imageHeight = Math.Min(Image.Image.Size.Height * imageScale, Image.Frame.Height);
                var imageSize = new CGSize(Image.Frame.Width, imageHeight);
                Image.Frame = new CGRect(Image.Frame.Location, imageSize);
            }

            Position();

            LoadingOverlay.Hide();
        }

        void RenderAvailability()
        {
            if (null == ViewModel.Availabilities || ViewModel.Availabilities.Length == 0) return;

            _boxes.AddSectionHeader("Tilgjengelighet");

            foreach (var availability in ViewModel.Availabilities)
            {
                var box = _boxes.StartBox();
                new LabelAndValue(box, "Filial", availability.Branch, true);
                new LabelAndValue(box, "Avdeling", availability.Department);
                new LabelAndValue(box, "Samling", availability.Collection);
                new LabelAndValue(box, "Finnes på hylle", availability.Location);

                var availabilityText = availability.AvailableCount + " av " + availability.TotalCount + " tilgjengelig for utlån.";

                if (availability.EstimatedAvailableDate.HasValue)
                {
                    availabilityText += " Tidligst tilgjengelig " + availability.EstimatedAvailableText;
                }

                new LabelAndValue(box, "Tilgjengelighet", availabilityText, colspan: 3);

                var reserve = new UIButton();

                reserve.TouchUpInside += (s, e) => availability.PlaceHoldRequestCommand.Execute(null);

                var set = this.CreateBindingSet<MediaDetailView, MediaDetailViewModel>();
                set.Bind(reserve).For("Title").To(vm => vm.ButtonText);
                set.Bind(reserve).For("Enabled").To(vm => vm.ButtonEnabled);
                set.Apply();


                Application.ThemeColors.Style(reserve);

                var btnPadding = UIHelpers.MinVersion7 ? 0f : padding;

                reserve.Frame = new CGRect(padding, box.Subviews.Last().Frame.Bottom + padding, 165f, reserve.SizeThatFits(new CGSize(0f, 0f)).Height + btnPadding);

                box.Add(reserve);
                box.Frame = new CGRect(box.Frame.Location, new CGSize(box.Frame.Width, reserve.Frame.Bottom + padding));
            }
        }


        void RenderRating()
        {
            if (null != ViewModel.Rating)
            {
                RatingSourceLabel.Text = "Fra " + ViewModel.Rating.Source;

                var x = 0;
                for (int i = 0; i < (int)ViewModel.Rating.MaxScore; i++)
                {
                    var star = new UIImageView(new CGRect(x, 0, 14, 14));
                    if (i < (int)ViewModel.Rating.Score)// add star.half.on.png for better precision?
                    {
                        star.Image = UIImage.FromBundle("star.on.png");
                    }
                    else
                    {
                        star.Image = UIImage.FromBundle("star.off.png");
                    }
                    StarsContainer.Add(star);
                    x += 14;
                }
            }
        }

        void RenderBook()
        {
            if (!string.IsNullOrEmpty(ViewModel.Review))
            {
                _boxes.AddSectionHeader("Bokbasens omtale");
                var box = _boxes.StartBox();
                new LabelAndValue(box, String.Empty, ViewModel.Review, colspan: 3);
            }

            _boxes.AddSectionHeader("Fakta om boka");

            var dto = ViewModel.RawDto as BookDto;

            var facts = _boxes.StartBox();

            if (!string.IsNullOrEmpty(dto.AuthorName)) new LabelAndValue(facts, "Forfatter", dto.AuthorName);
            if (!string.IsNullOrEmpty(dto.Classification)) new LabelAndValue(facts, "Sjanger", dto.Classification);
            if (null != dto.Series) new LabelAndValue(facts, "Del av serie", dto.Series.Title);
            if (null != dto.Series) new LabelAndValue(facts, "Nummber i serie", dto.Series.SequenceNo);

            if (!string.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Forlag", ViewModel.Publisher);
            if (!string.IsNullOrEmpty(ViewModel.Language)) new LabelAndValue(facts, "Språk", ViewModel.Language);
        }

        void RenderCd()
        {
            _boxes.AddSectionHeader("Fakta om CDen");

            var dto = ViewModel.RawDto as CdDto;

            var facts = _boxes.StartBox();

            if (!string.IsNullOrEmpty(dto.ArtistOrComposerName)) new LabelAndValue(facts, "Artist eller komponist", dto.ArtistOrComposerName);
            if (null != dto.CompositionTypesOrGenres && dto.CompositionTypesOrGenres.Length > 0) new LabelAndValue(facts, "Komposisjonstype eller sjanger", string.Join(", ", dto.CompositionTypesOrGenres));
            if (!string.IsNullOrEmpty(dto.Language)) new LabelAndValue(facts, "Språk", dto.Language);
            if (!string.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Label/utgiver", ViewModel.Publisher);
            if (!string.IsNullOrEmpty(ViewModel.Year)) new LabelAndValue(facts, "Publikasjonsår", ViewModel.Year);
        }

        void RenderGame()
        {
            _boxes.AddSectionHeader("Fakta om spillet");

            var dto = ViewModel.RawDto as GameDto;

            var facts = _boxes.StartBox();
            if (!string.IsNullOrEmpty(dto.Language)) new LabelAndValue(facts, "Platform", dto.Platform);

            if (!string.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Utgiver", ViewModel.Publisher);
            if (!string.IsNullOrEmpty(ViewModel.Year)) new LabelAndValue(facts, "Publikasjonsår", ViewModel.Year);
        }

        void RenderJournal()
        {
            _boxes.AddSectionHeader("Fakta om journalen");

            var dto = ViewModel.RawDto as JournalDto;

            var facts = _boxes.StartBox();
            if (null != dto.Subjects && dto.Subjects.Length > 0) new LabelAndValue(facts, "Kategorier", string.Join(", ", dto.Subjects));

            if (!string.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Utgiver", ViewModel.Publisher);
            if (!string.IsNullOrEmpty(ViewModel.Year)) new LabelAndValue(facts, "Publikasjonsår", ViewModel.Year);
        }

        void RenderSheetMusic()
        {
            _boxes.AddSectionHeader("Fakta om notehefte");

            var dto = ViewModel.RawDto as SheetMusicDto;

            var facts = _boxes.StartBox();

            if (!string.IsNullOrEmpty(dto.ComposerName)) new LabelAndValue(facts, "Komponist", dto.ComposerName);
            if (!string.IsNullOrEmpty(dto.CompositionType)) new LabelAndValue(facts, "Komposisjonstype", dto.CompositionType);
            if (!string.IsNullOrEmpty(dto.NumberOfPagesAndParts)) new LabelAndValue(facts, "Sidetall, type noter og antall stemmer", dto.NumberOfPagesAndParts);

            if (null != dto.MusicalLineup && dto.MusicalLineup.Length > 0) new LabelAndValue(facts, "Beseting", string.Join(", ", dto.MusicalLineup));
        }


        void RenderMovie()
        {
            _boxes.AddSectionHeader("Fakta om filmen");

            var dto = ViewModel.RawDto as FilmDto;

            var facts = _boxes.StartBox();

            if (!string.IsNullOrEmpty(dto.AgeLimit)) new LabelAndValue(facts, "Aldersgrense", dto.AgeLimit.Replace("Aldersgrense:", string.Empty).Trim());
            if (!string.IsNullOrEmpty(dto.MediaInfo)) new LabelAndValue(facts, "Format", dto.MediaInfo);
            if (null != dto.ActorNames && dto.ActorNames.Length > 0) new LabelAndValue(facts, "Skuespillere", string.Join(", ", dto.ActorNames));
            if (!string.IsNullOrEmpty(dto.Language)) new LabelAndValue(facts, "Språk", dto.Language);
            if (null != dto.SubtitleLanguages && dto.SubtitleLanguages.Length > 0) new LabelAndValue(facts, "Undertekster", string.Join(", ", dto.SubtitleLanguages));
            if (null != dto.ReferredPeopleNames && dto.ReferredPeopleNames.Length > 0) new LabelAndValue(facts, "Refererte personer", string.Join(", ", dto.ReferredPeopleNames));
            if (null != dto.ReferencedPlaces && dto.ReferencedPlaces.Length > 0) new LabelAndValue(facts, "Omtalte steder", string.Join(", ", dto.ReferencedPlaces));
            if (null != dto.Genres && dto.Genres.Length > 0) new LabelAndValue(facts, "Sjanger", string.Join(", ", dto.Genres)); if (null != dto.ActorNames && dto.ActorNames.Length > 0) new LabelAndValue(facts, "Skuespillere", String.Join(",", dto.ActorNames));

            if (null != dto.InvolvedPersonNames && dto.InvolvedPersonNames.Length > 0) new LabelAndValue(facts, "Involverte personer", string.Join(", ", dto.InvolvedPersonNames));
            if (null != dto.ResponsiblePersonNames && dto.ResponsiblePersonNames.Length > 0) new LabelAndValue(facts, "Ansvarlige personer", string.Join(", ", dto.ResponsiblePersonNames));

            if (!string.IsNullOrEmpty(ViewModel.Publisher)) new LabelAndValue(facts, "Utgiver", ViewModel.Publisher);
            if (!string.IsNullOrEmpty(ViewModel.Year)) new LabelAndValue(facts, "Publikasjonsår", ViewModel.Year);
        }

        float padding = 10.0f;

        private void Position()
        {
            var headerSize = HeaderLabel.SizeThatFits(new CGSize(HeaderLabel.Frame.Width, 0));

            HeaderLabel.Frame = new CGRect(HeaderLabel.Frame.Location, headerSize);

            var subtitleSize = SubtitleLabel.SizeThatFits(new CGSize(SubtitleLabel.Frame.Width, 0));
            var subtitlePos = new CGPoint(SubtitleLabel.Frame.X, HeaderLabel.Frame.Bottom);

            SubtitleLabel.Frame = new CGRect(subtitlePos, subtitleSize);

            var typeSize = TypeLabel.SizeThatFits(new CGSize(TypeLabel.Frame.Width, 0));
            var typePos = new CGPoint(TypeLabel.Frame.X, SubtitleLabel.Frame.Bottom);

            TypeLabel.Frame = new CGRect(typePos, typeSize);

            ScrollView.ContentSize = new CGSize(320, ScrollView.Subviews.Last().Frame.Bottom + padding);
        }
    }
}


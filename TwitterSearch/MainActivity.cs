using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Linq;
using System;
using Android.Views;
using Android.Net;
using Android.Views.InputMethods;

namespace TwitterSearch
{
    [Activity(Label = "Twitter Search", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button btnClearTags;
        private Button btnSave;
        private ScrollView queryScrollView;
        private TableLayout searchResultTableLayout;
        private AutoCompleteTextView txtSearchQuery;
        private AutoCompleteTextView txtTagQuery;
        private ISharedPreferences _savedCollection;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _savedCollection = GetSharedPreferences("searches", FileCreationMode.Private);
            SetContentView(Resource.Layout.Main);
            FindControls();
            RegisterEvents();
            RefreshButtons(null);
        }

        private void FindControls()
        {
            txtSearchQuery = FindViewById<AutoCompleteTextView>(Resource.Id.txtSearchQuery);
            txtTagQuery = FindViewById<AutoCompleteTextView>(Resource.Id.txtTagQuery);
            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnClearTags = FindViewById<Button>(Resource.Id.btnClearTags);
            queryScrollView = FindViewById<ScrollView>(Resource.Id.queryScrollView);
            searchResultTableLayout = FindViewById<TableLayout>(Resource.Id.searchResultTableLayout);
        }

        private void RegisterEvents()
        {
            btnSave.Click += (sender, args) =>
            {
                string query = txtSearchQuery.Text;
                string tag = txtTagQuery.Text;

                if (query != "" && tag != "")
                {
                    MakeTag(query, tag);
                    txtSearchQuery.Text = string.Empty;
                    txtTagQuery.Text = string.Empty;

                    (GetSystemService(InputMethodService) as InputMethodManager).HideSoftInputFromWindow(txtTagQuery.WindowToken, HideSoftInputFlags.None);
                }
                else
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle(Resource.String.MissingTitle);
                    builder.SetPositiveButton(Resource.String.OK, (s, a) => { });
                    builder.SetMessage(Resource.String.MissingMessage);

                    AlertDialog errorDailog = builder.Create();
                    errorDailog.Show();
                }
            };

            btnClearTags.Click += (sender, args) =>
            {
                ClearButtons();
            };
        }

        private void RefreshButtons(string newTag)
        {
            var tags = _savedCollection.All.Keys.OrderBy(s => s).ToList();
            if (newTag == null || string.IsNullOrEmpty(newTag))
            {
                foreach (var tag in tags)
                {
                    MakeTagGUI(tag, false);
                }
            }
            else
            {
                MakeTagGUI(newTag, true);
            }
        }

        private void MakeTagGUI(string newTag, bool addNew)
        {
            LayoutInflater layoutInflater = GetSystemService(LayoutInflaterService) as LayoutInflater;
            var newTagView = layoutInflater.Inflate(Resource.Layout.SavedTags, null);

            Button newTagButton = newTagView.FindViewById<Button>(Resource.Id.btnSavedTag);
            newTagButton.SetText(newTag, null);
            newTagButton.Click += (sender, args) =>
            {
                Button thisButton = (Button)sender;
                string query = _savedCollection.GetString(thisButton.Text, null);

                string urlString = GetString(Resource.String.SearchUrl) + query;
                Intent getURL = new Intent(Intent.ActionView, Android.Net.Uri.Parse(urlString));
                StartActivity(getURL);
            };

            Button newEditButton = newTagView.FindViewById<Button>(Resource.Id.btnEdit);
            newEditButton.Click += (sender, args) =>
            {
                Button clickedButton = (Button)sender;
                TableRow parentRow = clickedButton.Parent as TableRow;
                var btnSavedTag = parentRow.FindViewById<Button>(Resource.Id.btnSavedTag);
                var queryKey = btnSavedTag.Text;
                var savedQuery = _savedCollection.GetString(queryKey, null);

                txtSearchQuery.Text = savedQuery;
                txtTagQuery.Text = queryKey;

            };

            searchResultTableLayout.AddView(newTagView);

        }

        private void MakeTag(string query, string tag)
        {
            string originalQuery = _savedCollection.GetString(tag, null);
            ISharedPreferencesEditor editor = _savedCollection.Edit();
            editor.PutString(tag, query);
            editor.Apply();

            if (originalQuery == null)
            {
                RefreshButtons(tag);
            }
        }

        private void ClearButtons()
        {
            _savedCollection.All.Clear();
            searchResultTableLayout.RemoveAllViews();
        }
    }
}


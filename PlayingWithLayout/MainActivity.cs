using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace PlayingWithLayout
{
    [Activity(Label = "Todo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private AutoCompleteTextView txtTodo;
        private Button btnAdd;
        private ListView lstTodo;
        private Button btnCancel;
        private List<string> _todoCollection;
        private ArrayAdapter<string> arryAdapter;

        protected override void OnCreate(Bundle bundle)
        {
            _todoCollection = new List<string>();

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            FindControls();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            btnCancel.Click += (sender, args) =>
            {
                txtTodo.Text = string.Empty;
            };

            btnAdd.Click += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(txtTodo.Text))
                {
                    AddToTodoListView(txtTodo.Text);
                    txtTodo.Text = string.Empty;
                }

            };
        }

        private void AddToTodoListView(string item)
        {
            _todoCollection.Add(item);
            arryAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, _todoCollection);
            lstTodo.Adapter = arryAdapter;
        }

        private void FindControls()
        {
            txtTodo = FindViewById<AutoCompleteTextView>(Resource.Id.txtTodoItem);
            btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            lstTodo = FindViewById<ListView>(Resource.Id.lstTodoList);
        }
    }
}


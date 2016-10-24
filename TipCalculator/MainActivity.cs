using System;
using Android.App;
using Android.Widget;
using Android.OS;
using TipCalculatorBusiness;

namespace TipCalculator
{
    [Activity(Label = "Tip Calculator", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        EditText txtBillAmount;
        TextView tvTipFor10p;
        TextView tvTipFor15p;
        TextView tvTipFor20p;
        TextView tvTotalFor10p;
        TextView tvTotalFor15p;
        TextView tvTotalFor20p;
        TextView tvCustomTipPercentage;
        TextView tvCustomTipAmount;
        TextView tvCustomTotalAmount;
        SeekBar sbCustomTipSlider;
        Calculator calculator = new Calculator();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            FindAllControls();
            ResetAllFields();
            RegisterEvents();
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
        }

        private void FindAllControls()
        {
            txtBillAmount = FindViewById<EditText>(Resource.Id.txtTotalBillAmount);
            tvTipFor10p = FindViewById<TextView>(Resource.Id.lblTip10p);
            tvTipFor15p = FindViewById<TextView>(Resource.Id.lblTip15p);
            tvTipFor20p = FindViewById<TextView>(Resource.Id.lblTip20p);
            tvTotalFor10p = FindViewById<TextView>(Resource.Id.lblTotal10p);
            tvTotalFor15p = FindViewById<TextView>(Resource.Id.lblTotal15p);
            tvTotalFor20p = FindViewById<TextView>(Resource.Id.lblTotal20p);
            tvCustomTipPercentage = FindViewById<TextView>(Resource.Id.lblCustomTipPercentage);
            tvCustomTipAmount = FindViewById<TextView>(Resource.Id.lblCustomTipValue);
            tvCustomTotalAmount = FindViewById<TextView>(Resource.Id.lblCustomTotalAmount);
            sbCustomTipSlider = FindViewById<SeekBar>(Resource.Id.sbCustomSet);
        }

        private void RegisterEvents()
        {
            sbCustomTipSlider.ProgressChanged += SbCustomTipSlider_ProgressChanged;
            txtBillAmount.AfterTextChanged += TxtBillAmount_AfterTextChanged;
        }

        private void SbCustomTipSlider_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            UpdateCustomTipFields(e.Progress);
        }

        private void TxtBillAmount_AfterTextChanged(object sender, Android.Text.AfterTextChangedEventArgs e)
        {
            SetFinalValues(e.Editable.ToString());
        }

        private void ResetAllFields()
        {
            string zero = "Rs. 0.0";
            tvTipFor10p.Text = zero;
            tvTipFor15p.Text = zero;
            tvTipFor20p.Text = zero;
            tvTotalFor10p.Text = zero;
            tvTotalFor15p.Text = zero;
            tvTotalFor20p.Text = zero;
            tvCustomTipPercentage.Text = zero;
            tvCustomTipAmount.Text = zero;
            tvCustomTotalAmount.Text = zero;
        }

        private void SetFinalValues(string amount)
        {
            amount = amount.Trim();
            if (string.IsNullOrEmpty(amount))
            {
                amount = "0.0";
            }

            var totalBillAmount = Convert.ToDouble(amount);
            var tipAmount10 = calculator.CalculateTipAmount(totalBillAmount, 10);
            var tipAmount15 = calculator.CalculateTipAmount(totalBillAmount, 15);
            var tipAmount20 = calculator.CalculateTipAmount(totalBillAmount, 20);
            var totalAmount10 = calculator.CalculateTotalPayable(totalBillAmount, 10);
            var totalAmount15 = calculator.CalculateTotalPayable(totalBillAmount, 15);
            var totalAmount20 = calculator.CalculateTotalPayable(totalBillAmount, 20);

            tvTipFor10p.Text = FormatAmount(tipAmount10);
            tvTipFor15p.Text = FormatAmount(tipAmount15);
            tvTipFor20p.Text = FormatAmount(tipAmount20);
            tvTotalFor10p.Text = FormatAmount(totalAmount10);
            tvTotalFor15p.Text = FormatAmount(totalAmount15);
            tvTotalFor20p.Text = FormatAmount(totalAmount20);

            int customSeekValue = sbCustomTipSlider.Progress;
            UpdateCustomTipFields(customSeekValue);
        }

        public void UpdateCustomTipFields(int seekValue)
        {
            var amount = txtBillAmount.Text.Trim();
            if (string.IsNullOrEmpty(amount))
            {
                amount = "0";
            }

            var totalBillAmount = Convert.ToDouble(amount);
            var tipAmount = calculator.CalculateTipAmount(totalBillAmount, seekValue);
            var totalAmount = calculator.CalculateTotalPayable(totalBillAmount, seekValue);

            tvCustomTipPercentage.Text = $"{seekValue}%";
            tvCustomTipAmount.Text = FormatAmount(tipAmount);
            tvCustomTotalAmount.Text = FormatAmount(totalAmount);
        }

        private string FormatAmount(double amount)
        {
            return $"Rs. {amount.ToString()}";
        }


    }
}


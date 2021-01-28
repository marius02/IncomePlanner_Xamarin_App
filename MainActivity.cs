using System;
using System.Globalization;
using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace IncomePlanner
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        EditText _incomePerHourEditText;
        EditText _hourWorkPerDayEditText;
        EditText _taxRateEditText;
        EditText _savingsRateEditText;

        TextView _worksummaryTextView;
        TextView _grossIncomeTextView;
        TextView _taxPayableTextView;
        TextView _annualSavingsTextView;
        TextView _spendableIncomeTextView;

        RelativeLayout _resultLayout;
        Button _calculateButton;
        private bool _isInputCalculated;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            ConnectViews();
        }

        public void ConnectViews()
        {
            _incomePerHourEditText = FindViewById<EditText>(Resource.Id.incomePerHourEditText);
            _hourWorkPerDayEditText = FindViewById<EditText>(Resource.Id.workHoursPerDayEditText);
            _taxRateEditText = FindViewById<EditText>(Resource.Id.taxRateEditText);
            _savingsRateEditText = FindViewById<EditText>(Resource.Id.savingsRateEditText);

            _worksummaryTextView = FindViewById<TextView>(Resource.Id.workSummaryTextView);
            _grossIncomeTextView = FindViewById<TextView>(Resource.Id.grossIncomeTextView);
            _taxPayableTextView = FindViewById<TextView>(Resource.Id.taxPayableTextView);
            _annualSavingsTextView = FindViewById<TextView>(Resource.Id.annualSavingsTextView);
            _spendableIncomeTextView = FindViewById<TextView>(Resource.Id.spendableIncomeTextView);

            _calculateButton = FindViewById<Button>(Resource.Id.calculateButton);
            _resultLayout = FindViewById<RelativeLayout>(Resource.Id.resultLayout);

            _calculateButton.Click += CalculateButtonOnClick;
        }

        private void CalculateButtonOnClick(object sender, EventArgs e)
        {
            if (_isInputCalculated)
            {
                _isInputCalculated = false;
                _calculateButton.Text = "Calculate";
                ClearInput();
                return;
            }
            var incomePerHour = double.Parse(_incomePerHourEditText.Text ?? string.Empty);
            var workHourPerDay = double.Parse(_hourWorkPerDayEditText.Text ?? string.Empty);
            var taxRate = double.Parse(_taxRateEditText.Text ?? string.Empty);
            var savingsRate = double.Parse(_savingsRateEditText.Text ?? string.Empty);

            var annualWorkHourSummary = workHourPerDay * 5 * 50;
            var annualIncome = incomePerHour * workHourPerDay * 5 * 50;
            var taxPayable = (taxRate / 100) * annualIncome;
            var annualSavings = (savingsRate / 100) * annualIncome;
            var spendableIncome = annualIncome - annualSavings - taxPayable;

            //Display results

            _grossIncomeTextView.Text = annualIncome.ToString("#,##",CultureInfo.InvariantCulture) + "USD";
            _worksummaryTextView.Text = annualWorkHourSummary.ToString("#,##", CultureInfo.InvariantCulture) + "HRS";
            _taxPayableTextView.Text = taxPayable.ToString("#,##", CultureInfo.InvariantCulture) + "USD";
            _annualSavingsTextView.Text = annualSavings.ToString("#,##", CultureInfo.InvariantCulture) + "USD";
            _spendableIncomeTextView.Text = spendableIncome.ToString("#,##", CultureInfo.InvariantCulture) + "USD";

            _resultLayout.Visibility = ViewStates.Visible;
            _isInputCalculated = true;
            _calculateButton.Text = "Clear";
        }

        public void ClearInput()
        {
            _incomePerHourEditText.Text = "";
            _hourWorkPerDayEditText.Text = "";
            _taxRateEditText.Text = "";
            _savingsRateEditText.Text = "";

            _resultLayout.Visibility = ViewStates.Invisible;
        }
    }
}
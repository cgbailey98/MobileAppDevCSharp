using C971.Services;

namespace C971.Views;

public partial class TermAdd : ContentPage
{
	public TermAdd()
	{
		InitializeComponent();
	}

    async void SaveTerm_OnClicked(object? sender, EventArgs e)
    {
        var today = DateTime.Today;
        if (string.IsNullOrWhiteSpace(TermName.Text))
        {
            await DisplayAlert("Missing Name", "Please enter a name.", "OK");
            return;
        }

        if (StartDatePicker.Date == today)
        {
            bool confirm = await DisplayAlert("Confirm Start Date", "Start Date is set to today. Is that correct?", "Yes", "No");

            if (!confirm)
            {
                return;
            }
        }

        if (EndDatePicker.Date < (StartDatePicker.Date.AddMonths(6)))
        {
            await DisplayAlert("Invalid Term Length", "The End Date must be 6 months after the Start Date", "OK");
            return;
        }

        await DatabaseService.AddTerm(TermName.Text, StartDatePicker.Date, EndDatePicker.Date);

        await Navigation.PushAsync(new TermList());
    }

    async void CancelTerm_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
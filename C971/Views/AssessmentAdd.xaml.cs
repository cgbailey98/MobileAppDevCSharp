using C971.Models;
using C971.Services;

namespace C971.Views;

public partial class AssessmentAdd : ContentPage
{
    private readonly int _selectedCourseId;
    private readonly Assessment.AssessmentType _selectedAssessmentType;
	public AssessmentAdd(int courseId, Assessment.AssessmentType assessmentType)
	{
		InitializeComponent();
        _selectedCourseId = courseId;
        _selectedAssessmentType = assessmentType;

        AssessmentTypePicker.SelectedItem = _selectedAssessmentType.ToString();
        AssessmentTypePicker.IsEnabled = false;
    }

    async void SaveAssessment_OnClicked(object? sender, EventArgs e)
    {
        var today = DateTime.Today;

        if (string.IsNullOrWhiteSpace(AssessmentName.Text))
        {
            await DisplayAlert("Missing Name", "Please enter a name.", "OK");
            return;
        }

        if (StartDatePicker.Date == today)
        {
            bool confirm = await DisplayAlert("Confirm Start Date", "Start Date is set to today. Is this correct?",
                "Yes", "No");

            if (!confirm)
            {
                return;
            }
        }

        if (EndDatePicker.Date < (StartDatePicker.Date.AddDays(1)))
        {
            await DisplayAlert("Invalid length", "The End Date must be at least one day after the Start Date.", "OK");
            return;
        }

        //if (AssessmentTypePicker.SelectedIndex == -1)
        //{
        //    await DisplayAlert("Missing Assessment Type", "Please select an Assessment Type", "OK");
        //    return;
        //}

        //if (!Enum.TryParse(AssessmentTypePicker.SelectedItem.ToString(),
        //        out Assessment.AssessmentType selectedAssessmentType))
        //{
        //    await DisplayAlert("Error", "Invalid Assessment Type selected.", "OK");
        //    return;
        //}

        await DatabaseService.AddAssessment(_selectedCourseId, AssessmentName.Text, StartDatePicker.Date,
            EndDatePicker.Date, _selectedAssessmentType, StartNotification.IsToggled, EndNotification.IsToggled);
        await Navigation.PopAsync();
    }

    async void CancelAssessment_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
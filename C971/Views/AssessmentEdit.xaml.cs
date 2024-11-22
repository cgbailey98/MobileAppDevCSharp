using C971.Services;
using C971.Models;

namespace C971.Views;

public partial class AssessmentEdit : ContentPage
{
    private readonly int _courseId;
	public AssessmentEdit(Assessment selectedAssessment)
	{
		InitializeComponent();

        _courseId = selectedAssessment.CourseId;

        AssessmentId.Text = selectedAssessment.Id.ToString();
        AssessmentName.Text = selectedAssessment.Name;
        StartDatePicker.Date = selectedAssessment.StartDate;
        EndDatePicker.Date = selectedAssessment.EndDate;
        AssessmentTypePicker.SelectedItem = selectedAssessment.Type;
    }

    async void SaveAssessment_OnClicked(object? sender, EventArgs e)
    {
        var today = DateTime.Today;

        if (string.IsNullOrWhiteSpace(AssessmentName.Text))
        {
            await DisplayAlert("Missing Name", "Please enter a name", "OK");
            return;
        }

        if (StartDatePicker.Date == today)
        {
            bool confirm = await DisplayAlert("Confirm Start Date", "Start Date is set to today. Is that correct?",
                "Yes", "No");

            if (!confirm) return;
        }

        if (EndDatePicker.Date < (StartDatePicker.Date.AddDays(1)))
        {
            await DisplayAlert("Invalid Length", "The End Date must be at least one day after the Start Date.", "OK");
            return;
        }

        if (AssessmentTypePicker.SelectedItem == null)
        {
            await DisplayAlert("Missing Type", "Please select an Assessment Type", "OK");
            return;
        }

        if (!Enum.TryParse(AssessmentTypePicker.SelectedItem.ToString(), out Assessment.AssessmentType selectedType))
        {
            await DisplayAlert("Error", "Invalid Assessment Type Selected.", "OK");
            return;
        }

        int assessmentId = Int32.Parse(AssessmentId.Text);
        

        int objectiveCount = await DatabaseService.GetObjectiveAssessmentCountExcludingAsync(_courseId, assessmentId);
        int performanceCount =
            await DatabaseService.GetPerformanceAssessmentCountExcludingAsync(_courseId, assessmentId);

        if (selectedType == Assessment.AssessmentType.Objective && objectiveCount >= 1)
        {
            await DisplayAlert("Limit Reached", "Only one Objective Assessment is allowed per course.", "OK");
            return;
        }

        if (selectedType == Assessment.AssessmentType.Performance && performanceCount >= 1)
        {
            await DisplayAlert("Limit Reached", "Only one Performance Assessment is allowed per course.", "OK");
            return;
        }

        await DatabaseService.UpdateAssessment(assessmentId, AssessmentName.Text,
            StartDatePicker.Date, EndDatePicker.Date, selectedType);

        await Navigation.PopAsync();
    }

    async void CancelAssessment_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async void DeleteAssessment_OnClicked(object? sender, EventArgs e)
    {
        var answer = await DisplayAlert("Delete this Assessment?", "Delete this Assessment?", "Yes", "No");

        if (answer == true)
        {
            var id = int.Parse(AssessmentId.Text);

            await DatabaseService.RemoveAssessment(id);

            await DisplayAlert("Assessment Deleted", "Assessment Deleted", "OK");
        }
        else
        {
            await DisplayAlert("Delete Canceled", "Nothing Deleted", "OK");
        }

        await Navigation.PopAsync();
    }
}
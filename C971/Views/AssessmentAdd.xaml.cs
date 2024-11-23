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
        if (string.IsNullOrWhiteSpace(AssessmentName.Text))
        {
            await DisplayAlert("Missing Name", "Please enter a name.", "OK");
            return;
        }

        Course associatedCourse = await DatabaseService.GetCourseById(_selectedCourseId);
        if (associatedCourse == null)
        {
            await DisplayAlert("Error", "Associated course not found.", "OK");
            return;
        }

        if (StartDatePicker.Date > EndDatePicker.Date)
        {
            await DisplayAlert("Invalid Date Range",
                "The assessment Start Date must be before the assessment End Date.", "OK");
            return;
        }

        if (StartDatePicker.Date < associatedCourse.StartDate || EndDatePicker.Date > associatedCourse.EndDate)
        {
            await DisplayAlert("Invalid Date Range",
                $"The assessment dates must be between {associatedCourse.StartDate:d} and {associatedCourse.EndDate:d}.",
                "OK");
            return;
        }

        await DatabaseService.AddAssessment(_selectedCourseId, AssessmentName.Text, StartDatePicker.Date,
            EndDatePicker.Date, _selectedAssessmentType, StartNotification.IsToggled, EndNotification.IsToggled);
        await Navigation.PopAsync();
    }

    async void CancelAssessment_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
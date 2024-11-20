using C971.Models;
using C971.Services;

namespace C971.Views;

public partial class CourseAdd : ContentPage
{
    private readonly int _selectedCourseId;
	public CourseAdd()
	{
		InitializeComponent();
	}

    public CourseAdd(int courseId)
    {
        InitializeComponent();
        _selectedCourseId = courseId;
    }

    async void SaveCourse_OnClicked(object? sender, EventArgs e)
    {
        var today = DateTime.Today;

        if (string.IsNullOrWhiteSpace(CourseName.Text))
        {
            await DisplayAlert("Missing Name", "Please enter a name.", "OK");
            return;
        }

        if (StartDatePicker.Date == today)
        {
            bool confirm = await DisplayAlert("Confirm Start Date", "Start Date is set to today. Is that correct?",
                "Yes", "No");

            if (!confirm) { return; }
        }

        if (EndDatePicker.Date < (StartDatePicker.Date.AddMonths(6)))
        {
            await DisplayAlert("Invalid term length", "The End Date must be 6 months after the Start Date.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(InstructorName.Text))
        {
            await DisplayAlert("Missing Instructor Info", "Please enter Instructor's name.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(InstructorPhone.Text))
        {
            await DisplayAlert("Missing Instructor Info", "Please enter Instructor's Phone Number.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(InstructorEmail.Text))
        {
            await DisplayAlert("Missing Instructor Info", "Please enter Instructor's Email.", "OK");
            return;
        }

        if (CourseStatusPicker.SelectedItem == null)
        {
            await DisplayAlert("Missing Course Status", "Please enter a status for the Course.", "OK");
            return;
        }

        if (!Enum.TryParse(CourseStatusPicker.SelectedItem.ToString(), out Course.StatusType selectedStatus))
        {
            await DisplayAlert("Error", "Invalid status selected.", "OK");
            return;
        }

        await DatabaseService.AddCourse(_selectedCourseId, CourseName.Text, StartDatePicker.Date, EndDatePicker.Date,
            InstructorName.Text, InstructorPhone.Text, InstructorEmail.Text,
            selectedStatus);
        await Navigation.PopAsync();
    }

    async void CancelCourse_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async void Home_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
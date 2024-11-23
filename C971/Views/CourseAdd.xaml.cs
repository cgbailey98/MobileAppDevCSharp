using System.Text.RegularExpressions;
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

        Term associatedTerm = await DatabaseService.GetTermById(_selectedCourseId);
        if (associatedTerm == null)
        {
            await DisplayAlert("Error", "Associated term not found.", "OK");
            return;
        }

        if (StartDatePicker.Date > EndDatePicker.Date)
        {
            await DisplayAlert("Invalid Date Range", "The course Start Date must be before the course End Date.", "OK");
            return;
        }

        if (StartDatePicker.Date < associatedTerm.StartDate || EndDatePicker.Date > associatedTerm.EndDate)
        {
            await DisplayAlert("Invalid Date Range",
                $"The course dates must be between {associatedTerm.StartDate:d} and {associatedTerm.EndDate:d}.", "OK");
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

        string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(InstructorEmail.Text, emailPattern))
        {
            await DisplayAlert("Invalid Email", "Please enter a valid email address", "OK");
            return;
        }

        if (CourseStatusPicker.SelectedItem == null)
        {
            await DisplayAlert("Missing Course Status", "Please enter a status for the Course.", "OK");
            return;
        }

        string selectedStatusString = CourseStatusPicker.SelectedItem.ToString();
        Course.StatusType selectedStatus;

        switch (selectedStatusString)
        {
            case "In Progress":
                selectedStatus = Course.StatusType.InProgress;
                break;
            case "None":
                selectedStatus = Course.StatusType.None;
                break;
            case "Completed":
                selectedStatus = Course.StatusType.Completed;
                break;
            case "Dropped":
                selectedStatus = Course.StatusType.Dropped;
                break;
            case "Planned":
                selectedStatus = Course.StatusType.Planned;
                break;
            default:
                await DisplayAlert("Error", "Invalid status selected.", "OK");
                return;
        }

        await DatabaseService.AddCourse(_selectedCourseId, CourseName.Text, StartDatePicker.Date, EndDatePicker.Date,
            InstructorName.Text, InstructorPhone.Text, InstructorEmail.Text,
            selectedStatus, StartNotification.IsToggled, EndNotification.IsToggled);
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
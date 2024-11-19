using C971.Models;
using C971.Services;

namespace C971.Views;

public partial class CourseEdit : ContentPage
{
	public CourseEdit(Course selectedCourse)
	{
		InitializeComponent();

        // Populate controls next
        CourseId.Text = selectedCourse.Id.ToString();
        CourseName.Text = selectedCourse.Name;
        StartDatePicker.Date = selectedCourse.StartDate;
        EndDatePicker.Date = selectedCourse.EndDate;
        InstructorName.Text = selectedCourse.InstructorName;
        InstructorPhone.Text = selectedCourse.InstructorPhone;
        InstructorEmail.Text = selectedCourse.InstructorEmail;
        CourseStatusPicker.SelectedItem = selectedCourse.Status;
    }

    async void ShareButton_OnClicked(object? sender, EventArgs e)
    {
        var text = CourseName.Text;
        await Share.RequestAsync(new ShareTextRequest
        {
            Text = text,
            Title = "Share Text"
        });
    }

    async void ShareUri_OnClicked(object? sender, EventArgs e)
    {
        string uri = "Not sure what goes here";
        await Share.RequestAsync(new ShareTextRequest
        {
            Uri = uri,
            Title = "Share Web Link"
        });
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

            if (!confirm) return;
        }

        if (EndDatePicker.Date < (StartDatePicker.Date.AddMonths(6))) //TODO find out if there is a better way to do this. Thinking of using the '<=' in some way but unsure on how to correctly do this.
        {
            await DisplayAlert("Invalid Term Length", "The End Date must be 6 months after the Start Date.", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(InstructorName.Text))
        {
            await DisplayAlert("Missing Instructor Info", "Please enter Instructor's Name", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(InstructorPhone.Text))
        {
            await DisplayAlert("Missing Instructor Info", "Please enter Instructor's Phone Number", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(InstructorEmail.Text))
        {
            await DisplayAlert("Missing Instructor Info", "Please enter Instructor's Email", "OK");
            return;
        }

        if (CourseStatusPicker.SelectedIndex == -1)
        {
            await DisplayAlert("Missing Status", "Please enter a Course Status", "OK");
            return;
        }

        await DatabaseService.UpdateCourse(Int32.Parse(CourseId.Text), CourseName.Text, StartDatePicker.Date,
            EndDatePicker.Date, InstructorName.Text, InstructorPhone.Text, InstructorEmail.Text,
            CourseStatusPicker.SelectedItem.ToString());

        await Navigation.PopAsync();
    }

    async void CancelCourse_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async void DeleteCourse_OnClicked(object? sender, EventArgs e)
    {
        var answer = await DisplayAlert("Delete this Course?", "Delete this Course?", "Yes", "No");

        if (answer == true)
        {
            var id = int.Parse(CourseId.Text);

            await DatabaseService.RemoveCourse(id);

            await DisplayAlert("Course Deleted", "Course Deleted", "OK");
        }
        else
        {
            await DisplayAlert("Delete Canceled", "Nothing Deleted", "OK");
        }

        await Navigation.PopAsync();
    }
}
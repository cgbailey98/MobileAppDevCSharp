using C971.Services;
using C971.Models;

namespace C971.Views;

public partial class TermEdit : ContentPage
{
    private readonly int _selectedTermId;

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        //TODO Return count from a table async
        // Note that the await unwraps a Task<T> to a T value (T may be a string, int, etc. In this case an int)

        int countCourses = await DatabaseService.GetCourseCountAsync(_selectedTermId);

        CountLabel.Text = countCourses.ToString();

        CourseCollectionView.ItemsSource = await DatabaseService.GetCourses(_selectedTermId);
    }
	public TermEdit()
	{
		InitializeComponent();
	}

    public TermEdit(Term term)
    {
        InitializeComponent();

        _selectedTermId = term.Id;

        TermId.Text = term.Id.ToString();
        TermName.Text = term.Name;
        StartDatePicker.Date = term.StartDate;
        EndDatePicker.Date = term.EndDate;
    }

    async void CourseCollectionView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var course = (Course)e.CurrentSelection.FirstOrDefault();
        if (e.CurrentSelection != null)
        {
            await Navigation.PushAsync((new CourseEdit(course)));
        }
    }

    async void AddCourse_OnClicked(object? sender, EventArgs e)
    {
        var termId = Int32.Parse(TermId.Text);

        int currentCourseCount = await DatabaseService.GetCourseCountAsync(termId);

        if (currentCourseCount >= 6)
        {
            await Application.Current.MainPage.DisplayAlert("Course Limit Reached",
                "Cannot add more than 6 courses to a term.", "OK");
            return;
        }

        await Navigation.PushAsync(new CourseAdd(termId));
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
            bool confirm = await DisplayAlert("Confirm Start Date", "The Start Date is set to today. Is this correct?",
                "Yes", "No");

            if (!confirm) return;
        }

        if (EndDatePicker.Date < (StartDatePicker.Date.AddMonths(6)))
        {
            await DisplayAlert("Invalid term length", "The End Date must be 6 months after Start Date",
                "OK");
            return;
        }

        await DatabaseService.UpdateTerm(int.Parse(TermId.Text), TermName.Text, StartDatePicker.Date,
            EndDatePicker.Date);

        await Navigation.PopAsync();


    }

    async void CancelTerm_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    async void DeleteTerm_OnClicked(object? sender, EventArgs e)
    {
        var answer = await DisplayAlert("Delete Term and related courses?", "Delete this Term?", "Yes", "No");

        if (answer == true)
        {
            var id = int.Parse(TermId.Text);

            await DatabaseService.RemoveTerm(id);

            await DisplayAlert("Term deleted", "Term deleted", "OK");
        }
        else
        {
            await DisplayAlert("Delete Canceled", "Nothing deleted", "OK");
        }

        await Navigation.PopAsync();
    }
}
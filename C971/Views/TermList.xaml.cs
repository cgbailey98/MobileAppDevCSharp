using C971.Models;
using C971.Services;
using Plugin.LocalNotification;

namespace C971.Views;

public partial class TermList : ContentPage
{
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (Services.Settings.FirstRun)
        {
            await DatabaseService.LoadSampleData();

            Services.Settings.FirstRun = false;

            await RefreshTermCollectionView();
        }

        await RefreshTermCollectionView();

        ShowCourseNotifications();
    }
	public TermList()
	{
		InitializeComponent();
	}

    async void AddTerm_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermAdd());
    }

    private async void ClearDatabase_OnClicked(object? sender, EventArgs e) //TODO Make sure this method isn't in final submission!!
    {
        await DatabaseService.ClearSampleData();
        await RefreshTermCollectionView();
    }

    private async void LoadSampleData_OnClicked(object? sender, EventArgs e) //TODO Make sure this method isn't in final submission!!
    {
        if (Settings.FirstRun)
        {
            await DatabaseService.LoadSampleData();
            await RefreshTermCollectionView();
        }
    }

    private async void TermCollectionView_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection != null)
        {
            Term term = (Term)e.CurrentSelection.FirstOrDefault();
            await Navigation.PushAsync(new TermEdit(term));
        }
    }

    private async Task RefreshTermCollectionView()
    {
        TermCollectionView.ItemsSource = await DatabaseService.GetTerms();
    }

    private async void ShowCourseNotifications()
    {
        //Test for enabled notifications added - 28 Sep 2024
        if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
        {
            await LocalNotificationCenter.Current.RequestNotificationPermission();
        }

        var courseList = await DatabaseService.GetCourses();
        var notifyRandom = new Random();

        foreach (Course courseRecord in courseList)
        {
            if (courseRecord.StartNotification == true && courseRecord.StartDate == DateTime.Today)
            {
                var notifyId = notifyRandom.Next(1000);

                var notification = new NotificationRequest
                {
                    NotificationId = notifyId,
                    Title = "Course Notification",
                    Subtitle = "Course Start Reminder",
                    Description = $"{courseRecord.Name} begins today!",
                    ReturningData = "Hello course notification!",
                    BadgeNumber = 42,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                    }
                };

                await LocalNotificationCenter.Current.Show(notification);
            }

            if (courseRecord.EndNotification == true && courseRecord.EndDate == DateTime.Today)
            {
                var notifyId = notifyRandom.Next(1000);

                var notification = new NotificationRequest
                {
                    NotificationId = notifyId,
                    Title = "Course Notification",
                    Subtitle = "Course End Reminder",
                    Description = $"{courseRecord.Name} ends today!",
                    ReturningData = "Hello course notification!",
                    BadgeNumber = 42,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                    }
                };

                await LocalNotificationCenter.Current.Show(notification);
            }
        }
    }
}
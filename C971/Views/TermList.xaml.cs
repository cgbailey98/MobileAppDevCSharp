namespace C971.Views;

public partial class TermList : ContentPage
{
	public TermList()
	{
		InitializeComponent();
	}

    async void AddTerm_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new TermAdd());
    }

    private void ClearDatabase_OnClicked(object? sender, EventArgs e)
    {
        //TODO Implement ClearDatabase_OnClicked
    }

    private void LoadSampleData_OnClicked(object? sender, EventArgs e)
    {
        //TODO Implement LoadSampleData_OnClicked
    }
}
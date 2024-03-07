using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SSW.Rewards.Mobile.Controls;
using SSW.Rewards.Shared.DTOs.Users;

namespace SSW.Rewards.Mobile.ViewModels;

public enum NetworkingPageSegments
{
    Friends,
    ToMeet,
    SSW,
    Other
}

public partial class NetworkingPageViewModel : BaseViewModel
{
    public NetworkingPageSegments CurrentSegment { get; set; }

    [ObservableProperty]
    private List<NetworkingProfileDto> _profiles;
    [ObservableProperty] 
    private List<Segment> _segments;
    [ObservableProperty]
    private Segment _selectedSegment;
    [ObservableProperty]
    private ObservableCollection<NetworkingProfileDto> _searchResults;

    private IDevService _devService;
    
    public NetworkingPageViewModel(IDevService devService)
    {
        _devService = devService;
    }
    
    public async Task Initialise()
    {
        if (Segments is null || Segments.Count() == 0)
        {
            Segments = new List<Segment>
            {
                new() { Name = "Friends", Value = NetworkingPageSegments.Friends },
                new() { Name = "To Meet", Value = NetworkingPageSegments.ToMeet },
                new() { Name = "SSW", Value = NetworkingPageSegments.SSW }
                // new() { Name = "This Year", Value = NetworkingPageSegments.Other },
            };
        }
        
        if(Profiles is null || Profiles.Count() == 0)
        {
            await GetProfiles();
            CurrentSegment = NetworkingPageSegments.Friends;
            SearchResults = Profiles.Where(x => x.Scanned).ToObservableCollection();
        }
    }

    private async Task GetProfiles()
    {
        var profiles = await _devService.GetProfilesAsync();
        Profiles = profiles.ToList();
    }

    [RelayCommand]
    private async Task FilterBySegment()
    {
        CurrentSegment = (NetworkingPageSegments)SelectedSegment.Value;

        if (Profiles is null || Profiles.Count() == 0)
        {
            await GetProfiles();
        }
        
        switch (CurrentSegment)
        {
            case NetworkingPageSegments.Friends:
                SearchResults = Profiles.Where(x => x.Scanned).ToObservableCollection();
                break;
            case NetworkingPageSegments.ToMeet:
                SearchResults = Profiles.Where(x => !x.Scanned).ToObservableCollection();
                break;
            case NetworkingPageSegments.SSW:
            case NetworkingPageSegments.Other:
            default:
                SearchResults = Profiles.ToObservableCollection();
                break;
        }
    }
    
    // TODO: Implement Navigation to OthersProfilePage with NetworkingProfileDto
    [RelayCommand]
    private async Task UserTapped(NetworkingProfileDto  leader)
    { 
        await Shell.Current.Navigation.PushModalAsync<OthersProfilePage>(leader);
    }
}
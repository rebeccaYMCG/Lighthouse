using Android.Locations;
using Mapsui.UI.Maui;
using Microsoft.Maui.Controls;
using Xamarin.Essentials;

namespace Lighthouse
{
    public partial class MainPage : ContentPage
    {
        MapView mapView;

        public MainPage()
        {
            InitializeComponent();
            RequestPermissions(); // Call the permission request method during initialization
            InitializationLocation(); // Call the location initialization method

            // Create a new map view and set it as the content of the page
            mapView = new MapView();
            Content = mapView;
        }

        async void RequestPermissions()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Location Denied", "Can't continue, try again.", "OK");
            }
        }

        async void InitializationLocation()
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                Latitude.Text = location.Latitude.ToString();
                Longitude.Text = location.Longitude.ToString();
                DisplayLocation(location);
            }

            Geolocation.LocationChanged += OnLocationChanged;
        }

        void DisplayLocation(Xamarin.Essentials.Location location)
        {
            mapView.Pins.Clear();
            var pin = new Pin
            {
                Label = "Current Location",
                Position = new Position(location.Latitude, location.Longitude),
                Type = PinType.Place
            };
            mapView.Pins.Add(pin);

            var mapSpan = MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromKilometers(1));
            mapView.MoveToRegion(mapSpan);
        }

        void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            var location = e.Location;
            DisplayLocation(location);
        }
    }
}
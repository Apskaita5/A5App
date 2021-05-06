using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components.Routing;

namespace A5Soft.A5App.WebApp
{
    /// <summary>
    /// A wrapper for <see cref="NavigationManager"/> that keeps navigation history.
    /// </summary>
    public class SmartNavigationManager : IDisposable
    {
        private const int MinHistorySize = 256;
        private const int AdditionalHistorySize = 64;

        private readonly NavigationManager _navigationManager;
        private readonly List<string> _history;

        public SmartNavigationManager(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager ?? throw new ArgumentNullException(nameof(navigationManager));
            _history = new List<string>(MinHistorySize + AdditionalHistorySize)
            {
                _navigationManager.Uri
            };
            _navigationManager.LocationChanged += OnLocationChanged;
        }


        /// <inheritdoc cref="NavigationManager.BaseUri"/>
        public string BaseUri 
            => _navigationManager.BaseUri;

        /// <inheritdoc cref="NavigationManager.Uri"/>
        public string Uri 
            => _navigationManager.Uri;

        /// <summary>
        /// Returns true if it is possible to navigate to the previous url.
        /// </summary>
        public bool CanNavigateBack
            => _history.Count >= 2;


        /// <inheritdoc cref="NavigationManager.NavigateTo"/>
        public void NavigateTo(string url, bool forceLoad = false)
        {
            _navigationManager.NavigateTo(url, forceLoad);
        }

        /// <inheritdoc cref="NavigationManager.ToAbsoluteUri"/>
        public Uri ToAbsoluteUri(string relativeUri)
        {
            return _navigationManager.ToAbsoluteUri(relativeUri);
        }

        /// <inheritdoc cref="NavigationManager.ToBaseRelativePath"/>
        public string ToBaseRelativePath(string uri)
        {
            return _navigationManager.ToBaseRelativePath(uri);
        }

        /// <summary>
        /// Navigates to the previous url if possible or does nothing if it is not.
        /// </summary>
        public void NavigateBack()
        {
            if (!CanNavigateBack) return;
            var backPageUrl = _history[^2];
            _history.RemoveRange(_history.Count - 2, 2);
            _navigationManager.NavigateTo(backPageUrl);
        }

        /// <summary>
        /// Navigates to the previous url if possible or to the main app page.
        /// </summary>
        public void NavigateBackOrMain()
        {
            if (CanNavigateBack) NavigateBack();
            else NavigateTo("/");
        }

        /// <summary>
        /// Navigates to the component that serves the use case type specified.
        /// </summary>
        /// <param name="useCaseType">a type of the use case to navigate to</param>
        /// <param name="entityId">an id of the entity to edit</param>
        public void NavigateToUseCase(Type useCaseType, string entityId = null)
        {
            if (null == useCaseType) throw new ArgumentNullException(nameof(useCaseType));

            _navigationManager.NavigateTo(useCaseType.RouteForUseCase(entityId));
        }


        private void OnLocationChanged(object sender, LocationChangedEventArgs e)
        {
            EnsureSize();
            _history.Add(e.Location);
        }

        private void EnsureSize()
        {
            if (_history.Count < MinHistorySize + AdditionalHistorySize) return;
            _history.RemoveRange(0, _history.Count - MinHistorySize);
        }

        public void Dispose()
        {
            _navigationManager.LocationChanged -= OnLocationChanged;
        }
    }
}

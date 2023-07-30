using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Chinook;
using Chinook.Shared;
using Chinook.Shared.Components;
using Chinook.Services;

namespace Chinook.Shared
{
    public partial class NavMenu
    {
        [Inject]
        IPlaylistService PlaylistService { get; set; }

        private bool collapseNavMenu = true;
        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;
        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}
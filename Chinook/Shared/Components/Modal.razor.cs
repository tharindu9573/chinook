using Microsoft.AspNetCore.Components;

namespace Chinook.Shared.Components
{
    public partial class Modal
    {
        [Parameter]
        public RenderFragment? Title { get; set; }

        [Parameter]
        public RenderFragment? Body { get; set; }

        [Parameter]
        public RenderFragment? Footer { get; set; }

        public Guid Guid = Guid.NewGuid();
        private string modalDisplay = "none;";
        private string modalClass = "";
        private bool showBackdrop = false;
        public void Open()
        {
            modalDisplay = "block;";
            modalClass = "show";
            showBackdrop = true;
            StateHasChanged();
        }

        public void Close()
        {
            modalDisplay = "none";
            modalClass = "";
            showBackdrop = false;
        }
    }
}
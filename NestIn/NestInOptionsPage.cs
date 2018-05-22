using Microsoft.VisualStudio.Shell;
using System.ComponentModel;

namespace NestIn
{
    public class NestInOptionsPage : DialogPage
    {
        private DefaultSelectionModeOptions _defaultSelectionMode = DefaultSelectionModeOptions.LastClicked;

        [Category("NestIn")]
        [DisplayName("Default Root Item")]
        [Description("Sets which item will be defaulted as the root item. Use 'LastClicked' when the last item you selected is the root. (default) 'FirstClicked' if you select the root class first. 'UseShortestName' will select the item with the shortest name as the root.")]
        public DefaultSelectionModeOptions DefaultSelectedItem
        {
            get { return _defaultSelectionMode; }
            set { _defaultSelectionMode = value; }
        }

        private bool _shouldPromptForDefaultRoot = true;
        [Category("NestIn")]
        [DisplayName("Prompt for Default Root?")]
        [Description("Set to True to bring up the root item selection prompt. (default)  Set to False to automatically nest without a prompt.")]
        public bool ShouldPromptForDefaultRoot
        {
            get { return _shouldPromptForDefaultRoot; }
            set { _shouldPromptForDefaultRoot = value; }
        }
    }
}

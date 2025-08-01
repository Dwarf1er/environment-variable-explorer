using EnvironmentVariableExplorer.Models;
using EnvironmentVariableExplorer.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentVariableExplorer.Components.Pages
{
    public partial class Home : LocalizedComponentBase<Home>
    {
        [Inject] private EnvironmentVariableService EnvironmentVariableService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] IDialogService DialogService { get; set; }

        private string _selectedTarget = "User";
        private string _searchString;
        private string[] _targets = ["User", "Machine", "All"];
        private EnvironmentVariable _environmentVariableBeforeEdit;
        private EnvironmentVariable _selectedEnvironmentVariable;
        private EnvironmentVariable _newEnvironmentVariable = new EnvironmentVariable();
        private List<EnvironmentVariable> _environmentVariables = new List<EnvironmentVariable>();
        private HashSet<EnvironmentVariable> _selectedEnvironmentVariables = new HashSet<EnvironmentVariable>();
        private MudTable<EnvironmentVariable> _mudTable;
        private MudTextField<string> _mudTextField;

        private bool _hover = true;
        private bool _fixedHeader = true;
        private bool _canCancelEdit = true;
        private List<string> _editEvents = new List<string>();
        private TableApplyButtonPosition _applyButtonPosition = TableApplyButtonPosition.End;
        private TableEditButtonPosition _editButtonPosition = TableEditButtonPosition.End;
        private TableEditTrigger _editTrigger = TableEditTrigger.RowClick;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            LoadEnvironmentVariables();
        }

        private void OnTargetChanged()
        {
            LoadEnvironmentVariables();
        }

        private void AddEditionEvent(string message)
        {
            _editEvents.Add(message);
            StateHasChanged();
        }

        private void BackupItem(object environmnentVariable)
        {
            _environmentVariableBeforeEdit = new()
            {
                Name = ((EnvironmentVariable)environmnentVariable).Name,
                Value = ((EnvironmentVariable)environmnentVariable).Value,
                Target = ((EnvironmentVariable)environmnentVariable).Target
            };

            AddEditionEvent($"RowEditPreview event: made a backup of EnvironmentVariable {((EnvironmentVariable)environmnentVariable).Name}");
        }

        private void ItemHasBeenCommitted(object environmentVariable)
        {
            EnvironmentVariableService.SetEnvironmentVariable(((EnvironmentVariable)environmentVariable).Name, ((EnvironmentVariable)environmentVariable).Value, ((EnvironmentVariable)environmentVariable).Target);
            AddEditionEvent($"RowEditCommit event: Changes to EnvironmentVariable {((EnvironmentVariable)environmentVariable).Name} committed");
        }

        private void ResetItemToOriginalValues(object environmentVariable)
        {
            if (environmentVariable != _newEnvironmentVariable)
            {
                ((EnvironmentVariable)environmentVariable).Name = _environmentVariableBeforeEdit.Name;
                ((EnvironmentVariable)environmentVariable).Value = _environmentVariableBeforeEdit.Value;
                ((EnvironmentVariable)environmentVariable).Target = _environmentVariableBeforeEdit.Target;
                AddEditionEvent($"RowEditCancel event: Editing of EnvironmentVariable {((EnvironmentVariable)environmentVariable).Name} canceled");
            }
            else
            {
                _environmentVariables.Remove((EnvironmentVariable)environmentVariable);
                _newEnvironmentVariable = new EnvironmentVariable();
                AddEditionEvent($"RowEditCancel event: Editing of EnvironmentVariable {((EnvironmentVariable)environmentVariable).Name} canceled");
            }
        }

        private async void AddRowAsync()
        {
            _newEnvironmentVariable = new EnvironmentVariable();
            _newEnvironmentVariable.Target = EnvironmentVariableTarget.User;
            _environmentVariables.Insert(0, _newEnvironmentVariable);

            await Task.Delay(25);

            _mudTable.SetEditingItem(_newEnvironmentVariable);
            AddEditionEvent("AddRow event: added a new row");
            await _mudTextField.FocusAsync();
        }

        private async void DeleteRowAsync(EnvironmentVariable environmentVariable)
        {
            _mudTable.SetEditingItem(null);

            bool? confirmed = await DialogService.ShowMessageBox(
                "Confirm Deletion",
                $"Are you sure you want to delete: {environmentVariable.Name}?",
                yesText: "Delete",
                noText: "Cancel"
            );

            if ((bool)confirmed)
            {
                EnvironmentVariableService.DeleteEnvironmentVariable(environmentVariable.Name, environmentVariable.Target);

                _environmentVariables.Remove(environmentVariable);
                AddEditionEvent($"DeleteRow event: deleted EnvironmentVariable {environmentVariable.Name}");
            }
        }

        private void LoadEnvironmentVariables()
        {
            switch (_selectedTarget)
            {
                case "User":
                    _environmentVariables = EnvironmentVariableService.GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.User);
                    break;

                case "Machine":
                    _environmentVariables = EnvironmentVariableService.GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.Machine);
                    break;

                case "All":
                    _environmentVariables = EnvironmentVariableService.GetAllEnvironmentVariables();
                    break;
            }
        }

        private IEnumerable<EnvironmentVariable> FilteredEnvironmentVariables =>
            string.IsNullOrWhiteSpace(_searchString)
                ? _environmentVariables
                : _environmentVariables.Where(ev =>
                    ev.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) ||
                    ev.Value.Contains(_searchString, StringComparison.OrdinalIgnoreCase)
                );

        private bool SearchEnvironmentVariables(EnvironmentVariable environmentVariable) =>
            FilteredEnvironmentVariables.Contains(environmentVariable);
    }
}

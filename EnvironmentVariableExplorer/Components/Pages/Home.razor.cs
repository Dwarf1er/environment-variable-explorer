using EnvironmentVariableExplorer.Helpers;
using EnvironmentVariableExplorer.Models;
using EnvironmentVariableExplorer.Services;
using EnvironmentVariableExplorer.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EnvironmentVariableExplorer.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject] private EnvironmentVariableService EnvironmentVariableService { get; set; }
        [Inject] private LanguageService LanguageService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] IDialogService DialogService { get; set; }

        private string _searchString;
        private List<EnvironmentVariableTargetSelectionOption> _availableEnvironmentVariableTargets;
        private EnvironmentVariableTargetSelectionOption _selectedEnvironmentVariableTarget;
        private EnvironmentVariable _environmentVariableBeforeEdit;
        private EnvironmentVariable _selectedEnvironmentVariable;
        private EnvironmentVariable _newEnvironmentVariable = new EnvironmentVariable();
        private List<EnvironmentVariable> _environmentVariables = new List<EnvironmentVariable>();
        private MudTable<EnvironmentVariable> _mudTable;
        private MudTextField<string> _mudTextField;

        private bool _hover = true;
        private bool _fixedHeader = true;
        private bool _canCancelEdit = true;
        private List<string> _editEvents = new List<string>();
        private TableApplyButtonPosition _applyButtonPosition = TableApplyButtonPosition.End;
        private TableEditButtonPosition _editButtonPosition = TableEditButtonPosition.End;
        private TableEditTrigger _editTrigger = TableEditTrigger.RowClick;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            LanguageService.OnLanguageChanged += LanguageChanged;

            _availableEnvironmentVariableTargets = new List<EnvironmentVariableTargetSelectionOption> { EnvironmentVariableTargetSelectionOption.Process };

            if(SystemUtils.IsWindows)
            {
                _availableEnvironmentVariableTargets.Add(EnvironmentVariableTargetSelectionOption.User);
                _availableEnvironmentVariableTargets.Add(EnvironmentVariableTargetSelectionOption.Machine);
                _availableEnvironmentVariableTargets.Add(EnvironmentVariableTargetSelectionOption.All);
            }

            _selectedEnvironmentVariableTarget = EnvironmentVariableTargetSelectionOption.Process;
            await LoadEnvironmentVariablesAsync();
        }

        private async Task OnEnvironmentVariableTargetChanged()
        {
            await LoadEnvironmentVariablesAsync();
        }

        private void AddEditionEvent(string message)
        {
            _editEvents.Add(message);
            StateHasChanged();
        }

        private void LanguageChanged()
        {
            InvokeAsync(StateHasChanged);
        }

        private async Task LoadEnvironmentVariablesAsync()
        {
            Result<List<EnvironmentVariable>, string> result = _selectedEnvironmentVariableTarget switch
            {
                EnvironmentVariableTargetSelectionOption.User => await Task.FromResult(EnvironmentVariableService.GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.User)),
                EnvironmentVariableTargetSelectionOption.Machine => await Task.FromResult(EnvironmentVariableService.GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.Machine)),
                EnvironmentVariableTargetSelectionOption.Process => await Task.FromResult(EnvironmentVariableService.GetEnvironmentVariablesByTarget(EnvironmentVariableTarget.Process)),
                EnvironmentVariableTargetSelectionOption.All => await Task.FromResult(EnvironmentVariableService.GetAllEnvironmentVariables()),
                _ => Result<List<EnvironmentVariable>, string>.Err(Resources.Strings.InvalidEnvironmentVariableTargetError)
            };

            if (result.IsSuccess)
            {
                _environmentVariables = result.Value;
            }

            else
            {
                Snackbar.Add($"{Resources.Strings.LoadEnvironmentVariablesError}: {result.Error}", Severity.Error);
                _environmentVariables = new List<EnvironmentVariable>();
            }

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

        private async void ItemHasBeenCommitted(object environmentVariableComitted)
        {
            EnvironmentVariable environmentVariable = (EnvironmentVariable)environmentVariableComitted;
            Result<Unit, string> result = await Task.FromResult(EnvironmentVariableService.SetEnvironmentVariable(environmentVariable.Name, environmentVariable.Value, environmentVariable.Target));

            if (result.IsSuccess)
            {
                AddEditionEvent($"RowEditCommit event: Changes to EnvironmentVariable {environmentVariable.Name} committed");
                Snackbar.Add(Resources.Strings.SnackbarChangeSaved, Severity.Success);
            }
            else
            {
                ResetItemToOriginalValues(environmentVariable);
                Snackbar.Add($"{Resources.Strings.SnackbarFailedToSave}: {result.Error}", Severity.Error);
            }
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

            EnvironmentVariableTarget actualTarget = _selectedEnvironmentVariableTarget switch
            {
                EnvironmentVariableTargetSelectionOption.User => EnvironmentVariableTarget.User,
                EnvironmentVariableTargetSelectionOption.Machine => EnvironmentVariableTarget.Machine,
                EnvironmentVariableTargetSelectionOption.Process => EnvironmentVariableTarget.Process,
                _ => EnvironmentVariableTarget.Process
            };

            if (!SystemUtils.IsWindows &&
                (actualTarget == EnvironmentVariableTarget.User || actualTarget == EnvironmentVariableTarget.Machine))
            {
                Snackbar.Add(Resources.Strings.SnackbarAddRowPlatformWarning, Severity.Warning);
                return;
            }

            _newEnvironmentVariable.Target = actualTarget;
            _environmentVariables.Insert(0, _newEnvironmentVariable);

            await Task.Delay(25);
            _mudTable.SetEditingItem(_newEnvironmentVariable);
            AddEditionEvent("AddRow event: added a new row");
            await _mudTextField.FocusAsync();
        }

        private async Task DeleteRowAsync(EnvironmentVariable environmentVariable)
        {
            _mudTable.SetEditingItem(null);

            bool? confirmed = await DialogService.ShowMessageBox(
                Resources.Strings.DialogConfirmDeletionTitle,
                $"{Resources.Strings.DialogConfirmDeletionMessage}: {environmentVariable.Name}?",
                yesText: Resources.Strings.Delete,
                noText: Resources.Strings.Cancel
            );

            if (confirmed == true)
            {
                Result<Unit, string> result = await Task.FromResult(EnvironmentVariableService.DeleteEnvironmentVariable(environmentVariable.Name, environmentVariable.Target));

                if (result.IsSuccess)
                {
                    _environmentVariables.Remove(environmentVariable);
                    AddEditionEvent($"DeleteRow event: deleted EnvironmentVariable {environmentVariable.Name}");
                    Snackbar.Add(Resources.Strings.SnackbarDeleteSuccess, Severity.Success);
                }
                else
                {
                    Snackbar.Add($"{Resources.Strings.SnackbarDeleteFailed}: {result.Error}", Severity.Error);
                }
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

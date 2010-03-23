using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using DependencyViewer.Common.Loaders;

namespace DependencyViewer.Common.Model
{
    [DebuggerDisplay("{Name}")]
    public class Project : INotifyPropertyChanged
    {
        private readonly HashSet<Guid> _projectRefs = new HashSet<Guid>();
        private readonly List<AssemblyName> _referencedDLLs = new List<AssemblyName>();

        public Project(ProjectLoader loader)
        {
            Name = loader.Name;
            ProjectIdentifier = loader.ProjectIdentifier;
            
            foreach(var reference in loader.ProjectReferences)
                _projectRefs.Add(reference);

            foreach(var reference in loader.ReferencedDLLs)
                _referencedDLLs.Add(reference);

            IsSelected = true;
        }

        // Immutable Properties
        public string Name { get; private set; }
        public Guid ProjectIdentifier { get; private set; }
        public IEnumerable<Guid> ProjectReferences { get { return _projectRefs; } }
        public IEnumerable<AssemblyName> ReferencedDLLs { get { return _referencedDLLs; } }

        // Mutable Properties
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; RaisePropertyChangedEvent("IsSelected"); }
        }

        private void RaisePropertyChangedEvent(string propertyName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public bool HasReferencedProject(Guid projectGuid)
        {
            return _projectRefs.Contains(projectGuid);
        }

        public void RemoveReferencedProject(Guid projectGuid)
        {
            _projectRefs.Remove(projectGuid);
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
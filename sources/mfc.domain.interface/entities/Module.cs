using System;
using System.Collections.Generic;

namespace mfc.domain.entities
{
    public class Module
    {
        public string Name { get; set; }
        public string Code { get; set; }

        private IEnumerable<Module> _modules = new List<Module>();
        public IEnumerable<Module> Modules
        {
            get
            {
                return _modules;
            }
            set
            {
                if (value != null)
                {
                    _modules = value;
                }
                else
                {
                    _modules = new List<Module>();
                }
            }
        }

        private IEnumerable<Operation> _operations = new List<Operation>();
        public IEnumerable<Operation> Operations
        {
            get
            {
                return _operations;
            }
            set
            {
                if (value != null)
                {
                    _operations = value;
                }
                else
                {
                    _operations = new List<Operation>();
                }
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Module;
            if (other == null) return false;

            return Equals(other.Code, Code);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        //todo: написать тест
        public Module Clone()
        {
            var modules = new List<Module>();
            foreach (var module in Modules)
            {
                modules.Add(module);
            }

            var operations = new List<Operation>();
            foreach (var operation in Operations)
            {
                operations.Add(operation);
            }

            return new Module { Code = Code, Name = Name, Modules = modules, Operations = operations };
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.domain.entities {
    /// <summary>
    /// Элемент справочника "Услуги"
    /// </summary>
    public class Service : Entity {
        public virtual Service Parent { get; set; }
        public virtual string Caption { get; set; }
        public virtual Organization Organization { get; set; }

        public static Service All = new Service { Id = -1, Caption = "Все" };

        public override bool Equals(object obj) {
            if (obj == null) {
                return false;
            }
            if (obj.GetType() == typeof(Service)) {
                return ((Service)obj).Id.Equals(Id);
            }

            return ReferenceEquals(obj, this);
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}

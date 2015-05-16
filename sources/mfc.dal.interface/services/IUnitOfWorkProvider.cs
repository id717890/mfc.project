using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mfc.dal.services {
    /// <summary>
    /// Интерфейс предназначен для доступа к реализации интерфейса UnitOfWork.
    /// Такое разделение необходимо, для того чтобы дать возможность получать актуальную версию
    /// экземпляра IUnitOfWork для объектов с различным временем жизни. Например, для типов,
    /// которые существуют в течение запроса или течение жизни всего приложения.
    /// </summary>
    public interface IUnitOfWorkProvider {
        IUnitOfWork GetUnitOfWork();
    }
}

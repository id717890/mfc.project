using System;
using mfc.webapi.Helpers;

namespace mfc.webapi.tests.Modules
{
    public class MockModulesConfiguration : IModulesConfiguration
    {
        public MockModulesConfiguration(string config = null)
        {
            if (config != null)
            {
                _config = config;
            }
        }
        private readonly string _config = @" [
    {
      'code': 'admin',
      'name': 'Администрирование',
      'modules': [
        {
          'code': 'customer-type',
          'name': 'Виды посетителей',
          'operations': [
            {
              'code': 'add',
              'name': 'Добавить'
            },
            {
              'code': 'delete',
              'name': 'Удалить'
            },
            {
              'code': 'update',
              'name': 'Изменить'
            }
          ]
        }
      ]

    },
    {
      'code': 'actions',
      'name': 'Приемы'
    },
    {
      'code': 'files',
      'name': 'Дела'
    }
  ]
";

        public string Configuration
        {
            get
            {
                return _config;
            }
        }
    }
}

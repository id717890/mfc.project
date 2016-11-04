using AutoMapper;
using mfc.domain.entities;

namespace mfc.webapi.tests.Automapper.Tests
{
    public abstract class AutomapperBaseTests<TEntity, TModel>
    {
        protected IMapper _mapper;

        public AutomapperBaseTests()
        {
            AutoMapperConfig.Configure();
            _mapper = AutoMapperConfig.Mapper();
        }

        protected TModel GetModel(TEntity entity)
        {
            return _mapper.Map<TModel>(entity);
        }
        protected TEntity GetEntity(TModel model)
        {
            return _mapper.Map<TEntity>(model);
        }

        protected void AssertModel(TModel model)
        {
            AssertModelToEntity(model, GetEntity(model));
        }

        protected void AssertEntity(TEntity entity)
        {
            AssertEntityToModel(entity, GetModel(entity));
        }

        protected abstract void AssertModelToEntity(TModel model, TEntity entity);
        protected abstract void AssertEntityToModel(TEntity entity, TModel model);
    }
}

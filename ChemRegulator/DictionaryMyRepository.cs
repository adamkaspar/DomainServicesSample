namespace ChemRegulator
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.Security.Claims;
    using DHI.Services;

    public class DictionaryMyRepository : IMyEntityRepository
    {
        protected readonly ConcurrentDictionary<Guid, MyEntity> entities;

        public DictionaryMyRepository()
        {
            entities = new ConcurrentDictionary<Guid, MyEntity>();
        }

        public void Add(MyEntity entity, ClaimsPrincipal user = null)
        {
            entities.TryAdd(entity.Id, entity);
        }

        public bool Contains(Guid id, ClaimsPrincipal user = null)
        {
            return entities.ContainsKey(id);
        }

        public int Count(ClaimsPrincipal user = null)
        {
            return entities.Count;
        }

        public IEnumerable<MyEntity> Get(Query<MyEntity> query)
        {
            var expression = query.ToExpression();

            var result = entities
                            .Values
                            .AsQueryable()
                            .Where(expression)
                            .ToList();

            return result;
        }

        public Maybe<MyEntity> Get(Guid id, ClaimsPrincipal user = null)
        {
            if (entities.TryGetValue(id, out var value))
            {
                return value.ToMaybe();
            }

            return Maybe.Empty<MyEntity>();
        }

        public IEnumerable<MyEntity> GetAll(ClaimsPrincipal user = null)
        {
            return entities.Values;
        }

        public IEnumerable<Guid> GetIds(ClaimsPrincipal user = null)
        {
            return entities.Keys;
        }

        public void Remove(Guid id, ClaimsPrincipal user = null)
        {
            entities.TryRemove(id, out _);
        }

        public void Update(MyEntity entity, ClaimsPrincipal user = null)
        {
            if (Contains(entity.Id))
            {
                entities[entity.Id] = entity;
            }
        }
    }
}
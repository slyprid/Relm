﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Relm.Entities
{
    public class EntityCollection
    {
        public Dictionary<string, OldEntity> Entities { get; }

        public List<OldEntity> Values => Entities.Values.ToList();

        public EntityCollection()
        {
            Entities = new Dictionary<string, OldEntity>();
        }

        public OldEntity Add<T>(params object[] args)
            where T : OldEntity
        {
            var entity = (T)Activator.CreateInstance(typeof(T), args);
            entity.OnCreate();
            Entities.Add(entity.Name, entity);
            return entity;
        }

        public OldEntity Add<T>(string name, params object[] args)
            where T : OldEntity
        {
            var entity = (T)Activator.CreateInstance(typeof(T), args);
            entity.Name = name;
            entity.OnCreate();
            Entities.Add(entity.Name, entity);
            return entity;
        }

        public void Remove(string name)
        {
            if (!Entities.ContainsKey(name)) return;
            var entity = Entities[name];
            Entities.Remove(name);
            entity.OnDestroy();
        }

        public void Clear()
        {
            foreach (var entity in Entities.Values)
            {
                entity.OnDestroy();
            }

            Entities.Clear();
        }

        public void ForEach(Action<OldEntity> action)
        {
            Entities.Values.ToList().ForEach(action);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using eth.Eve.Internal;
using eth.Eve.Storage;
using System.Collections.ObjectModel;
using eth.Common;
using Microsoft.EntityFrameworkCore;

namespace eth.Eve
{
    public sealed class EveBot : IDisposable
    {
        private readonly Action<DbContextOptionsBuilder<EveDb>> _dbContextOptionsBuilder;

        private volatile bool _shutdown;
        private volatile bool _started;

        private List<EveSpaceInitializer> _spaceInitializers;
        private List<EveBotSpace> _spaces;

        public IHttpClientProxy Proxy { get; set; }
                
        public EveBot(Action<DbContextOptionsBuilder<EveDb>> optionsBuilder)
        {
            _dbContextOptionsBuilder = optionsBuilder;
            var db = GetDbContext();

            var spaces = db.EveSpaces.Where(s => s.IsActive).ToList();

            if (spaces.Count == 0)
                throw new InvalidOperationException("there are no spaces, check your db");

            _spaceInitializers = spaces.Select(s => new EveSpaceInitializer(s)).ToList();
        }

        public IReadOnlyDictionary<long, IEveSpaceInitializer> GetSpaceInitializers()
        {
            if (_started || _shutdown)
                throw new InvalidOperationException("it's too late to initialize: either it's already running or already stopped");
            
            return new ReadOnlyDictionary<long, IEveSpaceInitializer>(_spaceInitializers.ToDictionary(i => i.SpaceId, i => (IEveSpaceInitializer)i));
        }

        public void Start()
        {
            if (_shutdown)
                throw new InvalidOperationException("_shutdown == true");
            if (_started)
                throw new InvalidOperationException("_started == true");

            try
            {
                _spaces = _spaceInitializers
                    .Where(i => i.IsEnabled)
                    .Select(i => new EveBotSpace(i, GetDbContext, Proxy))
                    .ToList();

                _spaceInitializers = null;

                foreach (var space in _spaces)
                    space.Start();

                _started = true;
            }
            catch
            {
                Dispose();

                throw;
            }
        }

        public void Stop()
        {
            Dispose();
        }

        public void Dispose()
        {
            _shutdown = true;

            if (_spaces != null)
            {
                foreach (var space in _spaces)
                    space.Dispose();

                _spaces = null;
            }
        }

        private EveDb GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<EveDb>();
            _dbContextOptionsBuilder(optionsBuilder);

            return new EveDb(optionsBuilder.Options);
        }
    }
}

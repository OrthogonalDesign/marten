﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Baseline;
using Marten.Services;
using Marten.Testing.Documents;
using Marten.Testing.Harness;
using Shouldly;
using Xunit;

namespace Marten.Testing.Services
{
    public class NulloIdentityMap_version_tracking : IdentityMap_Version_Tracking<NulloIdentityMap>
    {
        public NulloIdentityMap_version_tracking(DefaultStoreFixture fixture) : base(fixture)
        {
        }
    }
    public class IdentityMap_version_tracking : IdentityMap_Version_Tracking<IdentityMap>
    {
        public IdentityMap_version_tracking(DefaultStoreFixture fixture) : base(fixture)
        {
        }
    }
    public class DirtyTrackingIdentityMap_version_tracking : IdentityMap_Version_Tracking<DirtyTrackingIdentityMap>
    {
        public DirtyTrackingIdentityMap_version_tracking(DefaultStoreFixture fixture) : base(fixture)
        {
        }
    }


    public abstract class IdentityMap_Version_Tracking<T> : IntegrationContextWithIdentityMap<T> where T : IIdentityMap
    {
        private IIdentityMap theIdentityMap;

        public IdentityMap_Version_Tracking(DefaultStoreFixture fixture) : base(fixture)
        {
            throw new NotImplementedException();
            //theIdentityMap = theSession.As<DocumentSession>().IdentityMap;
        }

        [Fact]
        public void store_by_version()
        {
            var target = Target.Random();
            var version = Guid.NewGuid();

            theIdentityMap.Store(target.Id, target, version);

            theIdentityMap.Versions.Version<Target>(target.Id)
                .ShouldBe(version);
        }

        [Fact]
        public void get_by_id_and_json()
        {
            var target = Target.Random();
            var json = theStore.Advanced.Serializer.ToJson(target);

            var version = Guid.NewGuid();

            theIdentityMap.Get<Target>(target.Id, json.ToReader(), version);

            theIdentityMap.Versions.Version<Target>(target.Id)
                .ShouldBe(version);
        }

        [Fact]
        public void get_by_id_and_json_and_type()
        {
            var target = Target.Random();
            var json = theStore.Advanced.Serializer.ToJson(target);

            var version = Guid.NewGuid();

            theIdentityMap.Get<Target>(target.Id, typeof(Target), json.ToReader(), version);

            theIdentityMap.Versions.Version<Target>(target.Id)
                .ShouldBe(version);
        }

    }
}

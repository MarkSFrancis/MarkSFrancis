﻿namespace Phnx.AspNetCore.Rest.Tests.Fakes
{
    public class FakePatch
    {
        public FakePatch()
        {
        }

        public FakePatch(FakeResource resource)
        {
            Id = resource.Id;
        }

        public int Id { get; set; }
    }
}

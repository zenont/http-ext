using System;

namespace HttpClientExtensions.Abstractions.Test
{
    public class FakePayload
    {
        public int Id { get; set; }

        public Guid Token { get; set; }

        public DateTime NonNullDateTime { get; set; }

        public DateTime? NullableDateTime { get; set; }

        public DateTimeOffset DateTimeOffset { get; set; }

        public string Note { get; set; }

        public string[] SomeArray { get; set; }
    }
}

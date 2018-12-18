using System;
using AutoFixture;
using Core.Builders;
using Core.Interfaces;
using Core.Models;
using Xunit;

namespace Core.Tests
{
    public class ConnectionStringUtilityTest
    {
        private readonly IConnectionStringUtility<TestModel> _utility;
        private readonly Fixture _fixture;

        public ConnectionStringUtilityTest()
        {
            _utility = ConnectionStringUtility<TestModel>.New()
                .AddTypeConverter<Guid>(PropertyConverterBuilderInstance.New<Guid>()
                    .SetParse(x => Guid.Parse(x))
                    .SetToString(x => x.ToString())
                    .Build());

            _fixture = new Fixture();
        }

        [Fact]
        public void Test__Cycle()
        {
            // Arrange
            var instance = _fixture.Create<TestModel>();

            // Act
            var result = _utility.Parse(_utility.InstanceToString((ObjectWrapper<TestModel>) instance)).Value;

            // Assert
            Assert.Equal(instance, result, TestModelEqualityComparer.TestModelComparer);
        }
        
        [Fact]
        public void Test__Set_Native()
        {
            // Arrange
            var value = _fixture.Create<string>();
            var instance = _fixture.Create<TestModel>();

            // Act
            var result = ((ObjectWrapper<TestModel>) instance)
                .Set(x => x.Prop1, value);

            // Assert
            Assert.Contains(result.Flatten(), pair => pair.Key == "Prop1" && pair.Value == value);
        }

        [Fact]
        public void Test__Set_Custom()
        {
            // Arrange
            var instance = _fixture.Create<TestModel>();

            // Act
            var result = ((ObjectWrapper<TestModel>) instance)
                .Set("Key", "Value");

            // Assert
            Assert.Contains(result.CustomProperties, pair => pair.Key == "Key" && pair.Value == "Value");
        }
        
        [Fact]
        public void Test__Delete()
        {
            // Arrange
            var instance = _fixture.Create<TestModel>();

            // Act
            var result = ((ObjectWrapper<TestModel>) instance)
                .Delete(x => x.Prop1)
                .Flatten();

            // Assert
            Assert.DoesNotContain(result, pair => pair.Key == "Prop1");
        }
    }
}
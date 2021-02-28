using Amazon.Lambda.Core;
using AutoFixture;
using Moq;
using Newtonsoft.Json;
using Email.Int.Application;
using Email.Int.Domain.Entities;
using System.Collections.Generic;
using Xunit;
using Amazon.Lambda.SQSEvents;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Tests
{
    public class Tests
    {
        private Fixture _fixture;
        private Function _function;

        public Tests()
        {
            _fixture = new Fixture();
            _function = new Function();
        }

        [Fact]
        public async Task SendEmailTests()
        {
            var template = await GetTemplateAsync("Email.Int.Test.TemplateSample.Orders.html").ConfigureAwait(false);
            var lambdaContext = new Mock<ILambdaContext>();
            var message = _fixture
                .Build<Message>()
                .With(x => x.Body, template)
                .With(x => x.Subject, "Email teste")
                .With(x => x.To, "andreparis.comp@gmail.com")
                .Create();
            var apiContext = _fixture
                .Build<SQSEvent>()
                .With(x => x.Records, 
                new List<SQSEvent.SQSMessage>() 
                { 
                    new SQSEvent.SQSMessage() 
                    { 
                        Body = JsonConvert.SerializeObject(new { Message = message}) 
                    } 
                })
                .Create();

            _function.SendEmail(apiContext, lambdaContext.Object);

        }

        private async Task<string> GetTemplateAsync(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(resourceName);
            var reader = new StreamReader(stream);
            string template = await reader.ReadToEndAsync().ConfigureAwait(false);
            return template;
        }
    }
}
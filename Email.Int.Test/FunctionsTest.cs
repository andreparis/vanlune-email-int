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
                        Body = "{\"Message\":{\"From\":\"accounts@player2.store\",\"To\":\"andre.paris@yahoo.com.br\",\"Bcs\":null,\"Body\":\"<html xmlns=\\\"http://www.w3.org/1999/xhtml\\\">\\r\\n<head>\\r\\n    <meta name=\\\"viewport\\\" content=\\\"width=device-width, initial-scale=1.0\\\" />\\r\\n    <meta http-equiv=\\\"Content-Type\\\" content=\\\"text/html; charset=UTF-8\\\" />\\r\\n    <title>Verify your email address</title>\\r\\n    <style type=\\\"text/css\\\" rel=\\\"stylesheet\\\" media=\\\"all\\\">\\r\\n        /* Base ------------------------------ */\\r\\n        *:not(br):not(tr):not(html) {\\r\\n            font-family: Arial, 'Helvetica Neue', Helvetica, sans-serif;\\r\\n            -webkit-box-sizing: border-box;\\r\\n            box-sizing: border-box;\\r\\n        }\\r\\n\\r\\n        body {\\r\\n            width: 100% !important;\\r\\n            height: 100%;\\r\\n            margin: 0;\\r\\n            line-height: 1.4;\\r\\n            background-color: #F5F7F9;\\r\\n            color: #839197;\\r\\n            -webkit-text-size-adjust: none;\\r\\n        }\\r\\n\\r\\n        a {\\r\\n            color: #414EF9;\\r\\n        }\\r\\n\\r\\n        /* Layout ------------------------------ */\\r\\n        .email-wrapper {\\r\\n            width: 100%;\\r\\n            margin: 0;\\r\\n            padding: 0;\\r\\n            background-color: #F5F7F9;\\r\\n        }\\r\\n\\r\\n        .email-content {\\r\\n            width: 100%;\\r\\n            margin: 0;\\r\\n            padding: 0;\\r\\n        }\\r\\n\\r\\n        /* Masthead ----------------------- */\\r\\n        .email-masthead {\\r\\n            padding: 25px 0;\\r\\n            text-align: center;\\r\\n        }\\r\\n\\r\\n        .email-masthead_logo {\\r\\n            max-width: 400px;\\r\\n            border: 0;\\r\\n        }\\r\\n\\r\\n        .email-masthead_name {\\r\\n            font-size: 16px;\\r\\n            font-weight: bold;\\r\\n            color: #839197;\\r\\n            text-decoration: none;\\r\\n            text-shadow: 0 1px 0 white;\\r\\n        }\\r\\n\\r\\n        /* Body ------------------------------ */\\r\\n        .email-body {\\r\\n            width: 100%;\\r\\n            margin: 0;\\r\\n            padding: 0;\\r\\n            border-top: 1px solid #E7EAEC;\\r\\n            border-bottom: 1px solid #E7EAEC;\\r\\n            background-color: #FFFFFF;\\r\\n        }\\r\\n\\r\\n        .email-body_inner {\\r\\n            width: 570px;\\r\\n            margin: 0 auto;\\r\\n            padding: 0;\\r\\n        }\\r\\n\\r\\n        .email-footer {\\r\\n            width: 570px;\\r\\n            margin: 0 auto;\\r\\n            padding: 0;\\r\\n            text-align: center;\\r\\n        }\\r\\n\\r\\n            .email-footer p {\\r\\n                color: #839197;\\r\\n            }\\r\\n\\r\\n        .body-action {\\r\\n            width: 100%;\\r\\n            margin: 30px auto;\\r\\n            padding: 0;\\r\\n            text-align: center;\\r\\n        }\\r\\n\\r\\n        .body-sub {\\r\\n            margin-top: 25px;\\r\\n            padding-top: 25px;\\r\\n            border-top: 1px solid #E7EAEC;\\r\\n        }\\r\\n\\r\\n        .content-cell {\\r\\n            padding: 35px;\\r\\n        }\\r\\n\\r\\n        .align-right {\\r\\n            text-align: right;\\r\\n        }\\r\\n\\r\\n        /* Type ------------------------------ */\\r\\n        h1 {\\r\\n            margin-top: 0;\\r\\n            color: #292E31;\\r\\n            font-size: 19px;\\r\\n            font-weight: bold;\\r\\n            text-align: left;\\r\\n        }\\r\\n\\r\\n        h2 {\\r\\n            margin-top: 0;\\r\\n            color: #292E31;\\r\\n            font-size: 16px;\\r\\n            font-weight: bold;\\r\\n            text-align: left;\\r\\n        }\\r\\n\\r\\n        h3 {\\r\\n            margin-top: 0;\\r\\n            color: #292E31;\\r\\n            font-size: 14px;\\r\\n            font-weight: bold;\\r\\n            text-align: left;\\r\\n        }\\r\\n\\r\\n        p {\\r\\n            margin-top: 0;\\r\\n            color: #839197;\\r\\n            font-size: 16px;\\r\\n            line-height: 1.5em;\\r\\n            text-align: left;\\r\\n        }\\r\\n\\r\\n            p.sub {\\r\\n                font-size: 12px;\\r\\n            }\\r\\n\\r\\n            p.center {\\r\\n                text-align: center;\\r\\n            }\\r\\n\\r\\n        /* Buttons ------------------------------ */\\r\\n        .button {\\r\\n            display: inline-block;\\r\\n            width: 200px;\\r\\n            background-color: #414EF9;\\r\\n            border-radius: 3px;\\r\\n            color: #ffffff;\\r\\n            font-size: 15px;\\r\\n            line-height: 45px;\\r\\n            text-align: center;\\r\\n            text-decoration: none;\\r\\n            -webkit-text-size-adjust: none;\\r\\n            mso-hide: all;\\r\\n        }\\r\\n\\r\\n        .button--green {\\r\\n            background-color: #28DB67;\\r\\n        }\\r\\n\\r\\n        .button--red {\\r\\n            background-color: #FF3665;\\r\\n        }\\r\\n\\r\\n        .button--blue {\\r\\n            background-color: #414EF9;\\r\\n        }\\r\\n\\r\\n        /*Media Queries ------------------------------ */\\r\\n        @media only screen and (max-width: 600px) {\\r\\n            .email-body_inner,\\r\\n            .email-footer {\\r\\n                width: 100% !important;\\r\\n            }\\r\\n        }\\r\\n\\r\\n        @media only screen and (max-width: 500px) {\\r\\n            .button {\\r\\n                width: 100% !important;\\r\\n            }\\r\\n        }\\r\\n    </style>\\r\\n</head>\\r\\n<body>\\r\\n    <table class=\\\"email-wrapper\\\" width=\\\"100%\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\">\\r\\n        <tr>\\r\\n            <td align=\\\"center\\\">\\r\\n                <table class=\\\"email-content\\\" width=\\\"100%\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\">\\r\\n                    <!-- Logo -->\\r\\n                    <tr>\\r\\n                        <td class=\\\"email-masthead\\\">\\r\\n                            <a class=\\\"email-masthead_name\\\">PLAYER2</a>\\r\\n                        </td>\\r\\n                    </tr>\\r\\n                    <!-- Email Body -->\\r\\n                    <tr>\\r\\n                        <td class=\\\"email-body\\\" width=\\\"100%\\\">\\r\\n                            <table class=\\\"email-body_inner\\\" align=\\\"center\\\" width=\\\"570\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\">\\r\\n                                <!-- Body content -->\\r\\n                                <tr>\\r\\n                                    <td class=\\\"content-cell\\\">\\r\\n                                        <h1>Recover your password</h1>\\r\\n                                        <p>Please, click on the follow link to recover your password. This link is active for two hours, then you need to generate a new one.</p>\\r\\n                                        <!-- Action -->\\r\\n                                        <table class=\\\"body-action\\\" align=\\\"center\\\" width=\\\"100%\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\">\\r\\n                                            <tr>\\r\\n                                                <td align=\\\"center\\\">\\r\\n                                                    <div>\\r\\n                                                        <!--[if mso]><v:roundrect xmlns:v=\\\"urn:schemas-microsoft-com:vml\\\" xmlns:w=\\\"urn:schemas-microsoft-com:office:word\\\" href=\\\"{{action_url}}\\\" style=\\\"height:45px;v-text-anchor:middle;width:200px;\\\" arcsize=\\\"7%\\\" stroke=\\\"f\\\" fill=\\\"t\\\">\\r\\n                                                          <v:fill type=\\\"tile\\\" color=\\\"#414EF9\\\" />\\r\\n                                                          <w:anchorlock/>\\r\\n                                                          <center style=\\\"color:#ffffff;font-family:sans-serif;font-size:15px;\\\">Verify Email</center>\\r\\n                                                        </v:roundrect><![endif]-->\\r\\n                                                        <a href=\\\"https://www.player2.store/newpassword.html?u17=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhbmRyZS5wYXJpc0B5YWhvby5jb20uYnIiLCJqdGkiOiJhYThiMTljY2RjYjg0YmE0YTM4NzRmOTAwNDY2MmVlMyIsInVuaXF1ZV9uYW1lIjoiYW5kcmUucGFyaXNAeWFob28uY29tLmJyIiwiZXhwIjoxNjE3NjcyMzIwfQ.bguq_innXFiGWAyBh_uMziGD5aNu_uU-D0MBaZIMIGU\\\" class=\\\"button button--blue\\\">Verify Email</a>\\r\\n                                                    </div>\\r\\n                                                </td>\\r\\n                                            </tr>\\r\\n                                        </table>\\r\\n                                        <p>Thanks,<br>The PLAYER2 Team</p>\\r\\n                                        <!-- Sub copy -->\\r\\n                                    </td>\\r\\n                                </tr>\\r\\n                            </table>\\r\\n                        </td>\\r\\n                    </tr>\\r\\n                    <tr>\\r\\n                        <td>\\r\\n                            <table class=\\\"email-footer\\\" align=\\\"center\\\" width=\\\"570\\\" cellpadding=\\\"0\\\" cellspacing=\\\"0\\\">\\r\\n                                <tr>\\r\\n                                    <td class=\\\"content-cell\\\">\\r\\n                                        <p class=\\\"sub center\\\">\\r\\n                                            PLAYER2, Inc.\\r\\n                                            <br>From Brazil\\r\\n                                        </p>\\r\\n                                    </td>\\r\\n                                </tr>\\r\\n                            </table>\\r\\n                        </td>\\r\\n                    </tr>\\r\\n                </table>\\r\\n            </td>\\r\\n        </tr>\\r\\n    </table>\\r\\n</body>\\r\\n</html>\",\"Subject\":\"User Teste, change your password of PLAYER2 here!\"}}"
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
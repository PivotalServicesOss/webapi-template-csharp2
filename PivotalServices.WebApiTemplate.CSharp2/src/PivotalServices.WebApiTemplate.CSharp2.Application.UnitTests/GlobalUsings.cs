global using FluentValidation;
global using FluentValidation.Results;
global using FluentAssertions;
global using FluentAssertions.Common;

global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Logging;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Microsoft.AspNetCore.Routing;
global using Microsoft.AspNetCore.Mvc.Controllers;
global using Microsoft.OpenApi.Models;
global using Moq;

global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Globalization;
global using System.IO;
global using System.Linq;
global using System.Net;
global using System.Net.Http;
global using System.Net.Mime;
global using System.Threading.Tasks;
global using System.Text.Json.Serialization;
global using System.Text.Json;
global using System.Text.RegularExpressions;

global using Swashbuckle.AspNetCore.SwaggerGen;

global using Xunit;

global using PivotalServices.WebApiTemplate.CSharp2.Shared.Mvc;
global using PivotalServices.WebApiTemplate.CSharp2.Shared.Serialization;
global using PivotalServices.WebApiTemplate.CSharp2.Shared.Logging;
global using PivotalServices.WebApiTemplate.CSharp2.Shared.Documentation;


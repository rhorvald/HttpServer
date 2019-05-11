﻿using HTTPServer.Core.Abstractions;
using HTTPServer.Core.Attributes;
using HTTPServer.Core.Exceptions;
using HTTPServer.Core.Model;
using HTTPServer.Core.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTPServer.Core.Controllers
{
    [Controller("delete")]
    public class DeleteController
    {
        CountryRepository _countryRepository;
        PeopleRepository _peopleRepository;

        public DeleteController(CountryRepository countryRepository, PeopleRepository peopleRepository)
        {
            _countryRepository = countryRepository;
            _peopleRepository = peopleRepository;
        }

        [Page(@"[0-9]+")]
        public IHttpJsonResponse Get(string table, int i)
        {
            switch (table)
            {
                case "countries":
                    _countryRepository.Delete(i);
                    break;
                case "peoples":
                    _peopleRepository.Delete(i);
                    break;
                default:
                    throw new HttpStatusCodeException(HttpStatusCode.NotFound);
            }

            return new HttpJsonResponse();
        }

        [Page(@"index")]
        public IHttpJsonResponse GetIndex()
        {
            var methodsInfo = this.GetType().GetMethods();
            return new HttpJsonResponse { Data =  JsonConvert.SerializeObject(methodsInfo.Select(t => new { Name = t.Name, ReturnType = t.ReturnType, Params = t.GetParameters().Select(k => $"{k.ParameterType.Name} {k.Name}") }), Formatting.Indented) };
        }


        [Error(HttpStatusCode.NotFound, "Page with current index not found")]
        public IHttpJsonResponse NotFound()
        {
            var myAttribute = GetType().GetMethod("NotFound").GetCustomAttributes(true).OfType<ErrorAttribute>().FirstOrDefault();
            return new HttpJsonResponse { Data = $"<html><head><meta charset='utf8'></head><body>{myAttribute.Description}</body></html>" };
        }
    }
}

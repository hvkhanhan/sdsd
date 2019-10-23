﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using MultipartDataMediaFormatter;
using MultipartDataMediaFormatter.Infrastructure;

namespace WebApplication3
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            var cors = new EnableCorsAttribute("http://localhost:4200", headers: "*", methods: "*");//origins,headers,methods   
            config.EnableCors(cors);

            // Web API configuration and services
            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "trainees",
                routeTemplate: "api/{controller}/{action}/{activationcode}",
                defaults: new { activationcode = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "mentors",
                routeTemplate: "api/{controller}/{action}/{activationcode}",
                defaults: new { activationcode = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ActiontApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Formatters.Add(new FormMultipartEncodedMediaTypeFormatter(new MultipartFormatterSettings()));
        }
    }
}

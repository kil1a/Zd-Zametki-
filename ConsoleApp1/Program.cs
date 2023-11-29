using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;


HttpListener listener = new HttpListener();

    listener.Prefixes.Add("http://localhost:8080/");
    listener.Start();
    Console.WriteLine("Сервер запущен. Ожидание запросов...");

    while (true)
    {
        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        response.Headers.Add("Access-Control-Allow-Origin", "*");
        response.Headers.Add("Content-Type", "application/json");

    }


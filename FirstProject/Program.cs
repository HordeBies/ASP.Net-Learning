var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run(async (HttpContext context) =>
//{
//    //context.Response.StatusCode = 400;
//    context.Response.Headers["Content-type"] = "text/html";
//    if (context.Request.Headers.ContainsKey("AuthorizationKey"))
//    {
//        string authKey = context.Request.Headers["AuthorizationKey"];
//        await context.Response.WriteAsync($"<p>AuthorizationKey: {authKey}</p>\n");
//    }
//    await context.Response.WriteAsync("HelloWorld!\n");
//    await context.Response.WriteAsync("Bies");
//});

//app.Run(async (context) =>
//{
//    StreamReader reader = new(context.Request.Body);
//    string body = await reader.ReadToEndAsync();

//    var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body);
//    if (query.ContainsKey("age"))
//    {
//        string age = query["age"][0];
//        await context.Response.WriteAsync(age);
//    }

//});

//var operationList = new Dictionary<string,Func<int,int,int>>()
//{
//    { "add",(a,b)=> a+b},
//    { "substract",(a,b)=> a-b},
//    { "multiply",(a,b)=> a*b},
//    { "divide",(a,b)=> a/b},
//    { "modulo",(a,b)=> a%b},
//};
//app.Run(async (context) =>
//{
//    context.Response.Headers["Content-type"] = "text/html";
//    bool success = true;
//    string operation = "none";
//    if(context.Request.Method != "GET")
//    {
//        if (success) context.Response.StatusCode = 400;
//        await context.Response.WriteAsync("Invalid Request Method");
//        success = false;
//    }
//    if (!context.Request.Query.ContainsKey("firstNumber"))
//    {
//        if(success) context.Response.StatusCode = 400;
//        await context.Response.WriteAsync("Invalid input for 'firstNumber'\n");
//        success = false;
//    }
//    if (!context.Request.Query.ContainsKey("secondNumber"))
//    {
//        if (success) context.Response.StatusCode = 400;
//        await context.Response.WriteAsync("Invalid input for 'secondNumber'\n");
//        success = false;
//    }
//    if (!context.Request.Query.ContainsKey("operation"))
//    {
//        if (success) context.Response.StatusCode = 400;
//        await context.Response.WriteAsync("Invalid input for 'operation'\n");
//        success = false;
//    }
//    else
//    {
//        operation = context.Request.Query["operation"];
//        if (!operationList.ContainsKey(operation))
//        {
//            if (success) context.Response.StatusCode = 400;
//            await context.Response.WriteAsync("Invalid input for 'operation'\n");
//            success = false;
//        }
//    }
//    if (success)
//    {
//        context.Response.StatusCode = 200;
//        int firstNumber = int.Parse(context.Request.Query["firstNumber"]);
//        int secondNumber = int.Parse(context.Request.Query["secondNumber"]);
//        var result = operationList[operation](firstNumber, secondNumber);
//        await context.Response.WriteAsync($"{result}\n");
//    }

//});

app.Run();

using AspNetCoreRateLimit;
using EcommerceBackend.Models;
using EcommerceBackend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register DbContext BEFORE builder.Build()
builder.Services.AddDbContext<ShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<TokenService>();
// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // This tells System.Text.Json to handle cycles gracefully
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

        // Optional: make property names camelCase for Angular
        options.JsonSerializerOptions.PropertyNamingPolicy =
            System.Text.Json.JsonNamingPolicy.CamelCase;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<ShippingService>();
builder.Services.AddScoped<PayPalService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // explicitly allow Angular dev server
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // if you ever use cookies
    });
});

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();



// Option 1: Add basic HttpClient support (recommended for simple cases)
builder.Services.AddHttpClient();

// Option 2: Add a typed HttpClient for ShippingService (recommended for scalability)
builder.Services.AddHttpClient<ShippingService>();var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ShopDbContext>();
    DbInitializer.Seed(context);
}

app.UseIpRateLimiting();
app.UseWebSockets();

app.Map("/ws/orders/{orderId}", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var orderId = int.Parse((string)context.Request.RouteValues["orderId"]);
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();

        // Example: push status updates
        var statusUpdate = Encoding.UTF8.GetBytes("{\"status\":\"Shipped\"}");
        await webSocket.SendAsync(new ArraySegment<byte>(statusUpdate),
                                  WebSocketMessageType.Text, true, CancellationToken.None);
    }
    else
    {
        context.Response.StatusCode = 400;
    }
});

app.Run();

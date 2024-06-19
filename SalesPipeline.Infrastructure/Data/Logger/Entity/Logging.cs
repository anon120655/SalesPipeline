using System;
using System.Collections.Generic;

namespace SalesPipeline.Infrastructure.Data.Logger.Entity;

public partial class Logging
{
    public Guid LogId { get; set; }

    public DateTime RequestDate { get; set; }

    public string? Method { get; set; }

    public string? Scheme { get; set; }

    public string? Host { get; set; }

    public string? Path { get; set; }

    public string? Query { get; set; }

    public string? ContentType { get; set; }

    public string? RequestBody { get; set; }

    public string? ResponseStatus { get; set; }

    public string? ResponseContentType { get; set; }

    public string? DeviceInfo { get; set; }

    public string? ResponseBody { get; set; }

    public string? ClientIp { get; set; }

    public DateTime ResponseDate { get; set; }

    public string? ExceptionMessage { get; set; }
}
